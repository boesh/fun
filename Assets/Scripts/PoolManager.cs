using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private List<GameObject> pool;

    public PoolManager()
    {
        this.pool = new List<GameObject>();
    }

    public void AddObjectToPool(GameObject prefab, Transform parent)
    {
        pool.Add(Instantiate(prefab, parent));
    }

    public GameObject GetObjectFromPool(GameObject prefab, Transform parent)
    {
        
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        AddObjectToPool(prefab, parent);
        return pool[pool.Count - 1];
    }
}

