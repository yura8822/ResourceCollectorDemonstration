using UnityEngine;

public class UnloadResourceState : BaseState
{
    private HarvesterController _harvesterController;
    private float _unloadTimer;

    public UnloadResourceState(StateMachine stateMachine, HarvesterController harvesterController)
        : base(stateMachine)
    {
        _harvesterController = harvesterController;
    }

    public override void OnEnter()
    {
        _unloadTimer = 0f;
    }

    public override void OnExit()
    {
        UnloadResource();
        _unloadTimer = 0f;
    }

    public override void OnUpdate()
    {
        _unloadTimer += Time.deltaTime;

        if (_unloadTimer >= _harvesterController.UnloadDuration)
        {
            ChangeState<SearchResourceState>();
        }
    }

    private void UnloadResource()
    {
        _harvesterController.ParentBase.AddLoot();
        _harvesterController.HasResource = false;
    }
}