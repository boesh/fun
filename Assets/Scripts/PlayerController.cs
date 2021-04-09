using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Assets.Scripts
{
    [Serializable]
    public class SaveData
    {
        public int bestScore;
        public int gold;


        public float EarthDamageMultiplier; 
        public float FireDamageMultiplier;
        public float WaterDamageMultiplier; 
        public int maxHpMultiplier;
        public float castTimeMultiplier;
    }

    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        List<Text> info;
        [SerializeField]
        List<Button> shopLots;

        [SerializeField]
        private Camera cam;
        [SerializeField]
        Transform startSpellPosition;
       
        static int kills = 0;
        public static int hp = 100;




        int bestScore;
        public static int gold;
        float [] damageMultipliers;
        public int maxHpMultiplier;
        public float castTimeMultiplier;
        [SerializeField]
        List<Text> prices;


        bool pointerOnPause;
        bool gameIsPaused = false;
       


        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        List<AudioClip> audioClips;
        
        
      
        
        Vector3 v;

        Vector2 startPos;
        Vector2 dir;

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

        [SerializeField]
        GameObject pauseMenu;
        [SerializeField]
        GameObject resumeButton;
        [SerializeField]
        GameObject pauseButton;
        [SerializeField]
        GameObject instructionPanel;
        [SerializeField]
        Animator instructionAnimator;
        int instructionsCount = 2;
        [SerializeField]
        Text instructionText;
        [SerializeField]
        List<string> instructionTexts;
        


        [SerializeField]
        Transform uiSpells;
        Quaternion uiSpellsDir;

       
        void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath
              + "/MySaveData.dat");
            SaveData data = new SaveData();
            data.bestScore = bestScore;
            data.gold = gold;
            data.EarthDamageMultiplier = damageMultipliers[(int)Element.EARTH];
            data.FireDamageMultiplier = damageMultipliers[(int)Element.FIRE];
            data.WaterDamageMultiplier = damageMultipliers[(int)Element.WATER];
            data.maxHpMultiplier = maxHpMultiplier;
            data.castTimeMultiplier = castTimeMultiplier;


            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }

        void LoadGame()
        {
            if (File.Exists(Application.persistentDataPath
              + "/MySaveData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file =
                  File.Open(Application.persistentDataPath
                  + "/MySaveData.dat", FileMode.Open);
                SaveData data = (SaveData)bf.Deserialize(file);
                file.Close();
                bestScore = data.bestScore;
                gold = data.gold;
                damageMultipliers[(int)Element.EARTH] = data.EarthDamageMultiplier;
                damageMultipliers[(int)Element.FIRE] = data.FireDamageMultiplier;
                damageMultipliers[(int)Element.WATER] = data.WaterDamageMultiplier;
                maxHpMultiplier = data.maxHpMultiplier;
                castTimeMultiplier = data.castTimeMultiplier;

                Debug.Log("Game data loaded!");
            }
            else
                Debug.LogError("There is no save data!");
        }

        void ResetData()
        {
            if (File.Exists(Application.persistentDataPath
              + "/MySaveData.dat"))
            {
                File.Delete(Application.persistentDataPath
                  + "/MySaveData.dat");
                bestScore = 0;
                gold = 0;
                damageMultipliers[(int)Element.EARTH] = 1;
                damageMultipliers[(int)Element.FIRE] = 1;
                damageMultipliers[(int)Element.WATER] = 1;
                maxHpMultiplier = 1;
                castTimeMultiplier = 1;
                Debug.Log("Data reset complete!");
            }
            else
                Debug.LogError("No save data to delete.");
        }
       

      
        public static void IncrementKills()
        {
            kills++;
        }
        public static int GetKillsCount()
        {
            return kills;
        }


        public void PointerEnterOnPause()
        {
            pointerOnPause = true;
        }

        public void Instruction()
        {
            pauseMenu.SetActive(false);
            instructionPanel.SetActive(true);
        }

        public void ChangeInstructionClip()
        {
            if (instructionAnimator.GetInteger("state") == instructionsCount - 1)
            {
                instructionPanel.SetActive(false);
                instructionAnimator.SetInteger("state", 0);
                Pause();
            }
            else 
            {
                instructionAnimator.SetInteger("state", instructionAnimator.GetInteger("state") + 1);
                

            }
            instructionText.text = instructionTexts[instructionAnimator.GetInteger("state")];

        }



        public void PointerExitOnPause()
        {
            pointerOnPause = false;
        }


        public void Resume()
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            pauseButton.SetActive(true);
            gameIsPaused = false;

        }
        public void Test(GameObject UIgm)
        {
            UIgm.SetActive(true);
            pauseMenu.SetActive(false);
            Debug.Log("Work");
        }

        
        
        public float PriceConverter(float multiplier, float startPrice, float spellConstMult, float priceMult)
        {
            float price = startPrice;
            for (float i = multiplier; i != 1; i /= spellConstMult)
            {
                price *= priceMult;
            }

            return price;
        }

        public void UpgradeShopSpell(int _spellIndex)
        {
            float _price = PriceConverter(damageMultipliers[_spellIndex], 100f, 1.05f, 1.2f);
            if(_price < gold)
            {
                gold -= (int)_price;
                damageMultipliers[_spellIndex] *= 1.05f;
                SaveGame();
            }
            prices[_spellIndex].text = _price.ToString();

        }

        public void UpgradeShopMaxHP()
        {
            float price = PriceConverter(maxHpMultiplier, 100f, 2f, 1.2f);
            
            if(price < gold)
            {
                
                    gold -= (int)price;
                    maxHpMultiplier *= 2;
                    hp = (int)(hp * maxHpMultiplier);
                    SaveGame();
            }
            prices[3].text = price.ToString();

        }

        public void UpgradeShopCastSpeed ()
        {
            float price = PriceConverter(castTimeMultiplier, 100f, 1.2f, 1.2f);
            
            if (price < gold)
            {

                gold -= (int)price;
                castTimeMultiplier *= 1.2f;
                animator.speed *= castTimeMultiplier;

                SaveGame();
            }
            prices[4].text = price.ToString();
        }

























        public void BackToPauseMenu(GameObject toDisactivated)
        {
            pauseMenu.SetActive(true);
            toDisactivated.SetActive(false);
        }

       
        public void Pause()
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            pauseButton.SetActive(false);
            PointerExitOnPause();
            gameIsPaused = true;
        }






        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            

            kills = 0;
            hp = 100;
            SaveGame();
            Time.timeScale = 1f;
        }

        public void GameOver()
        {
            Time.timeScale = 0f;

            info[2].text = "GAME OVER";
            Pause();
            resumeButton.SetActive(false);
            

        }

        public void Exit()
        {
            Application.Quit();
        }

        void SpawnSpell()
        {
            //Debug.Log(currentSpellIndex);

                //animator.SetBool("Attack", false);

            poolManager.GetObjectFromPool(prefabs[currentSpellIndex], spell.transform.parent)
                    .GetComponent<SpellController>().Init(new Vector3(v.x, -v.y, cam.nearClipPlane).normalized, startSpellPosition.position, damageMultipliers[currentSpellIndex]);


        }


        private void Awake()
        {
            uiSpellsDir = uiSpells.rotation;

            damageMultipliers = new float[spellsSettings.Count];
            for (int i = 0; i < spellsSettings.Count; ++i)
            {
                damageMultipliers[i] = 1;
            }

            ResetData();

            //SaveGame();

            LoadGame();

            
            for(int i = 0; i < 3; ++i)
            {
                prices[i].text = (PriceConverter(damageMultipliers[i], 100f, 1.05f, 1.2f) / 1.2f).ToString();
            }
            prices[3].text = (PriceConverter(maxHpMultiplier, 100f, 1.1f, 1.2f) / 1.2f ).ToString();
            prices[4].text = (PriceConverter(castTimeMultiplier, 100f, 1.5f, 1.2f) / 1.2f).ToString();




            animator.speed *= castTimeMultiplier;
            hp *= maxHpMultiplier;
            

        }

        private void OnApplicationQuit()
        {
            SaveGame();
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


                



                //transform.rotation = Quaternion.Euler(ang + 90f, 90f, 90f); //WRONG ROTATION
                //float ang = Vector2.SignedAngle(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(mousePos.x, mousePos.y));

                //Debug.Log(ang);

            }
            info[0].text = "HP: " + (hp < 0 ? "0" : hp.ToString()) ;
            info[1].text = "Kills: " + kills.ToString();
            info[3].text = "Best Score: " + bestScore.ToString();
            info[4].text = "Gold: " + gold.ToString();
           
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
                if (!pointerOnPause && !gameIsPaused)
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) //  spell cast for PC tests TODO delete for bild
                        animator.SetBool("Attack", true);



            }





            
            if (Input.GetButtonDown("1"))
            {
                Time.timeScale = 1f;
                //hp = 10000000;
                gold = 11111111;
                //ResetData();
                //Debug.Log(EarthDamageMultiplier + "   " + WaterDamageMultiplier + "   " + FireDamageMultiplier + "   " + maxHpMultiplier + "      " + castTimeMultiplier);
                Debug.Log(currentSpellIndex);


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
                GameOver();
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

                                if (!pointerOnPause && !gameIsPaused)
                                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) //  spell cast
                                    animator.SetBool("Attack", true);
                            }
                        }
                        else
                        {

                            if (!pointerOnPause && !gameIsPaused)
                                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) //  spell cast
                                animator.SetBool("Attack", true);
                        }

                        break;
                }
            }

            uiSpells.rotation = Quaternion.Lerp(uiSpells.rotation, uiSpellsDir, Time.deltaTime * 5);
            //Debug.Log(Application.isPlaying);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.position.x, v.x, transform.position.z), 2f);
            //transform.right = Vector3.Lerp(transform.right, transform.position - new Vector3(vv.x, -vv.y, 0f), .0001f);

            //transform.Rotate(transform.position - new Vector3(vv.x, -vv.y, 0f));

            //transform.rotation = Quaternion.Euler( new Vector3(v.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            
            if(kills > bestScore)
            {
                bestScore = kills;
            }
            

        }



        private void FixedUpdate()
        {

        }
    }
}
