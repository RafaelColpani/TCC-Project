using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcess : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public RenderTargetIdentifier source;

        //---------
        

        //------------

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {

        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get();
            Blit(commandBuffer, source, source);
            
        }


        public override void FrameCleanup(CommandBuffer cmd)
        {

        }
    }

    //------------------


    [System.Serializable]
    public class Settings{
        public Material mateiral = null;
    }

    public Settings settings = new Settings();

    //--------------------

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();

        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData rendertingData)
    {
        m_ScriptablePass.source = renderer.cameraColorTarget;
        renderer.EnqueuePass(m_ScriptablePass);  
    }

}
