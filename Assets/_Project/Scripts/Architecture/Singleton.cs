using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T s_Instance;

    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindFirstObjectByType<T>();
                
                if (s_Instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    s_Instance = singletonObject.AddComponent<T>();
                    
                    singletonObject.name = typeof(T).ToString();
                }
            }

            return s_Instance;
        }
    }

    protected virtual void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this as T;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (s_Instance == this)
        {
            s_Instance = null;
        }
    }
}