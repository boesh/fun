using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpellController : MonoBehaviour 
    {
        [SerializeField]
        SpellData spellData;
        Vector3 direction;


        Vector3 startPosition;
        [SerializeField]
        float deathDistation = 80f;


        MeshFilter meshFilter;
        Renderer rend;



        [SerializeField]
        public ParticleSystem particleSys;

        

        public Element GetElement()
        {
            return spellData.SpellType;
        }

        public void SpellSettings(ISpell _spellData)
        {
            spellData = (SpellData)_spellData;
            
            
            
            meshFilter.mesh = spellData.SpellMesh;
            rend.material = spellData.SpellMaterial;

            
            //particleSys = spellData.Particle;
            //var sss = particleSys.main;
            //sss.startLifetime = spellData.Particle.main.startLifetime;

            //particleSys = GetComponent<ParticleSystem>();
            //sss.transform.position = transform.position;
            //sss.transform.parent = transform;

            //particleSys.transform.position = transform.position;
            //particleSys.transform.parent = transform;

            Debug.Log(particleSys.name);
            Debug.Log(particleSys.particleCount);

        }

      

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<GolemController>().TakeDamage(spellData);

            //Destroy(particleSys);
            gameObject.SetActive(false);
        }

        

        public void Init(Vector3 _direction, Vector3 startPosition, ISpell _spellData)
        {
            direction = _direction;
            transform.position = startPosition;
            this.startPosition = startPosition;
            //SpellSettings(_spellData);

        }

        private void ReturnToPull()
        {
            if (Vector3.Distance(startPosition, transform.position) > deathDistation)
            {
                //Destroy(particleSys);

                gameObject.SetActive(false);

            }
        }
        private void Awake()
        {
            //meshFilter = GetComponent<MeshFilter>();
            //meshFilter.mesh = spellData.SpellMesh;

            //rend = GetComponent<Renderer>();
            //rend.material = spellData.SpellMaterial;


            //particleSys = GetComponent<ParticleSystem>();
            

            //particleSys = Instantiate(spellData.Particle);

            //particleSys.transform.position = transform.position;
            //particleSys.transform.parent = transform;
        }
       

        private void Start()
        {
            //particleSys = spellData.Particle;
            //particleSys.Play();

        }
        void Update()
        {
           
        }

        private void FixedUpdate()
        {
            
            

            ReturnToPull();
            transform.right = (direction - transform.position);
            //transform.LookAt(direction);
            this.transform.position -= direction * spellData.Speed / 10;
        }

    }
}
