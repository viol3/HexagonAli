using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    public event Action OnHexagonExplode;
    public event Action OnMoveChanged;
    public event Action OnThousandScoreReached;

    private HexagonGroup _selectedGroup;

    private int score = 0;
    private int highscore = 0;
    private int moveCount = 0;
    private int nextBombScore;
    

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        ColorReferencer.Instance.Init();
        InputManager.Instance.OnTap += OnTap;
        HexagonManager.Instance.Initialize(GameConfiguration.Instance.ColumnCount, GameConfiguration.Instance.RowCount);
        HexagonManager.Instance.GenerateHexagons();
        HexagonGroupManager.Instance.CalculateGroups(GameConfiguration.Instance.ColumnCount, GameConfiguration.Instance.RowCount);
        UIManager.Instance.UpdateTexts();
        nextBombScore = GameConfiguration.Instance.BombScore;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RotateTask(true, 0.2f));
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(AreThereAnyPossibleMoves());
        }
    }

    public void OnHexagonExploded()
    {
        score += 5;
        if (score >= nextBombScore)
        {
            nextBombScore += GameConfiguration.Instance.BombScore ;
            HexagonManager.Instance.SetNextBombFlag(true);
            OnThousandScoreReached?.Invoke();
        }
        OnHexagonExplode?.Invoke();
    }

    private void IncreaseMoveCount()
    {
        moveCount++;
        OnMoveChanged?.Invoke();
    }

    private void OnTap(Vector3 pos)
    {
        _selectedGroup = HexagonGroupManager.Instance.NearestGroupTo(pos + (Vector3)HexagonManager.Instance.CenterOffset());
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
                IncreaseMoveCount();
                anyMatch = true;
            }
            if(anyMatch)
            {
                HexagonManager.Instance.CountdownBombHexagons();
                yield break;
            }
        }
        
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighscore()
    {
        return highscore;
    }

    public int GetMoveCount()
    {
        return moveCount;
    }
   
    bool AreThereAnyPossibleMoves()
    {
        List<PairedGroup> pairedGroups = HexagonGroupManager.Instance.GetAllPairedGroups();
        for (int i = 0; i < pairedGroups.Count; i++)
        {
            List<HexagonGroup> singleCommonGroups = HexagonGroupManager.Instance.GetNeighbours(pairedGroups[i], 1);
            for (int j = 0; j < singleCommonGroups.Count; j++)
            {
                if(HexagonGroupManager.Instance.ColorCountOfGroup(singleCommonGroups[j], pairedGroups[i].colorIndex) >= 1)
                {
                    return true;
                }
            }

            List<HexagonGroup> doubleCommonGroups = HexagonGroupManager.Instance.GetNeighbours(pairedGroups[i], 2);
            for (int j = 0; j < doubleCommonGroups.Count; j++)
            {
                if (HexagonGroupManager.Instance.ColorCountOfGroup(doubleCommonGroups[j], pairedGroups[i].colorIndex) >= 2)
                {
                    return true;
                }
            }
        }
        return false;
    }




}
