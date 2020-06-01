using UnityEngine;

public struct HexagonGroup
{
    public OffsetCoordinate A { get; }
    public OffsetCoordinate B { get; }
    public OffsetCoordinate C { get; }

    public GroupRotation Rotation { get; }

    public HexagonGroup(OffsetCoordinate a, OffsetCoordinate b, OffsetCoordinate c,
        GroupRotation rotation)
    {
        A = a;
        B = b;
        C = c;
        Rotation = rotation;
    }

    public Vector2 GetCenter()
    {
        return (A.ToPixel() + B.ToPixel() + C.ToPixel()) / 3f;
    }
}