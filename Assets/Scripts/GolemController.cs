using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{

    public class GolemController : MonoBehaviour
    {
        [SerializeField]
        private GolemData golemData;
        [SerializeField]
        Vector3 startPosition;
        [SerializeField]
        float deathDistation = 50f;
        [SerializeField]
        Vector2 correctPointForStartPositionX;
        [SerializeField]
        Rigidbody2D rb;
        [SerializeField]
        Transform direction;
        [SerializeField]
        Animator animator;
        [SerializeField]
        Collider2D col;

        [SerializeField]
        Slider UIHPbar;
        float hp;
        float speed;

        public Element GetElement()
        {
            return golemData.GolemType;
        }

        public void EnemySettings(IGolem _enemyData)
        {
           
            hp = golemData.HealthPoint * (1 + PlayerController.kills / 100);
            speed = golemData.MoveSpeed * (1 + PlayerController.kills / 100);

            col.enabled = true;

            UIHPbar.maxValue = hp / 1000;
            UIHPbar.value = UIHPbar.maxValue;
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


            UIHPbar.value = hp / 1000;
        }

        void Move()
        {


            rb.velocity = (new Vector2(direction.position.x, direction.position.y) - rb.position).normalized * speed * 5;
            transform.right = (direction.position - transform.position);
            rb.inertia = 0;



        }

        private void ReturnToPull()
        {
            if (Vector3.Distance(startPosition, transform.position) > deathDistation)
            {
                PlayerController.hp -= golemData.AttackDamage;
                gameObject.SetActive(false);
                startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), startPosition.y, startPosition.z);
                transform.position = startPosition;



                
            }
        }

        

        private void Awake()
        {
            speed = golemData.MoveSpeed;
            EnemySettings(golemData);
            rb = GetComponent<Rigidbody2D>();
            

            startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), startPosition.y, startPosition.z);
            transform.position = startPosition;

        }

        private void Update()
        {
            //UIHPbar.rectTransform.right = new Vector3(hp / fullhp, 0,0);

            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length  + "          " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "           " + animator.GetCurrentAnimatorClipInfo(0).Length);
            if(hp <= 0)
            {
                if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                    animator.SetTrigger("Death");
                if(speed > 0)
                speed -= .5f;
                //transform.position = Vector3.forward / 100;

                col.enabled = false;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).length < (animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 3f)
                && animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                PlayerController.kills++;

                gameObject.SetActive(false);
                startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), startPosition.y, startPosition.z);
                transform.position = startPosition;
            }
            ReturnToPull();

            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length + "          " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
           


        }
    }
}
