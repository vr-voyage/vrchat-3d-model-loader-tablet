
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorMaterialController_Filamented : INodeInspectorMaterialController
{
    public RawImage albedo;
    public RawImage normal;
    public RawImage metal;
    public RawImage emission;
    public RawImage heightMap;
    public RawImage occlusion;

    public Image          albedoColor;
    public TMPro.TMP_Text normalScale;
    public TMPro.TMP_Text metalValue;
    public TMPro.TMP_Text smoothValue;
    public Image          emissionColor;

    public TMPro.TMP_Text renderQueue;
    public TMPro.TMP_Text[] tilingOffset;

    public Toggle normalShadows;
    public TMPro.TMP_Text variance;
    public TMPro.TMP_Text threshold;

    public TMPro.TMP_Text heightScale;
    public TMPro.TMP_Text hardness;

    public Toggle lightmapSpecular;
    public TMPro.TMP_Text maxSmoothness;

    public Toggle vrcLightVolumes;
    public TMPro.TMP_Text lightVolumeSurfaceBias;

    public Toggle exposureOcclusionEnabled;
    public TMPro.TMP_Text exposureOcclusionLevel;

    public Toggle specularHighlights;
    public Toggle reflections;


    public override void DisplayMaterial(Material m)
    {
        base.DisplayMaterial(m);

        object[] properties = new object[]
        {
            new object[] {"_MainTex", PROPERTY_TEXTURE, albedo},
            new object[] {"_BumpMap", PROPERTY_TEXTURE, normal},
            new object[] {"_MetallicGlossMap", PROPERTY_TEXTURE, metal},
            new object[] {"_EmissionMap", PROPERTY_TEXTURE, emission},
            new object[] {"_ParallaxMap", PROPERTY_TEXTURE, heightMap},
            new object[] {"_OcclusionMap", PROPERTY_TEXTURE, occlusion },
            new object[] {"_Color", PROPERTY_COLOR, albedoColor},
            new object[] {"_BumpScale", PROPERTY_FLOAT, normalScale},
            new object[] {"_Metallic", PROPERTY_FLOAT, metalValue},
            new object[] {"_Glossiness", PROPERTY_FLOAT, smoothValue},
            new object[] {"_EmissionColor", PROPERTY_COLOR, emissionColor },
            new object[] {"_specularAntiAliasingVariance", PROPERTY_FLOAT, variance },
            new object[] {"_specularAntiAliasingThreshold", PROPERTY_FLOAT, threshold},
            new object[] {"_BumpShadowHeightScale", PROPERTY_FLOAT, heightScale},
            new object[] {"_BumpShadowHardness", PROPERTY_FLOAT, hardness },
            new object[] {"_LightmapSpecular", PROPERTY_FLOAT, lightmapSpecular},
            new object[] {"_LightmapSpecularMaxSmoothness", PROPERTY_FLOAT, maxSmoothness},
            new object[] {"_VRCLV", PROPERTY_FLOAT, vrcLightVolumes},
            new object[] {"_VRCLVSurfaceBias", PROPERTY_FLOAT, lightVolumeSurfaceBias},
            new object[] {"_ExposureOcclusion", PROPERTY_FLOAT, exposureOcclusionEnabled},
            new object[] {"_ExposureOcclusion", PROPERTY_FLOAT, exposureOcclusionLevel},
            new object[] {"_SpecularHighlights", PROPERTY_FLOAT, specularHighlights},
            new object[] {"_GlossyReflections", PROPERTY_FLOAT, reflections},
            new object[] {"_MainTex", PROPERTY_TILINGOFFSET, tilingOffset }
        };

        SetProperties(m, properties);
        if (renderQueue != null) { renderQueue.text = m.renderQueue.ToString(); }
    }
}
