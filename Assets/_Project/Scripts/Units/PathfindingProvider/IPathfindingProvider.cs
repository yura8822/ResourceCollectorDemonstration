using UnityEngine;

public interface IPathfindingProvider
{
    public void Initialize(float speed, float rotationSpeed, float acceleration);
    public void MoveTo(Vector3 destination);
    public void Stop();
    public Vector3[] GetPathCorners();
}
