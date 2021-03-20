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
        private Element spellName;
        [SerializeField]
        private Mesh spellMesh;
        [SerializeField]
        private Material spellMaterial;
        [SerializeField]
        private int damage;
        [SerializeField]
        private float speed;

        private Vector3 direction;

        public Element SpellType => spellName;

        public Mesh SpellMesh => spellMesh;

        public Material SpellMaterial => spellMaterial;

        public int Damage => damage;

        public float Speed => speed;

        public Vector3 Direction => direction;
    }
}
