using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ISpell
    {
        Element SpellType { get; }

        Mesh SpellMesh { get; }

        Material SpellMaterial { get; }

        int Damage { get; }

        float Speed { get; }

        Vector3 Direction { get; }
    }
}
