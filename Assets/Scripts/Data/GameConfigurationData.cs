using UnityEngine;

namespace HexagonAli.Data
{
    [CreateAssetMenu(fileName = "Game Configuration", menuName = "Hexfall/Game Configuration", order = 1)]
    public class GameConfigurationData : ScriptableObject
    {
        public int columnCount = 5;
        public int rowCount = 5;
        public int bombScore = 1000;
        public Color[] hexagonColors;
    }
}