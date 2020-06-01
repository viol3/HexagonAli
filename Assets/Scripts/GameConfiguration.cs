using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfiguration : GenericSingleton<GameConfiguration>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public int ColumnCount = 5;
    public int RowCount = 5;

}
