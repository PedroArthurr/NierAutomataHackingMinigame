using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private List<T> pool;
    private Transform parent;

    public ObjectPool(T prefab, int initialCapacity = 0, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        pool = new List<T>();

        for (int i = 0; i < initialCapacity; i++)
        {
            T obj = CreateObject();
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public T GetObjectFromPool()
    {
        T obj;

        if (pool.Count > 0)
        {
            obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = CreateObject();
        }

        return obj;
    }

    public void ReturnObjectToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }

    private T CreateObject()
    {
        T obj = Object.Instantiate(prefab);
        obj.transform.parent = parent;
        return obj;
    }
}
