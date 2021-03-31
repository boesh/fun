using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
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

        public GameObject GetObjectFromPool(GolemController prefab, Transform parent)
        {

            for (int i = 0; i < pool.Count; i++)
            {

                if (!pool[i].activeSelf && pool[i].GetComponent<GolemController>().GetElement() == prefab.GetElement())
                {
                    pool[i].SetActive(true);
                    return pool[i];
                }
            }

            AddObjectToPool(prefab.gameObject, parent);
            return pool[pool.Count - 1];
        }

        public GameObject GetObjectFromPool(SpellController prefab, Transform parent)
        {

            for (int i = 0; i < pool.Count; i++)
            {

                if (!pool[i].activeSelf && pool[i].GetComponent<SpellController>().GetElement() == prefab.GetElement())
                {
                    Debug.Log(pool[i].GetComponent<SpellController>().GetElement() + "      " + prefab.GetElement());

                    pool[i].SetActive(true);
                    return pool[i];
                }
            }

            AddObjectToPool(prefab.gameObject, parent);
            return pool[pool.Count - 1];
        }
    }

}