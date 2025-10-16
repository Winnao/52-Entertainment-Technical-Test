using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class DiceController : MonoBehaviour
{
    private int currentScore;

    public TextMeshProUGUI scoreNumberText;

    public Dictionary<int, Vector3> diceFacesOrientations = new Dictionary<int, Vector3>()
    {
        {1, new Vector3(0,0,180)},
        {2, new Vector3(0,0,-90)},
        {3, new Vector3(-90,0,0)},
        {4, new Vector3(90,0,0)},
        {5, new Vector3(0,0,90)},
        {6, new Vector3(0,0,0)}
    };

    private void Awake()
    {
        currentScore = 0;
        scoreNumberText.text = currentScore.ToString();
    }

    public void OnPressRollButton()
    {
        RollTheDice();
    }

    public void OnPressCheatRollButton()
    {
        RollTheDice(6);
    }

    private void RollTheDice(int cheatValue = 0)
    {
        if (cheatValue >= 1 && cheatValue <= 6)
        {
            PlayDiceRollAnimation(cheatValue);
            return;
        }
        int diceRollResult = CalculateDiceRollResult();
        PlayDiceRollAnimation(diceRollResult);
    }

    private int CalculateDiceRollResult()
    {
        int diceRollResult = Random.Range(1, 7);
        return diceRollResult;
    }

    private void PlayDiceRollAnimation(int diceRollResult)
    {
        transform.DORotate(new Vector3(360*Random.Range(5,10), 360 * Random.Range(5, 10), 360 * Random.Range(5, 10)), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                transform.eulerAngles = diceFacesOrientations[diceRollResult];
                Debug.Log($"Dice landed on: {diceRollResult}");
                if (diceRollResult == 6)
                {
                    transform.DOShakeScale(0.4f, 1f, 10, 90, false).OnComplete(() => {
                        UpdateScoreText(diceRollResult);
                    }).Play();
                }
                else 
                    UpdateScoreText(diceRollResult);
             }).Play();
    }

    private void UpdateScoreText(int diceRollResult)
    {
        if (diceRollResult != 6) return;

        scoreNumberText.transform.DOPunchScale(new Vector3(1f, 1f, 0), 0.4f, 10, 90).Play();
        currentScore += 1;
        scoreNumberText.text = currentScore.ToString();
    }
}
