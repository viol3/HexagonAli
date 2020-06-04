using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace HexagonAli.View
{
    public class Hexagon : MonoBehaviour
    {
        protected SpriteRenderer HexagonSpriteRenderer;
        private int _colorIndex;
        private bool _newlySpawned = true;
        private bool _onEventSubbed = false;

        public UnityEvent OnExploded;

        protected virtual void Awake()
        {
            HexagonSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetColor(Color color)
        {
            HexagonSpriteRenderer.color = color;
        }

        public void SetColorIndex(int index)
        {
            _colorIndex = index;
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


        public Color GetColor()
        {
            return HexagonSpriteRenderer.color;
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
            //gameObject.name = "Disabled Hexagon";
            OnExploded.Invoke();
        }

        public void SetNewlySpawnedFlag(bool value)
        {
            _newlySpawned = value;
        }

        public void SetEventSubbedFlag(bool value)
        {
            _onEventSubbed = value;
        }

        public bool IsNewlySpawned()
        {
            return _newlySpawned;
        }

        public bool IsEventSubbed()
        {
            return _onEventSubbed;
        }
    }
}
