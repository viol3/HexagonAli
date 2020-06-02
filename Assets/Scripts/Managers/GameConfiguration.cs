using HexagonAli.Helpers;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class GameConfiguration : GenericSingleton<GameConfiguration>
    {
        protected override void Awake()
        {
            base.Awake();
            if(columnCount < 3)
            {
                columnCount = 3;
            }

            if (rowCount < 3)
            {
                rowCount = 3;
            }
        }

        public int columnCount = 5;
        public int rowCount = 5;
        public int bombScore = 1000;
        public Color[] hexagonColors; 

        

    }
}
