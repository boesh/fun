using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{

    public class GolemController : MonoBehaviour
    {
        [SerializeField]
        private GolemData golemData;
        [SerializeField]
        Vector3 startPosition;
        float deathDistation = 40f;
        [SerializeField]
        Vector2 correctPointForStartPositionX;
        [SerializeField]
        Rigidbody rb;
        [SerializeField]
        Transform direction;
        float hp;


        public void EnemySettings(IGolem _enemyData)
        {
            golemData = (GolemData)_enemyData;
            GetComponent<Renderer>().material = golemData.GolemMaterial;
            GetComponent<MeshFilter>().mesh = golemData.GolemMesh;
            hp = golemData.HealthPoint;
        }


        public void TakeDamage(ISpell _spellData)
        {
            float damageMultipler = 1;

            if(golemData.GolemType == _spellData.SpellType)
            {
                damageMultipler = 1;
            }
            else if((golemData.GolemType == Element.EARTH && _spellData.SpellType == Element.WATER) ||
                (golemData.GolemType == Element.WATER && _spellData.SpellType == Element.FIRE) ||
                (golemData.GolemType == Element.FIRE && _spellData.SpellType == Element.EARTH))
            {
                damageMultipler = 0.5f;
            }
            else
            {
                damageMultipler = 2;
            }
            

            hp -= _spellData.Damage * damageMultipler;
        }

        void Move()
        {
            transform.Translate(Vector3.forward * golemData.MoveSpeed / 10);
            transform.LookAt(direction);
            rb.velocity = Vector3.zero;

            //Vector3 v = new Vector3(direction.position.x, transform.position.y, direction.position.z) - transform.position;
            //rb.MovePosition(v.normalized);
            //rb.AddForce(-Vector3.forward * golemData.MoveSpeed * 2, ForceMode.Impulse);
            //rb.AddForce(v.normalized);
            //rb.MovePosition(-Vector3.forward * golemData.MoveSpeed);
        }

        private void ReturnToPull()
        {
            if (Vector3.Distance(startPosition, transform.position) > deathDistation)
            {
                PlayerController.hp -= golemData.AttackDamage;
                gameObject.SetActive(false);
                startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), transform.position.y, 20f);
                transform.position = startPosition;
                //Debug.Log(rb.velocity);
            }
        }

        

        private void Awake()
        {
            GetComponent<Rigidbody>().mass = golemData.Mass;
            GetComponent<Renderer>().material = golemData.GolemMaterial;
            GetComponent<MeshFilter>().mesh = golemData.GolemMesh;

            startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), transform.position.y, 20f);
            transform.position = startPosition;
        }

        private void Update()
        {
            if(hp < 0)
            {
                PlayerController.kills++;
                gameObject.SetActive(false);
                startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), transform.position.y, 20f);
                transform.position = startPosition;
            }
            ReturnToPull();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
        }
    }
}
