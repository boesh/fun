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
        float spawnTime = 0.3f;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnTime);
                pool.GetObjectFromPool(enemy.gameObject, transform).GetComponent<GolemController>().EnemySettings(enemyData[Random.Range(0, enemyData.Count)]);
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
