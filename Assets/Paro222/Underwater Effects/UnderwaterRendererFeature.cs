
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

public class UnderwaterRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material material;

        public RenderPassEvent renderPassEvent =
            RenderPassEvent.AfterRenderingSkybox;

        public Color color;

        public float FogDensity = 1f;

        [Range(0, 1)]
        public float alpha;

        public float refraction = 0.1f;

        public Texture normalmap;

        public Vector4 UV = new Vector4(1, 1, 0.2f, 0.1f);
    }

    public Settings settings = new Settings();

    private UnderwaterPass pass;

    class UnderwaterPass : ScriptableRenderPass
    {
        private Settings settings;

        public UnderwaterPass(Settings settings)
        {
            this.settings = settings;
        }

        public override void RecordRenderGraph(
            RenderGraph renderGraph,
            ContextContainer frameContext)
        {
            if (settings.material == null)
                return;

            // Get camera textures
            UniversalResourceData resources =
                frameContext.Get<UniversalResourceData>();

            TextureHandle source = resources.activeColorTexture;

            if (!source.IsValid())
                return;

            // Create destination texture
            TextureDesc descriptor = source.GetDescriptor(renderGraph);

            descriptor.name = "_UnderwaterColor";
            descriptor.depthBufferBits = 0;
            descriptor.clearBuffer = false;

            TextureHandle destination =
                renderGraph.CreateTexture(descriptor);

            // Set underwater material properties
            settings.material.SetFloat(
                "_FogDensity", settings.FogDensity);

            settings.material.SetFloat(
                "_alpha", settings.alpha);

            settings.material.SetColor(
                "_color", settings.color);

            settings.material.SetTexture(
                "_NormalMap", settings.normalmap);

            settings.material.SetFloat(
                "_refraction", settings.refraction);

            settings.material.SetVector(
                "_normalUV", settings.UV);

            // Apply underwater effect
            var blitParameters =
                new RenderGraphUtils.BlitMaterialParameters(
                    source,
                    destination,
                    settings.material,
                    0
                );

            renderGraph.AddBlitPass(
                blitParameters,
                "Underwater Effects"
            );

            // Use the processed texture as the camera color
            resources.cameraColor = destination;
        }
    }

    public override void Create()
    {
        pass = new UnderwaterPass(settings);
        pass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(
        ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        pass.renderPassEvent = settings.renderPassEvent;

        renderer.EnqueuePass(pass);
    }
}