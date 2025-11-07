using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualizer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private IPathfindingProvider _pathfindingProvider;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(IPathfindingProvider pathfindingProvider)
    {
        _pathfindingProvider = pathfindingProvider;
    }

    public void UpdatePath()
    {
        Vector3[] pathCorners = _pathfindingProvider.GetPathCorners();
        
        if (pathCorners == null || pathCorners.Length == 0)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        _lineRenderer.positionCount = pathCorners.Length;
        _lineRenderer.SetPositions(pathCorners);
    }

    public void ClearPath()
    {
        _lineRenderer.positionCount = 0;
    }
}
