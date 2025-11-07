using UnityEngine;

public class SearchResourceState : BaseState
{
    private HarvesterController _harvesterController;
    private LootSpawner[] _lootSpawners;
    
    private float _searchTimer;
    private const float SEARCH_INTERVAL = 1f;

    public SearchResourceState(StateMachine stateMachine, HarvesterController harvesterController) : base(stateMachine)
    {
        _harvesterController = harvesterController;
    }

    public override void OnEnter()
    {
        _lootSpawners = Object.FindObjectsByType<LootSpawner>(FindObjectsSortMode.None);

        if (_lootSpawners == null || _lootSpawners.Length == 0)
            Debug.LogError("No loot spawners found");

        SortSpawnersByDistance();
    }

    public override void OnExit()
    {
        _searchTimer = 0;
    }

    public override void OnUpdate()
    {
        _searchTimer -= Time.deltaTime;

        if (_searchTimer <= 0f)
        {
            Loot targetLoot = FindClosestLootFromSpawner();

            if (targetLoot != null)
            {
                _harvesterController.LootTarget = targetLoot;
                ChangeState<MoveToResourceState>(); 
            }
            else
            {
                _searchTimer = SEARCH_INTERVAL;
            }
        }
    }


    private void SortSpawnersByDistance()
    {
        System.Array.Sort(_lootSpawners, (spawnerA, spawnerB) =>
        {
            float distanceA = (spawnerA.transform.position - _harvesterController.transform.position).sqrMagnitude;
            float distanceB = (spawnerB.transform.position - _harvesterController.transform.position).sqrMagnitude;

            return distanceA.CompareTo(distanceB);
        });
    }

    private Loot FindClosestLootFromSpawner()
    {
        for (int i = 0; i < _lootSpawners.Length; i++)
        {
            Loot closest = _lootSpawners[i].GetLootClosestTo(_harvesterController.Position);
            if (closest != null) return closest;
        }

        return null;
    }
}
