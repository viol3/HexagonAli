using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabReferencer : GenericSingleton<PrefabReferencer>
{
    [SerializeField]
    private GameObject _hexagonPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    public GameObject GetHexagonPrefab()
    {
        return _hexagonPrefab;
    }
}
