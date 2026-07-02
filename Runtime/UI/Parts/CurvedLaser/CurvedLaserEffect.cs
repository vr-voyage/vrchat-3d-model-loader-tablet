
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;
using VRC.SDK3.ClientSim;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public static class DataListHelper
{
    public static void Add(this DataList list, Vector3 value)
    {
        list.Add(new DataToken(value));
    }
}

[RequireComponent(typeof(LineRenderer))]

public class CurvedLaserEffect : UdonSharpBehaviour
{
    [Header("Settings")]
    public float launchForce = 20f;
    public int resolution = 30; // How many segments the line has
    public float maxTime = 5f;  // How long the laser can travel
    public LayerMask groundLayer;

    [Header("Visuals")]
    public float lineWidth = 0.1f;
    public Gradient colorGradient;

    public LineRenderer lineRenderer;
    private DataList points = new DataList(); // We'll use a simpler list for the example
    private DataList trajectoryPoints = new DataList();

    Vector3 hitLocation;

    public Transform hitLocationPoint;
    public UdonSharpBehaviour hitLocationReceiver;

    void Update()
    {
        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        trajectoryPoints.Clear();

        Vector3 startPosition = transform.position;
        // Velocity is the forward direction * our force
        Vector3 velocity = transform.forward * launchForce;
        Vector3 gravity = Physics.gravity;

        trajectoryPoints.Add(startPosition);

        //bool hitGround = false;
        float timeStep = maxTime / resolution;

        for (int i = 1; i <= resolution; i++)
        {
            float t = i * timeStep;

            // The Physics Formula: P = P0 + Vt + 0.5gt^2
            Vector3 nextPoint = startPosition + (velocity * t) + (0.5f * gravity * t * t);

            // Check if the segment between the last point and this point hits the ground
            RaycastHit hit;
            int lastPoint = trajectoryPoints.Count - 1;
            Vector3 lastVector3 = (Vector3)(trajectoryPoints[lastPoint].Reference);
            if (Physics.Linecast(lastVector3, nextPoint, out hit, groundLayer))
            {
                trajectoryPoints.Add(hit.point);
                //hitGround = true;
                break;
            }

            trajectoryPoints.Add(nextPoint);
        }

        // Update the LineRenderer
        int nPoints = trajectoryPoints.Count;
        lineRenderer.positionCount = nPoints;
        Vector3[] points = new Vector3[nPoints];
        for (int p = 0; p < nPoints; p++)
        {
            Vector3 point = (Vector3)(trajectoryPoints[p].Reference);
            points[p] = point;
        }
        lineRenderer.SetPositions(points);

        hitLocation = (Vector3)trajectoryPoints[points.Length - 1].Reference;
        // If we hit the ground, we might want to stop the line or change color
        // (Optional: you could add logic here to make the end point glow brighter)
    }

    void NotifyHit()
    {
        if (hitLocationPoint != null) hitLocationPoint.position = hitLocation;
        if (hitLocationReceiver != null)
        {
            hitLocationReceiver.SetProgramVariable("hitLocation", hitLocation);
            hitLocationReceiver.SendCustomEvent("HandleHitLocation");
        }
    }

    public override void InputUse(bool value, UdonInputEventArgs args)
    {
        if (!gameObject.activeSelf) return;

        if (value && args.handType == HandType.RIGHT)
        {
            NotifyHit();
            gameObject.SetActive(false);
        }
    }

}
