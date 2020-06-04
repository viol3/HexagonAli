using System.Collections.Generic;
using HexagonAli.Data;
using HexagonAli.Helpers;
using HexagonAli.View;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class HexagonManager : LocalSingleton<HexagonManager>
    {
#pragma warning disable 0649
        [SerializeField]
        private Transform parent;

        private Hexagon[,] _hexagons;

        private List<BombHexagon> _bombHexagons = new List<BombHexagon>();

        private bool _nextHexagonIsBomb = false;
#pragma warning restore 0649

        public void Initialize(int columnCount, int rowCount)
        {
            _hexagons = new Hexagon[columnCount, rowCount];
        }

        public void GenerateHexagons()
        {
            for (int i = 0; i < GameConfiguration.Instance.GetColumnCount(); i++)
            {
                for (int j = 0; j < GameConfiguration.Instance.GetRowCount(); j++)
                {
                    OffsetCoordinate coord = new OffsetCoordinate(i, j);
                    Hexagon hexa = PoolManager.Instance.GetNewHexagon();
                    hexa.gameObject.SetActive(true);
                    hexa.name = "[" + i + ", " + j + "]";
                    hexa.transform.SetParent(parent);
                    if (!hexa.IsEventSubbed())
                    {
                        hexa.SetEventSubbedFlag(true);
                        hexa.OnExploded.AddListener(() => GameManager.Instance.OnHexagonExploded());
                        hexa.OnExploded.AddListener(() => PoolManager.Instance.DespawnHexagon(hexa));
                    }
                    hexa.transform.position = CoordToWorld(coord);
                    if(i > 0)
                    {
                        //avoid pre-matches at startup
                        SetDifferentColorToHexagon(hexa, coord);
                    }
                    else
                    {
                        int colorIndex = ColorReferencer.Instance.GetRandomColorIndexExcepts();
                        hexa.SetColor(ColorReferencer.Instance.GetColorByIndex(colorIndex));
                        hexa.SetColorIndex(colorIndex);
                    }
                    SetHexagon(coord, hexa);
                }
            }
        }

        public void RemoveBombHexagonFromList(BombHexagon bombHexagon)
        {
            _bombHexagons.Remove(bombHexagon);
        }

        public void ClearBombHexagonList()
        {
            _bombHexagons.Clear();
        }

        public void KillAllHexagons()
        {
            for (int i = 0; i < _hexagons.GetLength(0); i++)
            {
                for (int j = 0; j < _hexagons.GetLength(1); j++)
                {
                    if (_hexagons[i, j].GetType() == typeof(BombHexagon))
                    {
                        PoolManager.Instance.DespawnBombHexagon((BombHexagon)_hexagons[i, j]);
                    }
                    else
                    {
                        PoolManager.Instance.DespawnHexagon(_hexagons[i, j]);
                    }
                    
                    _hexagons[i, j] = null;
                }
            }
        }

        public void ShiftColumns()
        {
            //first drop down hexagons, after spawn new hexagons
            //hata
            for (int column = 0; column < GameConfiguration.Instance.GetColumnCount(); column++)
            {
                //counting empty tiles
                int emptyCount = 0;
                for (int row = 0; row < GameConfiguration.Instance.GetRowCount(); row++)
                {
                    if(_hexagons[column, row] == null)
                    {
                        emptyCount++;
                    }
                    else if(emptyCount > 0)
                    {
                        //drop down hexagons in air
                        Hexagon hexagon = SwapHexagonLocation(new OffsetCoordinate(column, row), new OffsetCoordinate(column, row - emptyCount));
                        Vector2 newPos = CoordToWorld(new OffsetCoordinate(column, row - emptyCount));
                        StartCoroutine(hexagon.Move(newPos, 5f));
                    }
                }
                if(emptyCount > 0)
                {
                    SpawnNewHexagons(column, emptyCount);

                }
            
            }
        }

        void SpawnNewHexagons(int column, int emptyCount)
        {
            for (int i = 0; i < emptyCount; i++)
            {
            
                OffsetCoordinate coord = new OffsetCoordinate(column, GameConfiguration.Instance.GetRowCount() - 1 - i);
                Vector2 target = CoordToWorld(coord);
                Hexagon newHexagon;
                if (_nextHexagonIsBomb)
                {
                    newHexagon = PoolManager.Instance.GetNewBombHexagon();
                    BombHexagon bombHexagon = (BombHexagon) newHexagon;
                    bombHexagon.SetCounter(5);
                    bombHexagon.UpdateText();
                    //if detonate event not registered yet
                    if (bombHexagon.OnBombDetonated.GetPersistentEventCount() == 0)
                    {
                        
                    }
                    if (!bombHexagon.IsEventSubbed())
                    {
                        bombHexagon.OnBombDetonated.AddListener(
                            () => GameManager.Instance.LoseGame(GameOverReason.BombExploded));
                        bombHexagon.OnExploded.AddListener(() => GameManager.Instance.OnHexagonExploded());
                        bombHexagon.OnExploded.AddListener(() => RemoveBombHexagonFromList(bombHexagon));
                        bombHexagon.OnExploded.AddListener(() => PoolManager.Instance.DespawnBombHexagon(bombHexagon));
                        bombHexagon.SetEventSubbedFlag(true);
                    }

                    bombHexagon.SetNewlySpawnedFlag(true);
                    _nextHexagonIsBomb = false;
                    _bombHexagons.Add(bombHexagon);
                }
                else
                {
                    newHexagon = PoolManager.Instance.GetNewHexagon();
                    if (!newHexagon.IsEventSubbed())
                    {
                        newHexagon.OnExploded.AddListener(() => PoolManager.Instance.DespawnHexagon(newHexagon));
                        newHexagon.OnExploded.AddListener(() => GameManager.Instance.OnHexagonExploded());
                        newHexagon.SetEventSubbedFlag(true);
                    }
                }
                newHexagon.name = "[" + coord.Column + ", " + coord.Row + "]";
                newHexagon.transform.SetParent(parent);
                newHexagon.transform.position = new Vector3(target.x, CameraManager.Instance.GetTopOfCamera() + 1.5f + ((emptyCount - i) * 0.7f));
                int colorIndex = ColorReferencer.Instance.GetRandomColorIndexExcepts();
                newHexagon.SetColorIndex(colorIndex);
                newHexagon.SetColor(ColorReferencer.Instance.GetColorByIndex(colorIndex));
                newHexagon.gameObject.SetActive(true);
                _hexagons[coord.Column, coord.Row] = newHexagon;
                StartCoroutine(newHexagon.Move(target, 10f));
            }
        }

        public void SetDifferentColorToHexagon(Hexagon hexagon, OffsetCoordinate coord)
        {
            OffsetCoordinate leftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row);
            OffsetCoordinate topLeftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row + 1);
            OffsetCoordinate lowerLeftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row - 1);
            if (coord.Row == 0)
            {
                Hexagon leftHexa = GetHexagon(leftCoord);
                Hexagon topLeftHexa = GetHexagon(topLeftCoord);
                int colorIndex = ColorReferencer.Instance.GetRandomColorIndexExcepts(leftHexa.GetColorIndex(), topLeftHexa.GetColorIndex());
                hexagon.SetColorIndex(colorIndex);
                hexagon.SetColor(ColorReferencer.Instance.GetColorByIndex(colorIndex));
            }
            else
            {
                Hexagon leftHexa = GetHexagon(leftCoord);
                Hexagon lowerLeftHexa = GetHexagon(lowerLeftCoord);
                int colorIndex = ColorReferencer.Instance.GetRandomColorIndexExcepts(leftHexa.GetColorIndex(), lowerLeftHexa.GetColorIndex());
                hexagon.SetColorIndex(colorIndex);
                hexagon.SetColor(ColorReferencer.Instance.GetColorByIndex(colorIndex));
            }
        }

        public void SetNextBombFlag(bool value)
        {
            _nextHexagonIsBomb = value;
        }

        public void CountdownBombHexagons()
        {
            for (int i = 0; i < _bombHexagons.Count; i++)
            {
                if(_bombHexagons[i].IsNewlySpawned())
                {
                    _bombHexagons[i].SetNewlySpawnedFlag(false);
                }
                else
                {
                    _bombHexagons[i].CountDown();
                }
            
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

        public Hexagon GetHexagon(OffsetCoordinate coord)
        {
            return _hexagons[coord.Column, coord.Row];
        }

        public void SetHexagon(OffsetCoordinate coord, Hexagon hexagon)
        {
            _hexagons[coord.Column, coord.Row] = hexagon;
        }

        private Hexagon SwapHexagonLocation(OffsetCoordinate oldCoord, OffsetCoordinate newCoord)
        {
            Hexagon hexOld = GetHexagon(oldCoord);
            SetHexagon(oldCoord, GetHexagon(newCoord));
            SetHexagon(newCoord, hexOld);
            return hexOld;
        }

        public static float GetHexagonWidth()
        {
            return GetEdgeLength() * 2f;
        }

        public static float GetHexagonHeight()
        {
            return GetEdgeLength() * Mathf.Sqrt(3);
        }

        public static float GetEdgeLength()
        {
            return 0.4f;
        }

        public Vector2 CenterOffset()
        {
            int columnCount = _hexagons.GetLength(0);
            int rowCount = _hexagons.GetLength(1);
            return new Vector2(GetHexagonWidth() * 0.75f * (columnCount - 1) / 2f, GetHexagonHeight() * (rowCount - 1) / 3f);
        }

        public Vector3 PixelToWorld(Vector2 pixel)
        {
            return pixel - CenterOffset();
        }

        public Vector3 CoordToWorld(OffsetCoordinate coord)
        {
            return coord.ToPixel() - CenterOffset();
        }

    }
}
