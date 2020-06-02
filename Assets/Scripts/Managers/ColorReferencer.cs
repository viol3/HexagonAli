using HexagonAli.Helpers;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class ColorReferencer : GenericSingleton<ColorReferencer>
    {
        private Color[] _colors;

        public void Init()
        {
            _colors = GameConfiguration.Instance.hexagonColors;
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
}
