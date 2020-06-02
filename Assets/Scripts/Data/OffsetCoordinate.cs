using HexagonAli.Managers;
using UnityEngine;

namespace HexagonAli.Data
{
    /// <summary>
    /// This struct referenced from https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    public struct OffsetCoordinate
    {
        public int Row;
        public int Column;

        public OffsetCoordinate(int column, int row)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Converts to Pixel Coordinates(Not World Position)
        /// </summary>
        public Vector2 ToPixel()
        {
            float x = HexagonManager.GetEdgeLength() * (3 / 2f * Column);
            float y = HexagonManager.GetEdgeLength() * Mathf.Sqrt(3) * (Row - 0.5f * (Column & 1));
            return new Vector2(x, y);
        }

        public bool Equals(OffsetCoordinate other)
        {
            return Row == other.Row && Column == other.Column;
        }


    }
}

