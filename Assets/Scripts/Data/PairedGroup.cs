namespace HexagonAli.Data
{
    public class PairedGroup
    {
        /// <summary>
        /// This class is for implementation of checking that is game over or not.
        /// Pairs are for the hexagons that same color.
        /// Other is for the hexagon that different color.
        /// </summary>
        public OffsetCoordinate[] Pairs;
        public OffsetCoordinate Other;
        public int ColorIndex;

        public PairedGroup(OffsetCoordinate[] pairs, OffsetCoordinate other, int colorIndex)
        {
            this.Pairs = pairs;
            this.Other = other;
            this.ColorIndex = colorIndex;
        }

    }
}

