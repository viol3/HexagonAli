using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    public GameObject hexagonPrefab;

    private HexagonGroup _selectedGroup;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.OnTap += OnTap;
        HexagonManager.Instance.Initialize(GameConfiguration.Instance.ColumnCount, GameConfiguration.Instance.RowCount);
        HexagonManager.Instance.GenerateHexagons();
        HexagonGroupManager.Instance.CalculateGroups(GameConfiguration.Instance.ColumnCount, GameConfiguration.Instance.RowCount);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RotateTask(true, 0.2f));
        }
    }

    private void OnTap(Vector3 pos)
    {
        _selectedGroup = HexagonGroupManager.Instance.NearestGroupTo(pos + new Vector3(2f, 2f));
        HexagonGroupRotator.Instance.SetHexagonGroup(_selectedGroup);
    }
    
    IEnumerator RotateTask(bool clockwise, float duration)
    {
        for (int i = 0; i < 3; i++)
        {
            Hexagon a = HexagonManager.Instance.GetHexagon(_selectedGroup.A);
            Hexagon b = HexagonManager.Instance.GetHexagon(_selectedGroup.B);
            Hexagon c = HexagonManager.Instance.GetHexagon(_selectedGroup.C);
            if(clockwise)
            {
                HexagonManager.Instance.SetHexagon(_selectedGroup.B, a);
                HexagonManager.Instance.SetHexagon(_selectedGroup.C, b);
                HexagonManager.Instance.SetHexagon(_selectedGroup.A, c);
            }
            else
            {
                HexagonManager.Instance.SetHexagon(_selectedGroup.C, a);
                HexagonManager.Instance.SetHexagon(_selectedGroup.B, c);
                HexagonManager.Instance.SetHexagon(_selectedGroup.A, b);
            }
            

            HexagonGroupRotator.Instance.ParentSelectedHexagons();
            yield return HexagonGroupRotator.Instance.Rotate(clockwise, 120f, duration);
            HexagonGroupRotator.Instance.UnparentSelectedHexagons();
            yield return new WaitForSeconds(0.1f);
            bool anyMatch = false;
            while(HexagonGroupManager.Instance.CheckForMatchedGroups())
            {
                HexagonGroupManager.Instance.ExplodeMatchedGroups();
                HexagonGroupRotator.Instance.Reset();
                HexagonManager.Instance.ShiftColumns();
                yield return new WaitForSeconds(0.6f);
                anyMatch = true;
            }
            if(anyMatch)
            {
                yield break;
            }
        }
        
    }

   


}
