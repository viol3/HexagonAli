using HexagonAli.Helpers;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class ColorReferencer : GenericSingleton<ColorReferencer>
    {
        private Color[] _colors;

        public void Init()
        {
            _colors = GameConfiguration.Instance.GetHexagonColors();
        }

        public int GetColorCount()
        {
            return _colors.Length;
        }

        public Color GetColorByIndex(int index)
        {
            return _colors[index];
        }

        public int GetRandomColorIndexExcepts(params int[] colorIndices)
        {
            return HexfallUtility.RandomIntExcept(_colors.Length, colorIndices); ;
        }

    }
}
