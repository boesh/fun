using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "New SpellData", menuName = "Spell Data", order = 50)]

    class SpellData : ScriptableObject, ISpell
    {
        [SerializeField]
        private Element spellType;
        [SerializeField]
        private Mesh spellMesh;
        [SerializeField]
        private Material spellMaterial;
        [SerializeField]
        private int damage;
        [SerializeField]
        private float speed;
        
        public Element SpellType => spellType;

        public Mesh SpellMesh => spellMesh;

        public Material SpellMaterial => spellMaterial;

        public int Damage => damage;

        public float Speed => speed;

    }
}
