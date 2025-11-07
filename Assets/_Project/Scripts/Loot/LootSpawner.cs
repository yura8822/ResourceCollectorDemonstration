using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pool))]
public class LootSpawner : MonoBehaviour
{
    [SerializeField] private int _startLootCount = 5;
    [SerializeField] private int _maxLootCount = 20;
    [SerializeField] private float _spawnCooldown = 3f;
    [SerializeField] private float _spawnRadius = 10f;

    private Pool _pool;
    private float _spawnTimer;
    
    
    private void Start()
    {
        _pool = GetComponent<Pool>();
        _pool.CreatePool(_startLootCount, _maxLootCount);
        for (int i = 0; i < _startLootCount; i++)
        {
            SpawnLootInRadius();
        }
    }
    
    private void Update()
    {
        SpawnLoop();
    }
    
    public Loot GetLootClosestTo(Vector3 position)
    {
        if (_pool.GetPoolActiveCount() == 0)
            return null;
    
        PoolableComponent closest = null;
        float closestSqrDistance = float.MaxValue;
    
        foreach (PoolableComponent element in _pool.GetActiveElements())
        {
            float sqrDistance = (element.transform.position - position).sqrMagnitude;
        
            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closest = element;
            }
        }

        return closest.GetComponent<Loot>();
    }

    private void  SpawnLoop()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f && _pool.GetPoolActiveCount() < _maxLootCount)
        {
            SpawnLootInRadius();
            _spawnTimer += _spawnCooldown;
        }
    }
    
    private void SpawnLootInRadius()
    {
        Vector3 randomPosition = GetRandomPositionInRadius();
        PoolableComponent loot = _pool.Get();
        
        if (loot != null)
        {
            loot.transform.position = randomPosition;
            loot.transform.rotation = Quaternion.identity;
        }
    }

    private Vector3 GetRandomPositionInRadius()
    {
        Vector3 randomDirection = Random.insideUnitCircle;
        return transform.position + new Vector3(randomDirection.x, transform.position.y, randomDirection.y) * _spawnRadius;
    }

    private void OnDrawGizmos()
    {
        // Рисуем сферу радиуса спавна
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);

        // Рисуем центр спавна
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .3f);
    }
}
