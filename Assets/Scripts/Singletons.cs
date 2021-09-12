using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = Instantiate(new GameObject()).AddComponent<T>();
                    instance.hideFlags = HideFlags.HideAndDontSave;
                }
            }
            return instance;
        }
    }
}

public class SingletonPersistent<T> : Singleton<T> where T : Component
{
    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this);
        }
    }
}
