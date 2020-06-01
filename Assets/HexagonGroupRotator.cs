using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGroupRotator : LocalSingleton<HexagonGroupRotator>
{
    private SpriteRenderer _outlineSprite;
    private bool _active = false;
    private Hexagon[] outlinedHexagons = new Hexagon[3];

    protected override void Awake()
    {
        base.Awake();
        _outlineSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetHexagonGroup(HexagonGroup group)
    {
        _active = true;
        _outlineSprite.enabled = true;
        transform.position = group.GetCenter() - new Vector2(2f, 2f);
        
        if (group.Rotation == GroupRotation.TwoHexagonsLeft)
        {
            _outlineSprite.transform.localPosition = Vector3.right * 0.36f;
            _outlineSprite.transform.eulerAngles = Vector3.forward * 180f;
        }
        else
        {
            _outlineSprite.transform.localPosition = Vector3.left * 0.36f;
            _outlineSprite.transform.eulerAngles = Vector3.forward * 0;
        }

        outlinedHexagons[0] = HexagonManager.Instance.GetHexagon(group.A);
        outlinedHexagons[1] = HexagonManager.Instance.GetHexagon(group.B);
        outlinedHexagons[2] = HexagonManager.Instance.GetHexagon(group.C);
    }

    public void ParentSelectedHexagons()
    {
        for (int i = 0; i < outlinedHexagons.Length; i++)
        {
            if(outlinedHexagons[i])
            {
                outlinedHexagons[i].transform.SetParent(transform);
                outlinedHexagons[i].BringFront();
            }
        }
    }

    public void UnparentSelectedHexagons()
    {
        for (int i = 0; i < outlinedHexagons.Length; i++)
        {
            if (outlinedHexagons[i])
            {
                outlinedHexagons[i].transform.SetParent(HexagonManager.Instance.GetParent());
                outlinedHexagons[i].SendBackward();
            }
        }
    }

    public void Reset()
    {
        _active = false;
        _outlineSprite.enabled = false;
        for (int i = 0; i < outlinedHexagons.Length; i++)
        {
            outlinedHexagons[i] = null;
        }
        transform.eulerAngles = Vector3.zero;
    }

    public IEnumerator Rotate(bool clockwise, float angle, float duration)
    {
        yield return transform.DORotate((clockwise ? Vector3.back : Vector3.forward) * angle, duration).SetRelative().WaitForCompletion();
    }
}
