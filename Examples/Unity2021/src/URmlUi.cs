using System.Collections.Generic;
using UnityEngine;
using RmlUiNet;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;
using UnityEngine.InputSystem;

namespace URmlUi
{
	public class URmlUi : MonoBehaviour
	{
		private readonly (Key, RmlUiNet.Input.KeyIdentifier)[] m_SpecialKeys =
		{
			( Key.Backspace, RmlUiNet.Input.KeyIdentifier.KI_BACK ),
			( Key.LeftArrow, RmlUiNet.Input.KeyIdentifier.KI_LEFT ),
			( Key.RightArrow, RmlUiNet.Input.KeyIdentifier.KI_RIGHT ),
			( Key.DownArrow, RmlUiNet.Input.KeyIdentifier.KI_DOWN ),
			( Key.UpArrow, RmlUiNet.Input.KeyIdentifier.KI_UP ),
			( Key.Enter, RmlUiNet.Input.KeyIdentifier.KI_RETURN ),
			( Key.Escape, RmlUiNet.Input.KeyIdentifier.KI_ESCAPE ),
			( Key.Tab, RmlUiNet.Input.KeyIdentifier.KI_TAB ),
			( Key.Delete, RmlUiNet.Input.KeyIdentifier.KI_DELETE )
		};

		[SerializeField]
		private RenderRmlUi m_RenderFeature = null;
		[SerializeField, Tooltip("The resolution applied to the context.")]
		private Vector2 m_ReferenceResolution;

		private UnityRenderInterface m_RenderInterface;
		private UnitySystemInterface m_SystemInterface;
		private UnityFileInterface m_FileInterface;
		private CommandBuffer m_CommandBuffer;

		private Context? m_Context;
		private ElementDocument m_Document;

		// usually just the standard sprite material
		public Material RmlUiMaterial;
		public Camera Camera;

		private readonly List<char> m_TextInput = new List<char>();

		private void OnEnable()
		{
			if (Camera == null)
			{
				throw new System.Exception("No Camera!");
			}

			if (RmlUiMaterial == null)
			{
				throw new System.Exception("No Material!");
			}

			if (m_RenderFeature == null)
			{
				throw new System.Exception("No RenderFeature!");
			}

			m_RenderInterface = new UnityRenderInterface(RmlUiMaterial, Camera.pixelRect.size, m_ReferenceResolution);
			m_SystemInterface = new UnitySystemInterface();
			m_FileInterface = new UnityFileInterface();

			m_CommandBuffer = CommandBufferPool.Get("URmlUi");
			m_RenderFeature.Camera = Camera;
			m_RenderFeature.CommandBuffer = m_CommandBuffer;
			m_RenderInterface.SetCommandBuffer(m_CommandBuffer);

			Rml.SetRenderInterface(m_RenderInterface);
			Rml.SetSystemInterface(m_SystemInterface);
			Rml.SetFileInterface(m_FileInterface);

			Rml.Initialise();
			// you'll need a ttf/otf file to load
			//Rml.LoadFontFace(<FONT PATH>);

			m_Context = Rml.CreateContext("URmlUiBase", new Vector2i((int)m_ReferenceResolution.x, (int)m_ReferenceResolution.y));

			Keyboard.current.onTextInput += m_TextInput.Add;

			if (m_Context != null)
			{
				//m_Document = m_Context?.LoadDocument(<DOCUMENT PATH>);
				m_Document.Show();
			}
		}

		private void OnDisable()
		{
			if (m_Document != null)
			{
				m_Document.Close();
			}

			Keyboard.current.onTextInput -= m_TextInput.Add;

			Rml.Shutdown();

			if (m_CommandBuffer != null)
			{
				CommandBufferPool.Release(m_CommandBuffer);
			}

			if (m_RenderFeature != null)
			{
				m_RenderFeature.Camera = null;
				m_RenderFeature.CommandBuffer = null;
			}
		}

		// update context and input at a fixed rate, this is what the input system uses
		private void FixedUpdate()
		{
			// input management
			{
				var mouse = Mouse.current;
				var mPos = new Vector2(mouse.position.value.x, Camera.pixelRect.size.y - mouse.position.value.y);
				var mPercentage = mPos / Camera.pixelRect.size;

				var mRefPos = mPercentage * m_ReferenceResolution;

				m_Context.ProcessMouseMove((int)mRefPos.x, (int)mRefPos.y, 0);

				if (mouse.leftButton.wasPressedThisFrame)
				{
					m_Context.ProcessMouseButtonDown((int)RmlUiNet.Input.MouseButton.MB_LEFT, 0);
				}
				if (mouse.leftButton.wasReleasedThisFrame)
				{
					m_Context.ProcessMouseButtonUp((int)RmlUiNet.Input.MouseButton.MB_LEFT, 0);
				}

				if (mouse.rightButton.wasPressedThisFrame)
				{
					m_Context.ProcessMouseButtonDown((int)RmlUiNet.Input.MouseButton.MB_RIGHT, 0);
				}
				if (mouse.rightButton.wasReleasedThisFrame)
				{
					m_Context.ProcessMouseButtonUp((int)RmlUiNet.Input.MouseButton.MB_RIGHT, 0);
				}

				if (mouse.middleButton.wasPressedThisFrame)
				{
					m_Context.ProcessMouseButtonDown((int)RmlUiNet.Input.MouseButton.MB_MIDDLE, 0);
				}
				if (mouse.middleButton.wasReleasedThisFrame)
				{
					m_Context.ProcessMouseButtonUp((int)RmlUiNet.Input.MouseButton.MB_MIDDLE, 0);
				}
			}

			{
				foreach (var keyTup in m_SpecialKeys)
				{
					if (Keyboard.current[keyTup.Item1].isPressed)
					{
						// don't process textinput if we're using a special character
						m_TextInput.Clear();
					}

					if (Keyboard.current[keyTup.Item1].wasPressedThisFrame)
					{
						m_Context.ProcessKeyDown(keyTup.Item2, 0);
					}
					if (Keyboard.current[keyTup.Item1].wasReleasedThisFrame)
					{
						m_Context.ProcessKeyUp(keyTup.Item2, 0);
					}
				}

				if (m_TextInput.Count > 0)
				{
					m_Context.ProcessTextInput(new string(m_TextInput.ToArray()));
					m_TextInput.Clear();
				}
			}

			m_Context.Update();
		}

		private void Update()
		{
			m_RenderInterface.StartFrame();
			m_Context.Render();
		}
	}
}
