using UnityEngine;
using UnityEngine.Pool;

public class PoolableComponent : MonoBehaviour
{
    private IObjectPool<PoolableComponent> _objectPool;

    public IObjectPool<PoolableComponent> ObjectPool 
    { 
        get => _objectPool;
        set => _objectPool = value; 
    }

    public void ReturnToPool()
    {
        _objectPool?.Release(this);
    }


    public virtual void OnPoolReturn()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnPoolGet()
    {
        gameObject.SetActive(true);
    }
}