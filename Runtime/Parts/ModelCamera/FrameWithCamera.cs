
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class FrameWithCamera : UdonSharpBehaviour
{
    public Camera cam;

    public GameObject debugObject;

    public void Focus(GameObject target)
    {
        Debug.Log("Focus");

        FocusCameraOnObject(cam, target, 0.4f);

        //cam.TryGetFocusTransforms(target);
    }

    /// <summary>
    /// Positions and frames a camera so the entire target GameObject fits in the view.
    /// Works with both Perspective and Orthographic cameras.
    /// </summary>
    /// <param name="cam">Target Camera</param>
    /// <param name="target">GameObject to frame (e.g., House)</param>
    /// <param name="margin">Padding ratio (0.0 to 0.5). 0.1 = 10% padding</param>
    /// <param name="viewDirection">Optional. Direction the camera should face. Defaults to current forward.</param>
    public void FocusCameraOnObject(Camera cam, GameObject target, float margin = 0.1f)
    {
        if (cam == null || target == null) return;

        /* Bounds.Encapsulate is broken on my Unity installation.
           Doing new Bounds().Encapsulate(veryBigBounds).size() will return 0
           So... here we are ! Reimplementing Unity BOUNDS CALCULATION BY HAND ! */
        /* So most of this code is vibe'd except that part which required manual intervention */ 
        Vector3[] minMax = new Vector3[2] { Vector3.zero, Vector3.zero };

        Vector3 boundingBoxSize = GetBoundingBoxSize(target, minMax);
        Vector3 center = minMax[0] + (boundingBoxSize * 0.5f);
        Vector3 extents = boundingBoxSize * 0.5f;

        Debug.Log($"<color=cyan>[{minMax[0]} - {minMax[1]}] from {target.transform.position} Size: {boundingBoxSize} Center: {center} Extents: {extents}</color>");

        if (debugObject != null) debugObject.transform.position = center;

        if (extents.sqrMagnitude < Mathf.Epsilon)
        {
            Debug.LogWarning("Target has no valid bounds. Ensure it contains active Mesh/SkinnedMeshRenderers.");
            return;
        }

        // Safe aspect ratio handling (important when using RenderTextures)
        float aspect = cam.aspect;
        if (aspect <= 0f && cam.targetTexture != null)
            aspect = (float)cam.targetTexture.width / cam.targetTexture.height;
        if (aspect <= 0f) aspect = 1f; // Fallback to 1:1

        // Determine viewing direction
        Vector3 dir = cam.transform.forward;

        // Position camera
        float distance = GetPerspectiveDistance(extents, cam.fieldOfView, aspect, margin);
        cam.transform.position = center - dir * distance;
        cam.transform.LookAt(center);
        
    }

    private static Vector3 GetBoundingBoxSize(GameObject root, Vector3[] minMax)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

        int nRenderers = renderers.Length;
        if (nRenderers == 0)
        {
            return Vector3.zero;
        }

        Vector3 min = renderers[0].bounds.min;
        Vector3 max = renderers[0].bounds.max;

        for (int i = 1; i < nRenderers; i++)
        {
            Renderer r = renderers[i];
            Debug.Log($"<color=cyan>{r.name} - {r.bounds.min} - {r.bounds.max}</color>");
            min = Vector3.Min(min, r.bounds.min);
            max = Vector3.Max(max, r.bounds.max);
        }

        minMax[0] = min;
        minMax[1] = max;
        return max - min;
    }

    private static float GetPerspectiveDistance(Vector3 extents, float fov, float aspect, float margin)
    {
        float tanHalfFov = Mathf.Tan(fov * Mathf.Deg2Rad * 0.5f);
        float adjustedY = extents.y / (1f - margin);
        float adjustedX = extents.x / (1f - margin);

        float distHeight = adjustedY / tanHalfFov;
        float distWidth = adjustedX / (tanHalfFov * aspect);

        return Mathf.Max(distHeight, distWidth);
    }

}

