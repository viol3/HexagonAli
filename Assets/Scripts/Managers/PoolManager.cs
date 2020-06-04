using HexagonAli.Helpers;
using HexagonAli.View;
using Lean.Pool;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class PoolManager : LocalSingleton<PoolManager>
    {
        public LeanGameObjectPool hexagonPool;
        public LeanGameObjectPool bombHexagonPool;
        public LeanGameObjectPool explodeParticlePool;

        public Hexagon GetNewHexagon()
        {
            return hexagonPool.Spawn(Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<Hexagon>();
        }

        public BombHexagon GetNewBombHexagon()
        {
            return bombHexagonPool.Spawn(Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<BombHexagon>();
        }

        public ExplodeParticle GetNewExplodeParticle()
        {
            GameObject explodeParticleGO = explodeParticlePool.Spawn(Vector3.zero, Quaternion.Euler(Vector3.zero));
            if (explodeParticleGO)
            {
                return explodeParticleGO.GetComponent<ExplodeParticle>();
            }
            return null;
        }
            

        public void DespawnExplodeParticle(ExplodeParticle explodeParticle)
        {
            explodeParticlePool.Despawn(explodeParticle.gameObject);
        }

        public void DespawnHexagon(Hexagon hexagon)
        {
            hexagonPool.Despawn(hexagon.gameObject);
        }

        public void DespawnBombHexagon(BombHexagon bombHexagon)
        {
            bombHexagonPool.Despawn(bombHexagon.gameObject);
        }
    }
}
