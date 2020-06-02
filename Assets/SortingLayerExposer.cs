using UnityEngine;
using System.Collections;
public class SortingLayerExposer : MonoBehaviour
{
    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    void Awake()
    {
        GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }
}