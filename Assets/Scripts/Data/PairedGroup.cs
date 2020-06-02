using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairedGroup
{
    public OffsetCoordinate[] pairs;
    public OffsetCoordinate other;
    public int colorIndex;

    public PairedGroup(OffsetCoordinate[] pairs, OffsetCoordinate other, int colorIndex)
    {
        this.pairs = pairs;
        this.other = other;
        this.colorIndex = colorIndex;
    }

}
