using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexfallUtility
{

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
