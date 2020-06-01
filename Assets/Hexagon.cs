using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private int _colorIndex;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void UpdateColor()
    {
        _spriteRenderer.color = ColorReferencer.Instance.GetColorByIndex(_colorIndex);
    }

    public IEnumerator Move(Vector2 target, float speed)
    {
        yield return transform.DOMove(target, speed).SetSpeedBased().WaitForCompletion();
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void BringFront()
    {
        _spriteRenderer.sortingOrder = 10;
    }

    public void SendBackward()
    {
        _spriteRenderer.sortingOrder = 0;
    }
    
    public void SetColorIndex(int colorIndex)
    {
        _colorIndex = colorIndex;
        UpdateColor();
    }

    public void RandomizeColor(params int[] excepts)
    {
        _colorIndex = Utility.RandomIntExcept(ColorReferencer.Instance.GetColorCount(), excepts);
        //Debug.Log("Result : " + _colorIndex);
        UpdateColor();
    }

    public void SetNewColor(OffsetCoordinate coord)
    {
        OffsetCoordinate leftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row);
        OffsetCoordinate topLeftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row + 1);
        OffsetCoordinate lowerLeftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row - 1);
        //OffsetCoordinate lowerCoord = new OffsetCoordinate(coord.Column, coord.Row - 1);
        if(coord.Row == 0)
        {
            Hexagon leftHexa = HexagonManager.Instance.GetHexagon(leftCoord);
            Hexagon topLeftHexa = HexagonManager.Instance.GetHexagon(topLeftCoord);
            RandomizeColor(leftHexa.GetColorIndex(), topLeftHexa.GetColorIndex());
        }
        else
        {
            Hexagon leftHexa = HexagonManager.Instance.GetHexagon(leftCoord);
            Hexagon lowerLeftHexa = HexagonManager.Instance.GetHexagon(lowerLeftCoord);
            RandomizeColor(leftHexa.GetColorIndex(), lowerLeftHexa.GetColorIndex());
        }
        //Debug.Log("-----------");
    }

    public Color GetColor()
    {
        return ColorReferencer.Instance.GetColorByIndex(_colorIndex);
    }

    public int GetColorIndex()
    {
        return _colorIndex;
    }

    public bool IsSameColorWith(Hexagon other)
    {
        return other.GetColorIndex() == _colorIndex;
    }

    public void Explode()
    {
        transform.eulerAngles = Vector3.zero;
        gameObject.SetActive(false);
    }
}
