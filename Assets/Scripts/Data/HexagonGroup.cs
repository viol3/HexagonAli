using UnityEngine;

namespace HexagonAli.Data
{
    public struct HexagonGroup
    {
        public OffsetCoordinate A;
        public OffsetCoordinate B;
        public OffsetCoordinate C;

        public GroupRotation Rotation;

        public HexagonGroup(OffsetCoordinate a, OffsetCoordinate b, OffsetCoordinate c, GroupRotation rotation)
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
}
