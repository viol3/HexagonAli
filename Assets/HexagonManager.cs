using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : LocalSingleton<HexagonManager>
{
    private Transform _parent;

    private Hexagon[,] _hexagons;
    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize(int columnCount, int rowCount)
    {
        _hexagons = new Hexagon[columnCount, rowCount];
    }

    public void GenerateHexagons()
    {
        for (int i = 0; i < GameConfiguration.Instance.ColumnCount; i++)
        {
            for (int j = 0; j < GameConfiguration.Instance.RowCount; j++)
            {
                OffsetCoordinate coord = new OffsetCoordinate(i, j);
                Hexagon hexa = PoolManager.Instance.GetNewHexagon();
                hexa.gameObject.SetActive(true);
                if(_parent == null)
                {
                    _parent = hexa.transform.parent;
                }
                hexa.transform.position = coord.ToPixel() - new Vector2(2f, 2f);
                if(i > 0)
                {
                    hexa.SetNewColor(coord);
                }
                else
                {
                    hexa.RandomizeColor();
                }
                SetHexagon(coord, hexa);
            }
        }
    }

    public void ShiftColumns()
    {
        for (int column = 0; column < GameConfiguration.Instance.ColumnCount; column++)
        {
            int emptyCellCount = 0;
            for (int row = 0; row < GameConfiguration.Instance.RowCount; row++)
            {
                if(_hexagons[column, row] == null)
                {
                    emptyCellCount++;
                }
                else if(emptyCellCount > 0)
                {
                    Hexagon hexagon = ChangeHexagonLocation(new OffsetCoordinate(column, row), new OffsetCoordinate(column, row - emptyCellCount));
                    Vector2 newPos = new OffsetCoordinate(column, row - emptyCellCount).ToPixel() - new Vector2(2f, 2f);
                    StartCoroutine(hexagon.Move(newPos, 5f));
                }
            }
            if(emptyCellCount > 0)
            {
                SpawnNewHexagons(column, emptyCellCount);

            }
            
        }
    }

    void SpawnNewHexagons(int column, int emptyCount)
    {
        for (int i = 0; i < emptyCount; i++)
        {
            OffsetCoordinate coord = new OffsetCoordinate(column, GameConfiguration.Instance.RowCount - 1 - i);
            Vector2 target = coord.ToPixel() - new Vector2(2f, 2f);
            Hexagon newHexagon = PoolManager.Instance.GetNewHexagon();
            newHexagon.transform.position = new Vector3(target.x, 7f + ((emptyCount - i) * 0.7f));
            newHexagon.RandomizeColor();
            newHexagon.gameObject.SetActive(true);
            _hexagons[coord.Column, coord.Row] = newHexagon;
            StartCoroutine(newHexagon.Move(target, 10f));
        }
    }

    public void ExplodeHexagon(OffsetCoordinate coord)
    {
        if(_hexagons[coord.Column, coord.Row] == null)
        {
            return;
        }
        _hexagons[coord.Column, coord.Row].Explode();
        _hexagons[coord.Column, coord.Row] = null;
    }

    public Transform GetParent()
    {
        return _parent;
    }

    public Hexagon GetHexagon(int column, int row)
    {
        return _hexagons[column, row];
    }

    public Hexagon GetHexagon(OffsetCoordinate coord)
    {
        return _hexagons[coord.Column, coord.Row];
    }

    public void SetHexagon(OffsetCoordinate coord, Hexagon hexagon)
    {
        _hexagons[coord.Column, coord.Row] = hexagon;
    }

    private Hexagon ChangeHexagonLocation(OffsetCoordinate oldCoord, OffsetCoordinate newCoord)
    {
        Hexagon hexOld = GetHexagon(oldCoord);
        SetHexagon(oldCoord, GetHexagon(newCoord));
        SetHexagon(newCoord, hexOld);
        return hexOld;
    }

    public static float GetHexagonWidth()
    {
        return 0.8f;
    }

    public static float GetHexagonHeight()
    {
        return 0.4f * Mathf.Sqrt(3);
    }

}
