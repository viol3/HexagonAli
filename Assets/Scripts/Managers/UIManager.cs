using System;
using HexagonAli.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonAli.Managers
{
    public class UIManager : LocalSingleton<UIManager>
    {
#pragma warning disable 0649
        [Header("Texts")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text moveCountText;

        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Text gameOverReasonText;
#pragma warning restore 0649

        public event Action OnTryAgain;

        public void OnTryAgainButtonClick()
        {
            OnTryAgain?.Invoke();
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

        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateMoveCount(int moveCount)
        {
            moveCountText.text = moveCount.ToString();
        }
    }
}
