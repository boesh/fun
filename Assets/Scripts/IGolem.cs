using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IGolem
    {
        Element GolemType { get; }

        float MoveSpeed { get; }

        int AttackDamage { get; }

        int HealthPoint { get; }

        float Mass { get; }
    }
}
