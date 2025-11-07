using UnityEngine;

public class ReturnToBaseState : BaseState
{
    private HarvesterController _harvesterController;
    private IPathfindingProvider _pathfindingProvider;

    private float _validationTimer;
    private const float VALIDATION_INTERVAL = 0.25f;
    private const float ARRIVAL_DISTANCE = 3.5f;
    private Vector3 _unloadPos;

    public ReturnToBaseState(StateMachine stateMachine, HarvesterController harvesterController)
        : base(stateMachine)
    {
        _harvesterController = harvesterController;
    }

    public override void OnEnter()
    {
         _unloadPos = _harvesterController.ParentBase.UnloadTransform.position;
        _pathfindingProvider = _harvesterController.PathFindingProvider;
        _pathfindingProvider.MoveTo(_unloadPos);
        _validationTimer = VALIDATION_INTERVAL;
    }

    public override void OnExit()
    {
        _harvesterController.PathVisualizer.UpdatePath();
        
        _validationTimer = VALIDATION_INTERVAL;
        _pathfindingProvider.Stop();
    }

    public override void OnUpdate()
    {
        _harvesterController.PathVisualizer.UpdatePath();
        
        _validationTimer -= Time.deltaTime;

        if (_validationTimer <= 0f)
        {
            TryTransitionToSearchIfArrived();
            _validationTimer = VALIDATION_INTERVAL;
        }
    }

    private void TryTransitionToSearchIfArrived()
    {
        float distanceSqr = ((_unloadPos - _harvesterController.Position).sqrMagnitude);

        if (distanceSqr <= ARRIVAL_DISTANCE * ARRIVAL_DISTANCE)
        {
            ChangeState<UnloadResourceState>();
        }
    }
}