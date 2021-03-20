using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "New GolemData", menuName = "Golem Data", order = 51)]
    class GolemData : ScriptableObject, IGolem
    {

        [SerializeField]
        private Element golemName;
        [SerializeField]
        private Mesh golemMesh;
        [SerializeField]
        private Material golemMaterial;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private int attackDamage;
        [SerializeField]
        private int healthPoint;
        [SerializeField]
        private float mass;


        public Element GolemType => golemName;

        public Mesh GolemMesh => golemMesh;

        public Material GolemMaterial => golemMaterial;

        public float MoveSpeed => moveSpeed;

        public int AttackDamage => attackDamage;

        public int HealthPoint => healthPoint;

        public float Mass => mass;
    }
}
