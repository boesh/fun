using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace Assets.Scripts
{
    

    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        List<Text> info;
        [SerializeField]
        private Camera cam;
        [SerializeField]
        Transform startSpellPosition;
        [SerializeField]
        Transform uiSpells;
        Quaternion uiSpellsDir;
        static int kills = 0;
        public static int hp = 100;

        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        List<AudioClip> audioClips;

        
        
        Vector3 v;

        [SerializeField]
        Animator animator;
        [SerializeField]
        PoolManager poolManager;

        [SerializeField]
        SpellController spell;


        [SerializeField]
        List<SpellData> spellsSettings;

        [SerializeField]
        List<SpellController> prefabs;


        int currentSpellIndex;

        Vector2 startPos;
        Vector2 dir;


        
        public static void IncrementKills()
        {
            kills++;
        }
        public static int GetKillsCount()
        {
            return kills;
        }
        

        public void Restart()
        {
            SceneManager.LoadScene(0);
            kills = 0;
            hp = 100;
            Time.timeScale = 1f;
        }

        public void Exit()
        {
            Application.Quit();
        }

        void SpawnSpell()
        {
            Debug.Log(currentSpellIndex);
            poolManager.GetObjectFromPool(prefabs[currentSpellIndex], spell.transform.parent)
                .GetComponent<SpellController>().Init(new Vector3(v.x, -v.y, 0).normalized, startSpellPosition.position, spellsSettings[currentSpellIndex]);
            Debug.Log(currentSpellIndex);
        }

        private void Awake()
        {
            uiSpellsDir = uiSpells.rotation;
        }

        void OnGUI()
        {
            Vector3 point = new Vector3();
            Event currentEvent = Event.current;
            Vector2 mousePos = new Vector2();

           
            mousePos.x = currentEvent.mousePosition.x;
            mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;


            point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            if (Input.GetMouseButtonDown(0))
            {
                v = cam.WorldToScreenPoint(startSpellPosition.position) - new Vector3(mousePos.x, mousePos.y, cam.transform.position.z);
                //Debug.Log(cam.);


            }

            info[0].text = "HP: " + (hp < 0 ? "0" : hp.ToString()) ;
            info[1].text = "Kills: " + kills.ToString();

           
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("state length    " + animator.GetCurrentAnimatorStateInfo(0).length +
            //    "            clip length            " + animator.GetCurrentAnimatorClipInfo(0).Length + "         " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (animator.GetCurrentAnimatorStateInfo(0).length < (animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 1.5)
                && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetBool("Attack"))
            {
                
                    animator.SetBool("Attack", false);

                    SpawnSpell();
            }

            if (Input.GetMouseButtonDown(0))
            {

                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) //  spell cast for PC tests TODO delete for bild
                    animator.SetBool("Attack", true);

            }
            if (Input.GetButtonDown("1"))
            {
                Time.timeScale = 1f;
                hp = 100000000;
                
            }

            if (kills % 10 == 0 && kills != 0)
            {

                audioSource.clip = audioClips[0];
                audioSource.Play();
            }

            if (kills % 15 == 0 && kills != 0)
            {
                audioSource.clip = audioClips[1];
                audioSource.Play();
            }
            if (Input.GetButtonDown("2"))
            {
                
                if (currentSpellIndex < spellsSettings.Count - 1)
                {
                    currentSpellIndex++;


                }
                else
                {
                    currentSpellIndex = 0;


                }
                uiSpellsDir *= Quaternion.Euler(Vector3.forward * 120f);

            }
            if (Input.GetButtonDown("3"))
            {
                if (currentSpellIndex > 0)
                {
                    currentSpellIndex--;

                }
                else
                {

                    currentSpellIndex = spellsSettings.Count - 1;
                }
                uiSpellsDir *= Quaternion.Euler(-Vector3.forward * 120f);
            }
            if (Input.GetButtonDown("4"))
            {
                Restart();
            }
            if (hp <= 0)
            {
                Time.timeScale = 0f;
                
                info[2].text = "GAME OVER";
                //Debug.Log("gameOver");
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on touch phase.
                switch (touch.phase)
                {
                    // Record initial touch position.
                    case TouchPhase.Began:
                        startPos = touch.position;
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        dir = touch.position - startPos;

                        break;

                    //Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        dir = touch.position - startPos; //for next test
                        if (dir.y < 250 && dir.y > -250) // spell swipe
                        {
                            if (dir.x < -150)
                            {
                                if (currentSpellIndex < spellsSettings.Count - 1)
                                {
                                    currentSpellIndex++;
                                }
                                else
                                {
                                    currentSpellIndex = 0;
                                }
                                uiSpellsDir *= Quaternion.Euler(Vector3.forward * 120f);
                            } 
                            else if (dir.x > 150)
                            {
                                if (currentSpellIndex > 0)
                                {
                                    currentSpellIndex--;
                                }
                                else
                                {
                                    currentSpellIndex = spellsSettings.Count - 1;
                                }
                                uiSpellsDir *= Quaternion.Euler(-Vector3.forward * 120f);
                            }
                            else
                            {
                                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) //  spell cast
                                    animator.SetBool("Attack", true);
                            }
                        }
                        else
                        {
                            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) //  spell cast
                                animator.SetBool("Attack", true);
                        }

                        break;
                }
            }
            uiSpells.rotation = Quaternion.Lerp(uiSpells.rotation, uiSpellsDir, Time.deltaTime * 5);
            //Debug.Log(Application.isPlaying);
            
        }

        private void FixedUpdate()
        {
        }
    }
}
