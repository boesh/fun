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
        Transform target;
        [SerializeField]
        Animator animator;
        [SerializeField]
        Collider2D col;

        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        List<AudioClip> deathClips;
        [SerializeField]
        List<AudioClip> spawnClips;
        [SerializeField]
        List<AudioClip> hittedClips;
        [SerializeField]
        List<AudioClip> attackClips;
        [SerializeField]
        float attackRange;



        [SerializeField]
        Slider UIHPbar;
        float hp;
        float speed;

        public Element GetElement()
        {
            return golemData.GolemType;
        }

        void Death()
        {
            audioSource.clip = deathClips[Random.Range(0, spawnClips.Count - 1)];
            audioSource.Play();
            
        }

        public void EnemySettings(IGolem _enemyData)
        {
           
            hp = golemData.HealthPoint * (1 + (float)PlayerController.GetKillsCount() / 100);
            speed = golemData.MoveSpeed * (1 + (float)PlayerController.GetKillsCount() / 100);

            col.enabled = true;

            UIHPbar.maxValue = hp / 1000;
            UIHPbar.value = UIHPbar.maxValue;

            audioSource.clip = spawnClips[Random.Range(0, spawnClips.Count -1)];
            audioSource.Play();
        }


        public void TakeDamage(Element _spellType, float _damage)
        {
            float damageMultiplier = 1;

            if(golemData.GolemType == _spellType)
            {
                damageMultiplier = 1;
            }
            else if((golemData.GolemType == Element.EARTH && _spellType == Element.WATER) ||
                (golemData.GolemType == Element.WATER && _spellType == Element.FIRE) ||
                (golemData.GolemType == Element.FIRE && _spellType == Element.EARTH))
            {
                damageMultiplier = 0.5f;
            }
            else
            {
                damageMultiplier = 2;
            }
            

            hp -= _damage * damageMultiplier;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                audioSource.clip = hittedClips[Random.Range(0, spawnClips.Count - 1)];
                audioSource.Play();
            }
            //Debug.Log(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

            UIHPbar.value = hp / 1000;
        }

        void Move()
        {


            rb.velocity = (new Vector2(target.position.x, target.position.y) - rb.position).normalized * speed * 5;
            transform.right = (target.position - transform.position);

            rb.inertia = 0;



        }



        private void ReturnToPull()
        {
           
        }

        private void AttackPlayer()
        {
            PlayerController.hp -= golemData.AttackDamage;
            audioSource.clip = attackClips[Random.Range(0, attackClips.Count)];
            audioSource.Play();

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
               
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                { 
                    animator.SetTrigger("Death");
                    


                    //audioSource.clip = deathClips[Random.Range(0, spawnClips.Count - 1)];
                    //audioSource.Play();

                }
                
                if (speed > 0)
                {
                    speed -= .5f;
                }
                //transform.position = Vector3.forward / 100;
                col.enabled = false;



                
            }
            if (animator.GetCurrentAnimatorStateInfo(0).length < (animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 3f)
                && animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                PlayerController.IncrementKills();
                PlayerController.gold += 10 + PlayerController.GetKillsCount() / 10;

                gameObject.SetActive(false);
                startPosition = new Vector3(Random.Range(correctPointForStartPositionX.x, correctPointForStartPositionX.y), startPosition.y, startPosition.z);
                transform.position = startPosition;
            }

            if (Vector3.Distance(target.position, transform.position) < attackRange)
            {
                animator.SetBool("Attack", true);

            }
            //Debug.Log(Vector3.Distance(target.position, transform.position));
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length + "          " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
           


        }
    }
}
