using System;

namespace HexagonAli.Helpers
{
    public class HexfallUtility
    {
        /// <summary>
        /// Returns integer number between 0 and <param name="max"> </param>(exclusive)
        /// excepts <param name="excepts"></param> array.
        /// </summary>
        public static int RandomIntExcept(int max, params int[] excepts)
        {

            Array.Sort(excepts);
            int result = UnityEngine.Random.Range(0, max - excepts.Length);
            for (int i = 0; i < excepts.Length; i++)
            {
                if (result < excepts[i])
                    return result;
                result++;
            }
            return result;
        }
    }
}
