using System.Collections;
using UnityEngine;

public class BombHexagon : Hexagon
{
    [SerializeField]
    private TextMesh counterText;
    private int counter = 6;

    
    public void SetCounter(int counter)
    {
        this.counter = counter;
    }

    public void CountDown()
    {
        counter--;
        UpdateText();
    }

    public bool IsExplodable()
    {
        return counter <= 0;//to be sure :))
    }

    public void UpdateText()
    {
        counterText.text = counter.ToString();
    }
}
