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
        private Transform _oldParent;
#pragma warning restore 0649

        public void UpdateRotation(GroupRotation groupRotation)
        {
            if (groupRotation == GroupRotation.TwoHexagonsLeft)
            {
                //because i don't have perfect centerized group texture for this game
                outlineSprite.transform.localPosition = Vector3.right * 0.36f;
                outlineSprite.transform.eulerAngles = Vector3.forward * 180f;
            }
            else
            {
                outlineSprite.transform.localPosition = Vector3.left * 0.36f;
                outlineSprite.transform.eulerAngles = Vector3.forward * 0;
            }
        }

        public void SetHexagons(Hexagon a, Hexagon b, Hexagon c)
        {
            _outlinedHexagons[0] = a;
            _outlinedHexagons[1] = b;
            _outlinedHexagons[2] = c;
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public void ParentSelectedHexagons()
        {
            for (int i = 0; i < _outlinedHexagons.Length; i++)
            {
                if(_outlinedHexagons[i])
                {
                    if (_oldParent == null)
                    {
                        _oldParent = _outlinedHexagons[i].transform.parent;
                    }
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
                    _outlinedHexagons[i].transform.SetParent(_oldParent);
                    _outlinedHexagons[i].SendBackward();
                }
            }
            _oldParent = null;
        }
        public void Activate()
        {
            outlineSprite.gameObject.SetActive(true);
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
