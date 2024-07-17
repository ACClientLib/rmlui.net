using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URmlUi
{
	public class RenderRmlUi : ScriptableRendererFeature
	{
		private class CommandBufferPass : ScriptableRenderPass
		{
			public CommandBuffer commandBuffer;

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				context.ExecuteCommandBuffer(commandBuffer);
			}
		}

		[HideInInspector]
		public Camera Camera;
		public CommandBuffer CommandBuffer;
		// fuck it include UI in post processing?
		public RenderPassEvent RenderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
		
		private CommandBufferPass m_RenderPass;

		public override void Create()
		{
			m_RenderPass = new CommandBufferPass()
			{
				commandBuffer = CommandBuffer,
				renderPassEvent = RenderPassEvent,
			};
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (CommandBuffer == null) return;
			if (Camera != renderingData.cameraData.camera) return;

			m_RenderPass.renderPassEvent = RenderPassEvent;
			m_RenderPass.commandBuffer = CommandBuffer;

			renderer.EnqueuePass(m_RenderPass);
		}
	}
}
