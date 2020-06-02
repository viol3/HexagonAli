
using UnityEngine;

public struct OffsetCoordinate
{
    public int Row { get; }
    public int Column { get; }

    public OffsetCoordinate(int column, int row)
    {
        Row = row;
        Column = column;
    }

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
