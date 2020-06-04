using HexagonAli.Data;
using HexagonAli.Helpers;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class GameConfiguration : GenericSingleton<GameConfiguration>
    {
#pragma warning disable 0649
        [SerializeField] private GameConfigurationData currentConfiguration;
#pragma warning restore 0649
        public int GetColumnCount()
        {
            if (currentConfiguration.columnCount < 3)
            {
                return 3;
            }
            return currentConfiguration.columnCount;
        }

        public int GetRowCount()
        {
            if (currentConfiguration.rowCount < 3)
            {
                return 3;
            }
            return currentConfiguration.rowCount;
        }

        public int GetBombScore()
        {
            return currentConfiguration.bombScore;
        }

        public Color[] GetHexagonColors()
        {
            return currentConfiguration.hexagonColors;
        }

    }
}
