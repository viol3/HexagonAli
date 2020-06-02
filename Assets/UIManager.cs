using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : LocalSingleton<UIManager>
{
    [Header("Texts")]
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text moveText;

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

    private void UpdateScore()
    {
        scoreText.text = GameManager.Instance.GetScore().ToString();
    }

    private void UpdateMove()
    {
        moveText.text = GameManager.Instance.GetMoveCount().ToString();
    }
}
