using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private WaterResources _waterResources;


    private static readonly int _CameraRoll = Shader.PropertyToID("_CameraRoll");
    private static readonly int _InvViewProjection = Shader.PropertyToID("_InvViewProjection");

    private void OnEnable()
    {
        Camera.onPreRender += MyPreRender;
        Debug.Log("start");
    }
    

    private void MyPreRender(Camera cam)
    {
        //var roll = cam.transform.localEulerAngles.z;
        //Shader.SetGlobalFloat(_CameraRoll, roll);
        //Shader.SetGlobalMatrix(_InvViewProjection,
        //    (GL.GetGPUProjectionMatrix(cam.projectionMatrix, false) * cam.worldToCameraMatrix).inverse);

        // Water matrix
        const float quantizeValue = 6.25f;
        const float forwards = 10f;
        const float yOffset = -0.25f;

        var newPos = cam.transform.TransformPoint(Vector3.forward * forwards);
        newPos.y = yOffset;
        newPos.x = quantizeValue * (int)(newPos.x / quantizeValue);
        newPos.z = quantizeValue * (int)(newPos.z / quantizeValue);
        Debug.Log(newPos.x + ", " + newPos.y + ", " + newPos.z);

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

    private void OnDisable()
    {
        Camera.onPreRender -= MyPreRender;
        Debug.Log("end");
    }
}
