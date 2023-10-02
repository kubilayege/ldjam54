using System;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    protected bool initialized;
    private static volatile T instance;

    private void OnValidate()
    {
        gameObject.name = typeof(T).Name;
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (!instance.initialized)
                {
                    instance.Initialize();
                    instance.initialized = true;
                }
            }
            return instance;
        }
    }

    protected virtual void Initialize()
    { }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}