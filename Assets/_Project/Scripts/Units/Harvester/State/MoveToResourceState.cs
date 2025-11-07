using UnityEngine;

public class MoveToResourceState : BaseState
{
    private HarvesterController _harvesterController;
    private IPathfindingProvider _pathfindingProvider;

    private Vector3 _cachedTargetPosition;
    private float _validationTimer;
    private const float VALIDATION_INTERVAL = 0.25f;
    private const float ARRIVAL_DISTANCE = 2.5f;

    public MoveToResourceState(StateMachine stateMachine, HarvesterController harvesterController) : base(stateMachine)
    {
        _harvesterController = harvesterController;
    }

    public override void OnEnter()
    {
        _pathfindingProvider = _harvesterController.PathFindingProvider;
        _cachedTargetPosition = _harvesterController.LootTarget.transform.position;
        _pathfindingProvider.MoveTo(_cachedTargetPosition);
        _validationTimer = 0f;
    }
    
    public override void OnExit()
    {
        _validationTimer = 0;
        _pathfindingProvider.Stop();
        
        _harvesterController.PathVisualizer.ClearPath();
    }

    public override void OnUpdate()
    {
        _harvesterController.PathVisualizer.UpdatePath();
        
        _validationTimer -= Time.deltaTime;

        if (_validationTimer <= 0f)
        {
            /*
                Check if target position has changed. This happens when another harvester
                collected the resource, returned it to the pool via SetActive(false), and then
                the spawn system reactivated it at a new position via SetActive(true).
                As a result, the current harvester moves to the old position where the resource no longer exists.
             */
            if (IsTargetInvalid())
            {
                ChangeState<SearchResourceState>();
                return;
            }

            if (HasArrivedAtTarget())
            {
                ChangeState<CollectResourceState>();
            }

            _validationTimer = VALIDATION_INTERVAL;
        }
    }



    private bool IsTargetInvalid()
    {
        if (!_harvesterController.LootTarget.gameObject.activeSelf)
            return true;
        
        Vector3 currentPosition = _harvesterController.LootTarget.transform.position;
        return _cachedTargetPosition != currentPosition;
    }

    private bool HasArrivedAtTarget()
    {
        float distanceSqr = (_cachedTargetPosition - _harvesterController.Position).sqrMagnitude;
        return distanceSqr <= ARRIVAL_DISTANCE * ARRIVAL_DISTANCE;
    }
}