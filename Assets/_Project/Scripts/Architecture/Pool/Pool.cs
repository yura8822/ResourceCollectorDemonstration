using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolableComponent _lootPrefab;
    [SerializeField] private bool _collectionCheck = true;

    private ObjectPool<PoolableComponent> _objectPool;
    private List<PoolableComponent> _activeElements = new();


    public void CreatePool(int defaultCapacity, int maxSize)
    {
        if (_objectPool != null) return;

        _objectPool = new ObjectPool<PoolableComponent>(
            createFunc: CreatePoolable,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPooledObject,
            collectionCheck: _collectionCheck,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public PoolableComponent Get()
    {
        return _objectPool.Get();
    }


    public void Return(PoolableComponent poolable)
    {
        _objectPool.Release(poolable);
    }

    public int GetPoolActiveCount()
    {
        return _activeElements.Count;
    }

    public List<PoolableComponent> GetActiveElements()
    {
        return _activeElements;
    }


    private PoolableComponent CreatePoolable()
    {
        PoolableComponent instance = Instantiate(_lootPrefab, transform);
        instance.ObjectPool = _objectPool;
        return instance;
    }


    private void OnGetFromPool(PoolableComponent poolable)
    {
        _activeElements.Add(poolable);
        poolable.OnPoolGet();
    }


    private void OnReleaseToPool(PoolableComponent poolable)
    {
        _activeElements.Remove(poolable);
        poolable.OnPoolReturn();
    }


    private void OnDestroyPooledObject(PoolableComponent poolable)
    {
        _activeElements.Remove(poolable);
        Destroy(poolable.gameObject);
    }

    private void OnDestroy()
    {
        _activeElements.Clear();
        _objectPool?.Dispose();
    }
}
