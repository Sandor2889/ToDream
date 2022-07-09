using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

//[ExecuteAlways]
public class Water : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    [SerializeField] private WaterResources _waterResources;
    [SerializeField] private WaterSurfaceData _waterSufData;

    [SerializeField] private Texture2D _rampTexture;

    private static readonly int _absorptionScatteringRamp = Shader.PropertyToID("_AbsorptionScatteringRamp");



    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        //MyPreRender(_cam);
    }

    private void OnEnable()
    {
        GenerateColorRamp();
    }

    private void MyPreRender(Camera cam)
    {
        // Water matrix
        const float quantizeValue = 6.25f;
        const float forwards = 10f;
        const float yOffset = -0.25f;

        var newPos = cam.transform.TransformPoint(Vector3.forward * forwards);
        newPos.y = yOffset;
        newPos.x = quantizeValue * (int)(newPos.x / quantizeValue);
        newPos.z = quantizeValue * (int)(newPos.z / quantizeValue);

        var matrix = Matrix4x4.TRS(newPos + transform.position, Quaternion.identity, transform.localScale); // transform.localToWorldMatrix;

        foreach (var mesh in _waterResources.defaultWaterMeshes)
        {
            Graphics.DrawMesh(mesh,
                matrix,
                _waterResources.defaultSeaMaterial,
                gameObject.layer,
                cam,
                0,
                null,
                ShadowCastingMode.Off,
                true,
                null,
                LightProbeUsage.Off,
                null);
        }
    }


    private void GenerateColorRamp()
    {
        if (_rampTexture == null)
            _rampTexture = new Texture2D(128, 4, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None);
        _rampTexture.wrapMode = TextureWrapMode.Clamp;

        var defaultFoamRamp = _waterResources.defaultFoamRamp;

        var cols = new Color[512];
        for (var i = 0; i < 128; i++)
        {
            cols[i] = _waterSufData._absorptionRamp.Evaluate(i / 128f);
        }
        for (var i = 0; i < 128; i++)
        {
            cols[i + 128] = _waterSufData._scatterRamp.Evaluate(i / 128f);
        }
        for (var i = 0; i < 128; i++)
        {
            switch (_waterSufData._foamSettings.foamType)
            {
                case 0: // default
                    cols[i + 256] = defaultFoamRamp.GetPixelBilinear(i / 128f, 0.5f);
                    break;
                //case 1: // simple
                //    cols[i + 256] = defaultFoamRamp.GetPixelBilinear(_waterSufData._foamSettings.basicFoam.Evaluate(i / 128f), 0.5f);
                //    break;
                //case 2: // custom
                //    cols[i + 256] = Color.black;
                //    break;
            }

            _rampTexture.SetPixels(cols);
            _rampTexture.Apply();
            Shader.SetGlobalTexture(_absorptionScatteringRamp, _rampTexture);
        }
    }
}
