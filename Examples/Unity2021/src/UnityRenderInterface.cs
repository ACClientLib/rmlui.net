using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Assertions;
using RmlUiNet;
using System;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;
using static UnityEngine.UI.Image;
using System.Xml.Linq;

namespace URmlUi
{
	public class UnityRenderInterface : RenderInterface
	{
		private class DrawInfo
		{
			public Mesh mesh;
			public Material material;

			public DrawInfo()
			{
				mesh = new Mesh();
				mesh.MarkDynamic();
			}

			public void Destroy()
			{
				Object.Destroy(mesh);
				Object.Destroy(material);
			}
		}

		private CommandBuffer m_CurrentBuffer;
		private Material m_Material;

		private Stack<DrawInfo> m_Drawables;
		private List<Texture> m_Textures;
		private Vector2 m_DisplaySize;
		private Vector2 m_ContextSize;

		public UnityRenderInterface(Material material, Vector2 dispSize, Vector2 ctxSize) : base()
		{
			m_Material = material;
			m_Textures = new List<Texture>();
			m_Drawables = new Stack<DrawInfo>();

			m_DisplaySize = dispSize;
			m_ContextSize = ctxSize;
		}

		public void SetCommandBuffer(CommandBuffer cb)
		{
			m_CurrentBuffer = cb;
		}

		public void StartFrame()
		{
			// otherwise they stack up to absurd levels.
			m_CurrentBuffer.Clear();

			// destroy all existing drawables
			foreach (var drawable in m_Drawables)
			{
				drawable.Destroy();
			}
			m_Drawables.Clear();
		}

		public override void RenderGeometry(Vertex[] vertices, int vertexCount, int[] indices, int indexCount, ulong texture, System.Numerics.Vector2 translation)
		{
			m_CurrentBuffer.SetViewport(new Rect(0, 0, m_DisplaySize.x, m_DisplaySize.y));
			m_CurrentBuffer.SetViewProjectionMatrices(
				Matrix4x4.Translate(new Vector3(0.5f / m_ContextSize.x, 0.5f / m_ContextSize.y, 0f)),
				Matrix4x4.Ortho(0f, m_ContextSize.x, m_ContextSize.y, 0f, 0f, 1f)
			);

			DrawInfo drawable = new DrawInfo
			{
				material = Object.Instantiate(m_Material)
			};

			if (texture != 0)
			{
				Assert.IsNotNull(m_Textures[(int)texture - 1], "Invalid texture handle!");
				drawable.material.mainTexture = m_Textures[(int)texture - 1];
			}
			else
			{
				drawable.material.mainTexture = Texture2D.whiteTexture;
			}

			// create separate vertex, uv, and color arrays
			Vector3[] finalVerts = new Vector3[vertexCount];
			Vector2[] finalUVs = new Vector2[vertexCount];
			Color[] finalColors = new Color[vertexCount];

			for (int i = 0; i < vertexCount; i++)
			{
				finalVerts[i] = new Vector3(
					vertices[i].Position.X + translation.X,
					vertices[i].Position.Y + translation.Y,
					0.0f
				);

				finalUVs[i] = new Vector2(
					vertices[i].TextureCoordinates.X,
					vertices[i].TextureCoordinates.Y
				);

				finalColors[i] = new Color(
					vertices[i].Colour.Red / 255.0f,
					vertices[i].Colour.Green / 255.0f,
					vertices[i].Colour.Blue / 255.0f,
					vertices[i].Colour.Alpha / 255.0f
				);
			}

			drawable.mesh.SetVertices(finalVerts, 0, vertexCount);
			drawable.mesh.SetIndices(indices, MeshTopology.Triangles, 0);
			drawable.mesh.SetUVs(0, finalUVs);
			drawable.mesh.SetColors(finalColors);
			drawable.mesh.UploadMeshData(false);

			m_CurrentBuffer.DrawMesh(drawable.mesh, Matrix4x4.identity, drawable.material);
			m_Drawables.Push(drawable);
		}

		public override bool LoadTexture(out ulong textureHandle, out Vector2i textureDimensions, string source)
		{
			string path = source;
			// Swap separators if we determine that we're using the wrong ones
			if (!path.Contains(Path.DirectorySeparatorChar) && path.Contains(Path.AltDirectorySeparatorChar))
				path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			path = Path.Combine(Application.streamingAssetsPath, "rml", path);

			if (!File.Exists(path)) {
				Debug.LogError($"Couldn't find image {source} in RmlUi Files!");

				textureHandle = 0;
				textureDimensions = new Vector2i(0, 0);
				return false;
			}

			Texture2D tex = new Texture2D(1, 1);

			ImageConversion.LoadImage(tex, File.ReadAllBytes(path));
			tex.Apply();

			// flip texture bytes, if we're loading an image there's a 90% chance we need to flip it
			var originalPixels = tex.GetPixels();

			var newPixels = new Color[originalPixels.Length];

			var width = tex.width;
			var rows = tex.height;

			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < rows; y++)
				{
					newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
				}
			}

			tex.SetPixels(newPixels);
			tex.Apply();

			m_Textures.Add(tex);

			textureDimensions = new Vector2i(tex.width, tex.height);
			textureHandle = (ulong)m_Textures.Count;

			return true;
		}

		public override void ReleaseTexture(IntPtr textureHandle)
		{
			Assert.IsTrue(textureHandle != IntPtr.Zero);
			Assert.IsNotNull(m_Textures[(int)textureHandle - 1], "Invalid texture handle!");

			// wipe out this texture from the list, GC will take care of it.
			m_Textures[(int)textureHandle - 1] = null;
		}

		public override bool GenerateTexture(out ulong textureHandle, byte[] source, int sourceSize, Vector2i sourceDimensions)
		{
			Texture2D tex = new Texture2D(sourceDimensions.X, sourceDimensions.Y, TextureFormat.RGBA32, false, false);
			// we don't flip here because internal textures should have the right orientation
			tex.LoadRawTextureData(source);
			tex.Apply();

			m_Textures.Add(tex);
			textureHandle = (ulong)m_Textures.Count;

			return true;
		}
	}
}
