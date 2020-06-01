using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexagonGroupManager : LocalSingleton<HexagonGroupManager>
{
    private readonly List<HexagonGroup> _groupList = new List<HexagonGroup>();
    private List<HexagonGroup> _matchedGroups = new List<HexagonGroup>();
    protected override void Awake()
    {
        base.Awake();
    }

    public bool CheckForMatchedGroups()
    {
        _matchedGroups.Clear();
        for (int i = 0; i < _groupList.Count; i++)
        {
            Hexagon a = HexagonManager.Instance.GetHexagon(_groupList[i].A);
            Hexagon b = HexagonManager.Instance.GetHexagon(_groupList[i].B);
            Hexagon c = HexagonManager.Instance.GetHexagon(_groupList[i].C);
            if(a.IsSameColorWith(b) && b.IsSameColorWith(c))
            {
                _matchedGroups.Add(_groupList[i]);
            }
        }
        return _matchedGroups.Count > 0;
    }

    public void ExplodeMatchedGroups()
    {
        Color color = HexagonManager.Instance.GetHexagon(_matchedGroups[0].A).GetColor();
        for (int i = 0; i < _matchedGroups.Count; i++)
        {
            PoolManager.Instance.GetNewExplodeParticle().Explode(_matchedGroups[i].GetCenter() - new Vector2(2f, 2f), color);
            HexagonManager.Instance.ExplodeHexagon(_matchedGroups[i].A);
            HexagonManager.Instance.ExplodeHexagon(_matchedGroups[i].B);
            HexagonManager.Instance.ExplodeHexagon(_matchedGroups[i].C);
        }
    }

    public HexagonGroup NearestGroupTo(Vector2 position)
    {
        HexagonGroup nearest = _groupList[0];
        float minDistance = Vector2.Distance(nearest.GetCenter(), position);
        for (int i = 1; i < _groupList.Count; i++)
        {
            if(minDistance > Vector2.Distance(_groupList[i].GetCenter(), position))
            {
                minDistance = Vector2.Distance(_groupList[i].GetCenter(), position);
                nearest = _groupList[i];
            }
        }
        return nearest;
    }

    public void CalculateGroups(int columnCount, int rowCount)
    {
        //two hexagon rights, only even numbered columns
        for (int column = 0; column < columnCount - 1; column += 2)
        {
            for (int row = 0; row < rowCount - 1; row++)
            {
                var a = new OffsetCoordinate(column, row);
                var b = new OffsetCoordinate(column + 1, row + 1);
                var c = new OffsetCoordinate(column + 1, row);
                _groupList.Add(new HexagonGroup(a, b, c, GroupRotation.TwoHexagonsRight));
            }
        }

        //two hexagon rights, only odd numbered columns
        for (int column = 1; column < columnCount - 1; column += 2)
        {
            for (int row = 1; row < rowCount; row++)
            {
                var a = new OffsetCoordinate(column, row);
                var b = new OffsetCoordinate(column + 1, row);
                var c = new OffsetCoordinate(column + 1, row - 1);
                _groupList.Add(new HexagonGroup(a, b, c, GroupRotation.TwoHexagonsRight));
            }
        }

        //two hexagon lefts, only even numbered columns
        for (int column = 0; column < columnCount - 1; column += 2)
        {
            for (int row = 0; row < rowCount - 1; row++)
            {
                var a = new OffsetCoordinate(column, row);
                var b = new OffsetCoordinate(column, row + 1);
                var c = new OffsetCoordinate(column + 1, row + 1);
                _groupList.Add(new HexagonGroup(a, b, c, GroupRotation.TwoHexagonsLeft));
            }
        }

        //two hexagon lefts, only odd numbered columns
        for (int column = 1; column < columnCount - 1; column += 2)
        {
            for (int row = 0; row < rowCount - 1; row++)
            {
                var a = new OffsetCoordinate(column, row);
                var b = new OffsetCoordinate(column, row + 1);
                var c = new OffsetCoordinate(column + 1, row);
                _groupList.Add(new HexagonGroup(a, b, c, GroupRotation.TwoHexagonsLeft));
            }
        }




    }
}
