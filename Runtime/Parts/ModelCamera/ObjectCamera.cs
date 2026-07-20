
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ObjectCamera : UdonSharpBehaviour
{
    public GameObject cameraAxis;
    public Camera cameraObject;

    public GameObject debugObject;

    public GameObject target;

    public void SceneLoaded()
    {
        FrameTarget(cameraAxis, cameraObject, target, 0f);
    }

    /// <srp>
    /// Call this method to reposition the camera to frame the target object perfectly.
    /// </srp>
    public void FrameTarget(GameObject targetCameraCenter, Camera targetCamera, GameObject targetObject, float margin)
    {
        if (targetCameraCenter == null || targetObject == null || targetCamera == null) return;

        // 1. Compute the True World-Space Bounding Box
        Bounds combinedBounds = ComputeBounds(targetObject);
        Vector3 targetCenter = combinedBounds.center;

        if (debugObject != null) { debugObject.transform.position = targetCenter; }
        targetCameraCenter.transform.position = targetCenter;

        if (combinedBounds.size == Vector3.zero) return;

        // 2. Calculate the required distance
        float distance = CalculateRequiredDistance(combinedBounds, targetCamera, margin);

        // 3. Position the Camera
        // We position the camera at the center of the bounds, 
        // moved back along the 'lookDirection' by the calculated distance.
        //Vector3 targetCenter = combinedBounds.center;
        //targetCamera.transform.position = targetCenter - (Vector3.forward.normalized * distance);
        
        //cameraObject.transform.localPosition = new Vector3(0.0f, 0.0f, -distance);
        cameraObject.transform.position = targetCenter - (Vector3.forward.normalized * distance * 2.0f);
        // 4. Point the camera at the center
        targetCamera.transform.LookAt(combinedBounds.center);
    }

    private Bounds ComputeBounds(GameObject root)
    {
        // We use Renderer.bounds because it is already in World Space.
        // It accounts for rotation, scale, and mesh transformations.
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0) return new Bounds(root.transform.position, Vector3.zero);

        // Initialize bounds with the first renderer's bounds
        Bounds bounds = renderers[0].bounds;

        // Encapsulate all other renderers
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    private float CalculateRequiredDistance(Bounds bounds, Camera cam, float margin)
    {
        float distance = 0f;

        if (cam.orthographic)
        {
            // For Orthographic, distance doesn't affect size, 
            // only the 'orthographicSize' does. 
            // But if you want to move the camera, we just need a valid distance.
            distance = 10f;
            // Note: To truly frame orthographic, you'd need to adjust cam.orthographicSize
            // based on bounds.size.y / 2.
        }
        else
        {
            // Perspective Math:
            // The formula for the visible height of a frustum at distance D is:
            // Height = 2 * D * tan(FOV / 2)
            // Therefore: D = Height / (2 * tan(FOV / 2))
            cam.aspect = 1.0f;
            float fovRad = cam.fieldOfView * Mathf.Deg2Rad;

            // Calculate distance needed to fit the Height (Vertical)
            float distY = bounds.size.y / (2f * Mathf.Tan(fovRad * 0.5f));

            // Calculate distance needed to fit the Width (Horizontal)
            // We need the Horizontal FOV, which depends on the Aspect Ratio
            float aspect = cam.aspect;
            float distX = bounds.size.x / (2f * Mathf.Tan(fovRad * 0.5f) * aspect);

            // The distance must satisfy BOTH. So we take the maximum of the two.
            distance = Mathf.Max(distX, distY);
        }

        // Add the margin (e.g., 1.1 for 10% extra space)
        return distance * (1f + margin);
    }
}
