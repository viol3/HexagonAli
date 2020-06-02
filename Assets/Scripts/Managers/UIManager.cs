using HexagonAli.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonAli.Managers
{
    public class UIManager : LocalSingleton<UIManager>
    {
#pragma warning disable 0649
        [Header("Texts")]
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Text moveCountText;

        [Header("Game Over")]
        [SerializeField]
        private GameObject gameOverPanel;
        [SerializeField]
        private Text gameOverReasonText;
#pragma warning restore 0649

        private void Start()
        {
            GameManager.Instance.OnHexagonExplode += UpdateScore;
            GameManager.Instance.OnMoveChanged += UpdateMove;
        }

        public void UpdateTexts()
        {
            UpdateScore();
            UpdateMove();
        }

        public void OnTryAgainButtonClick()
        {
            GameManager.Instance.TryAgain();
        }

        public void ShowGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }

        public void HideGameOverPanel()
        {
            gameOverPanel.SetActive(false);
        }

        public void SetGameOverReason(string reason)
        {
            gameOverReasonText.text = reason;
        }

        private void UpdateScore()
        {
            scoreText.text = GameManager.Instance.GetScore().ToString();
        }

        private void UpdateMove()
        {
            moveCountText.text = GameManager.Instance.GetMoveCount().ToString();
        }
    }
}
