using HexagonAli.Data;
using HexagonAli.Managers;
using UnityEngine;

namespace HexagonAli.View
{
    public class BombHexagon : Hexagon
    {
#pragma warning disable 0649
        [SerializeField]
        private TextMesh counterText;
        private int _counter = 6;
        private MeshRenderer _textMeshRenderer;
#pragma warning restore 0649

        protected override void Awake()
        {
            base.Awake();
            _textMeshRenderer = counterText.GetComponent<MeshRenderer>();
            _textMeshRenderer.sortingLayerName = "Default";
            _textMeshRenderer.sortingOrder = 15;
        }
        public void SetCounter(int counter)
        {
            this._counter = counter;
        }

        private void Update()
        {
            _textMeshRenderer.transform.eulerAngles = Vector3.zero;
        }

        public void CountDown()
        {
            _counter--;
            UpdateText();
            if(_counter <= 0)
            {
                GameManager.Instance.LoseGame(GameOverReason.BombExploded);
            }
        }

        public override void Explode()
        {
            base.Explode();
            HexagonManager.Instance.RemoveBombHexagonFromList(this);
        }

        public override void BringFront()
        {
            base.BringFront();
            _textMeshRenderer.sortingOrder = HexagonSpriteRenderer.sortingOrder + 5;
        }

        public override void SendBackward()
        {
            base.SendBackward();
            _textMeshRenderer.sortingOrder = HexagonSpriteRenderer.sortingOrder + 5;
        }

        public void UpdateText()
        {
            counterText.text = _counter.ToString();
        }
    }
}
