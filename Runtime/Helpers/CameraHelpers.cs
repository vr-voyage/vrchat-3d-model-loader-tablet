using System.Collections;
using UnityEngine;
using VRC.SDK3.Data;

public static class CameraHelpers
{
    public static bool TryGetBounds(this GameObject obj, out Bounds bounds)
    {
        
        var renderers = obj.GetComponentsInChildren<Renderer>();
        Debug.Log($"<color=cyan>{obj.name} has {renderers.Length} renderers</color>");
        return renderers.TryGetBounds(out bounds);
    }

    public static bool TryGetBounds(this Renderer[] renderers, out Bounds bounds)
    {
        bounds = default;

        if (renderers.Length == 0)
        {
            Debug.Log("No renderers");
            return false;
        }

        bounds = renderers[0].bounds;

        for (var i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return true;
    }

    // Facto how far away the camera should be 
    private const float cameraDistance = 1f;

    public static bool TryGetFocusTransforms(this Camera camera, GameObject targetObject)
    {
        if (camera == null || targetObject == null) { return false; }

        if (!targetObject.TryGetBounds(out var bounds))
        {
            Debug.LogError("Could not get bounds");
            return false;
        }

        var objectSizes = bounds.max - bounds.min;
        var objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        var cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView);
        var distance = cameraDistance * objectSize / cameraView;
        distance += 0.5f * objectSize;
        Vector3 targetPosition = bounds.center - distance * camera.transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(bounds.center - targetPosition);

        camera.transform.position = targetPosition;
        camera.transform.rotation = targetRotation;


        Debug.Log("<color=cyan>Set camera</color>");
        return true;
    }
}

