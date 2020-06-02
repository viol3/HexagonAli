using System.Collections.Generic;
using HexagonAli.Data;
using HexagonAli.Helpers;
using HexagonAli.View;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class HexagonGroupManager : LocalSingleton<HexagonGroupManager>
    {
        private List<HexagonGroup> _groupList = new List<HexagonGroup>();
        private List<HexagonGroup> _matchedGroups = new List<HexagonGroup>();

        public bool CheckForMatchedGroups()
        {
            _matchedGroups.Clear();
            for (int i = 0; i < _groupList.Count; i++)
            {
                Hexagon a = HexagonManager.Instance.GetHexagon(_groupList[i].A);
                Hexagon b = HexagonManager.Instance.GetHexagon(_groupList[i].B);
                Hexagon c = HexagonManager.Instance.GetHexagon(_groupList[i].C);
                //checking if all colors are same
                if(a.IsSameColorWith(b) && b.IsSameColorWith(c))
                {
                    _matchedGroups.Add(_groupList[i]);
                }
            }
            return _matchedGroups.Count > 0;
        }

        public int ColorCountOfGroup(HexagonGroup group, int colorIndex)
        {
            int count = 0;
            if(colorIndex == HexagonManager.Instance.GetHexagon(group.A).GetColorIndex())
            {
                count++;
            }
            if (colorIndex == HexagonManager.Instance.GetHexagon(group.B).GetColorIndex())
            {
                count++;
            }
            if (colorIndex == HexagonManager.Instance.GetHexagon(group.C).GetColorIndex())
            {
                count++;
            }
            return count;
        }

        public List<HexagonGroup> GetNeighbours(PairedGroup pairedGroup, int commonCount)
        {
            List<HexagonGroup> neighbours = new List<HexagonGroup>();
            for (int i = 0; i < _groupList.Count; i++)
            {
                if(_groupList[i].A.Equals(pairedGroup.Other) || _groupList[i].B.Equals(pairedGroup.Other) || _groupList[i].C.Equals(pairedGroup.Other))
                {
                    //already have 1 common, now counting pairs also
                    int count = 1;
                    for (int j = 0; j < pairedGroup.Pairs.Length; j++)
                    {
                        if (_groupList[i].A.Equals(pairedGroup.Pairs[j]) || _groupList[i].B.Equals(pairedGroup.Pairs[j]) || _groupList[i].C.Equals(pairedGroup.Pairs[j]))
                        {
                            count++;
                        }
                    }
                    if(count == commonCount)
                    {
                        neighbours.Add(_groupList[i]);
                    }
                
                }
            }
            return neighbours;
        }

        public List<PairedGroup> GetAllPairedGroups()
        {
            List<PairedGroup> pairedGroupList = new List<PairedGroup>();
            for (int i = 0; i < _groupList.Count; i++)
            {
                PairedGroup pairedGroup = GetPairedGroup(_groupList[i]);
                if(pairedGroup != null)
                {
                    pairedGroupList.Add(pairedGroup);
                }
            }
            return pairedGroupList;
        }

        PairedGroup GetPairedGroup(HexagonGroup group)
        {
            Hexagon a = HexagonManager.Instance.GetHexagon(group.A);
            Hexagon b = HexagonManager.Instance.GetHexagon(group.B);
            Hexagon c = HexagonManager.Instance.GetHexagon(group.C);
            if (a.IsSameColorWith(b))
            {
                OffsetCoordinate[] pairs = new OffsetCoordinate[2];
                pairs[0] = group.A;
                pairs[1] = group.B;
                OffsetCoordinate other = group.C;
                return new PairedGroup(pairs, other, a.GetColorIndex());
            }

            if (b.IsSameColorWith(c))
            {
                OffsetCoordinate[] pairs = new OffsetCoordinate[2];
                pairs[0] = group.B;
                pairs[1] = group.C;
                OffsetCoordinate other = group.A;
                return new PairedGroup(pairs, other, b.GetColorIndex());
            }

            if (c.IsSameColorWith(a))
            {
                OffsetCoordinate[] pairs = new OffsetCoordinate[2];
                pairs[0] = group.C;
                pairs[1] = group.A;
                OffsetCoordinate other = group.B;
                return new PairedGroup(pairs, other, c.GetColorIndex());
            }

            return null;
        }


        public void ExplodeMatchedGroups()
        {
            Color color = HexagonManager.Instance.GetHexagon(_matchedGroups[0].A).GetColor();
            for (int i = 0; i < _matchedGroups.Count; i++)
            {
                PoolManager.Instance.GetNewExplodeParticle().Explode(HexagonManager.Instance.PixelToWorld(_matchedGroups[i].GetCenter()), color);
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
            //two hexagons on right, only even numbered columns
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

            //two hexagons on right, only odd numbered columns
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

            //two hexagons on left, only even numbered columns
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

            //two hexagons on left, only odd numbered columns
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
}
