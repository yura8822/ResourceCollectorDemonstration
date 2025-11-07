using UnityEngine;

public class CollectResourceState : BaseState
{
    private HarvesterController _harvesterController;
    private float _collectionTimer;

    public CollectResourceState(StateMachine stateMachine, HarvesterController harvesterController) : base(stateMachine)
    {
        _harvesterController = harvesterController;
    }

    public override void OnEnter()
    {
        HarvestLoot();
        _collectionTimer = 0;
    }

    public override void OnExit()
    {
        _harvesterController.HasResource = true;
        _collectionTimer = 0;
    }

    public override void OnUpdate()
    {
        _collectionTimer += Time.deltaTime;

        if (_collectionTimer >= _harvesterController.CollectionDuration)
        {
            ChangeState<ReturnToBaseState>();
        }
    }

    private void HarvestLoot()
    {
        _harvesterController.LootTarget.ReturnToPool();
        _harvesterController.LootTarget = null;
    }
}