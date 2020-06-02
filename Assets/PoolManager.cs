using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : LocalSingleton<PoolManager>
{
    public LeanGameObjectPool HexagonPool;
    public LeanGameObjectPool BombHexagonPool;
    public LeanGameObjectPool ExplodeParticlePool;
    protected override void Awake()
    {
        base.Awake();
    }

    public Hexagon GetNewHexagon()
    {
        return HexagonPool.Spawn(Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<Hexagon>();
    }

    public BombHexagon GetNewBombHexagon()
    {
        return BombHexagonPool.Spawn(Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<BombHexagon>();
    }

    public ExplodeParticle GetNewExplodeParticle()
    {
        return ExplodeParticlePool.Spawn(Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<ExplodeParticle>();
    }
}
