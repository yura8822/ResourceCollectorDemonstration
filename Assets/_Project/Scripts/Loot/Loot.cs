using System;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(PoolableComponent))]
public class Loot : MonoBehaviour
{
    private PoolableComponent _poolableComponent;

    private void Start()
    {
        _poolableComponent = GetComponent<PoolableComponent>();
    }

    public void ReturnToPool()
    {
        _poolableComponent.ReturnToPool();
    }
    
    
    
}
