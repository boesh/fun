using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        GolemController enemy;
        [SerializeField]
        PoolManager pool;
        [SerializeField]
        List<GolemData> enemyData;
        [SerializeField]
        List<GameObject> enemyPrefabs;

        [SerializeField]
        float spawnTime = 1f;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnTime);
                int s = Random.Range(0, enemyData.Count);
                //pool.GetObjectFromPool(enemy.gameObject, transform).GetComponent<GolemController>().EnemySettings(enemyData[Random.Range(0, enemyData.Count)]);
                pool.GetObjectFromPool(enemyPrefabs[s].GetComponent<GolemController>(), transform).GetComponent<GolemController>().EnemySettings(enemyData[s]);
            }
        }

        private void Update()
        {
        }
        private void FixedUpdate()
        {
            if (spawnTime > 0.25f)
            spawnTime -= 0.0001f;
        }
    }
}
