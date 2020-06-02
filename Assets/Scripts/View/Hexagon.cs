using System.Collections;
using DG.Tweening;
using HexagonAli.Data;
using HexagonAli.Helpers;
using HexagonAli.Managers;
using UnityEngine;

namespace HexagonAli.View
{
    public class Hexagon : MonoBehaviour
    {
        protected SpriteRenderer HexagonSpriteRenderer;
        private int _colorIndex;
        private bool _newlySpawned = true;
        protected virtual void Awake()
        {
            HexagonSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void UpdateColor()
        {
            HexagonSpriteRenderer.color = ColorReferencer.Instance.GetColorByIndex(_colorIndex);
        }

        public IEnumerator Move(Vector2 target, float speed)
        {
            yield return transform.DOMove(target, speed).SetSpeedBased().WaitForCompletion();
        }

        public virtual void BringFront()
        {
            HexagonSpriteRenderer.sortingOrder = 30;
        }

        public virtual void SendBackward()
        {
            HexagonSpriteRenderer.sortingOrder = 0;
        }

        public void RandomizeColor(params int[] excepts)
        {
            _colorIndex = HexfallUtility.RandomIntExcept(ColorReferencer.Instance.GetColorCount(), excepts);
            UpdateColor();
        }

        public void SetNewColor(OffsetCoordinate coord)
        {
            OffsetCoordinate leftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row);
            OffsetCoordinate topLeftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row + 1);
            OffsetCoordinate lowerLeftCoord = new OffsetCoordinate(coord.Column - 1, coord.Row - 1);
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

        public virtual void Explode()
        {
            transform.eulerAngles = Vector3.zero;
            gameObject.SetActive(false);
            gameObject.name = "Disabled Hexagon";
            GameManager.Instance.OnHexagonExploded();
        }

        public void KillInstantly()
        {
            transform.eulerAngles = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void SetNewlySpawnedFlag(bool value)
        {
            _newlySpawned = value;
        }

        public bool IsNewlySpawned()
        {
            return _newlySpawned;
        }
    }
}
