using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshPathfindingProvider : MonoBehaviour, IPathfindingProvider
{
   private NavMeshAgent _agent;

   public void Initialize(float speed, float rotationSpeed, float acceleration)
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        _agent.angularSpeed = rotationSpeed;
        _agent.acceleration = acceleration;
    }
    
    public void MoveTo(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
    
    public void Stop()
    {
        _agent.ResetPath();
    }

    public Vector3[] GetPathCorners()
    {
        if (_agent.hasPath)
        {
            return _agent.path.corners;
        }
        return null;
    }
}
