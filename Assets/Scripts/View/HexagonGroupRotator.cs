using System.Collections;
using DG.Tweening;
using HexagonAli.Data;
using HexagonAli.Helpers;
using HexagonAli.Managers;
using UnityEngine;

namespace HexagonAli.View
{
    public class HexagonGroupRotator : LocalSingleton<HexagonGroupRotator>
    {
#pragma warning disable 0649
        [SerializeField]
        private SpriteRenderer outlineSprite;
        private Hexagon[] _outlinedHexagons = new Hexagon[3];
#pragma warning restore 0649

        public void SetHexagonGroup(HexagonGroup group)
        {
            outlineSprite.gameObject.SetActive(true);
            transform.position = HexagonManager.Instance.PixelToWorld(group.GetCenter());
        
            if (group.Rotation == GroupRotation.TwoHexagonsLeft)
            {
                outlineSprite.transform.localPosition = Vector3.right * 0.36f;
                outlineSprite.transform.eulerAngles = Vector3.forward * 180f;
            }
            else
            {
                outlineSprite.transform.localPosition = Vector3.left * 0.36f;
                outlineSprite.transform.eulerAngles = Vector3.forward * 0;
            }

            _outlinedHexagons[0] = HexagonManager.Instance.GetHexagon(group.A);
            _outlinedHexagons[1] = HexagonManager.Instance.GetHexagon(group.B);
            _outlinedHexagons[2] = HexagonManager.Instance.GetHexagon(group.C);
        }

        public void ParentSelectedHexagons()
        {
            for (int i = 0; i < _outlinedHexagons.Length; i++)
            {
                if(_outlinedHexagons[i])
                {
                    _outlinedHexagons[i].transform.SetParent(transform);
                    _outlinedHexagons[i].BringFront();
                }
            }
        }

        public void UnparentSelectedHexagons()
        {
            for (int i = 0; i < _outlinedHexagons.Length; i++)
            {
                if (_outlinedHexagons[i])
                {
                    _outlinedHexagons[i].transform.SetParent(HexagonManager.Instance.GetParent());
                    _outlinedHexagons[i].SendBackward();
                }
            }
        }

        public void Reset()
        {
            outlineSprite.gameObject.SetActive(false);
            for (int i = 0; i < _outlinedHexagons.Length; i++)
            {
                _outlinedHexagons[i] = null;
            }
            transform.eulerAngles = Vector3.zero;
        }

        public IEnumerator Rotate(bool clockwise, float angle, float duration)
        {
            yield return transform.DORotate((clockwise ? Vector3.back : Vector3.forward) * angle, duration).SetRelative().WaitForCompletion();
        }
    }
}
