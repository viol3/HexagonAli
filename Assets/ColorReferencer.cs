using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorReferencer : GenericSingleton<ColorReferencer>
{
    private Color[] _colors;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        _colors = GameConfiguration.Instance.HexagonColors;
    }

    public int GetColorCount()
    {
        return _colors.Length;
    }

    public Color GetColorByIndex(int colorIndex)
    {
        return _colors[colorIndex];
    }

}
