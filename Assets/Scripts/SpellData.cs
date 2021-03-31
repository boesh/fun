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
        [SerializeField]
        private ParticleSystem particle;

        private Vector3 direction;

        public Element SpellType => spellType;

        public Mesh SpellMesh => spellMesh;

        public ParticleSystem Particle => particle;

        public Material SpellMaterial => spellMaterial;

        public int Damage => damage;

        public float Speed => speed;

        public Vector3 Direction => direction;
    }
}
