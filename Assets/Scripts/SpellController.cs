using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class SpellController : MonoBehaviour 
    {
        [SerializeField]
        SpellData spellData;

        Vector3 direction;
        
        Vector3 startPosition;
        [SerializeField]
        float deathDistation = 80f;

        public void SpellSettings(ISpell _spellData)
        {
            spellData = (SpellData)_spellData;
            GetComponent<Renderer>().material = spellData.SpellMaterial;
            GetComponent<MeshFilter>().mesh = spellData.SpellMesh;
        }

        

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<GolemController>().TakeDamage(spellData);

            gameObject.SetActive(false);
        }

        public void Init(Vector3 _direction, Vector3 startPosition, ISpell _spellData)
        {
            direction = _direction;
            transform.position = startPosition;
            this.startPosition = startPosition;
            SpellSettings(_spellData);
        }

        private void ReturnToPull()
        {
            if (Vector3.Distance(startPosition, transform.position) > deathDistation)
            {
                gameObject.SetActive(false);
            }
        }
        private void Awake()
        {
            
            GetComponent<MeshFilter>().mesh = spellData.SpellMesh;

            GetComponent<Renderer>().material = spellData.SpellMaterial;
            
        }
     
     

        private void FixedUpdate()
        {
            ReturnToPull();

            this.transform.position -= direction * spellData.Speed / 10;
        }

    }
}
