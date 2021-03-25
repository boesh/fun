using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    

    public class PlayerController : MonoBehaviour
    {
        
        [SerializeField]
        private Camera cam;
        [SerializeField]
        Transform startSpellPosition;

        public static int kills = 0;
        public static int hp = 100;
        
        Vector3 v;

        [SerializeField]
        Animator animator;
        [SerializeField]
        PoolManager poolManager;

        [SerializeField]
        SpellController spell;


        [SerializeField]
        List<SpellData> spellsSettings;

        int currentSpellIndex;

       
        
        void SpawnSpell()
        {
            SpellController s = poolManager.GetObjectFromPool(spell.gameObject, spell.transform.parent).GetComponent<SpellController>();
            s.Init(new Vector3(v.x, 0, v.y).normalized, startSpellPosition.position, spellsSettings[currentSpellIndex]);
        }

        // Start is called before the first frame update
        void Start()
        {

            

            
            
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
                v = cam.WorldToScreenPoint(startSpellPosition.position) - new Vector3(mousePos.x, mousePos.y, 24.2f);
                //Debug.Log(v);
                

            }


            GUILayout.BeginArea(new Rect(20, 20, 250, 120));
            GUILayout.Label("hp: " + hp);

            GUILayout.Label("Kills: " + kills);

            GUILayout.EndArea();
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
                if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    animator.SetBool("Attack", true);
               

            }
            if (Input.GetButtonDown("1"))
            {
                currentSpellIndex = 0;
            }
            if (Input.GetButtonDown("2"))
            {
                currentSpellIndex = 1;
            }
            if (Input.GetButtonDown("3"))
            {
                currentSpellIndex = 2;
            }
            if (Input.GetButtonDown("4"))
            {
                hp = 10000000;
                Time.timeScale = 1f;
            }
            if (hp <= 0)
            {
                Debug.Log("gameOver");
                Time.timeScale = 0f;
            }
            //Debug.Log(Application.isPlaying);

        }

        private void FixedUpdate()
        {
           
        }
    }
}
