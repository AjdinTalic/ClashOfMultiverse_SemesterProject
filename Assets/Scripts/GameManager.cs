using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private TMP_Text gameOverTxt;

    private int timeCount = 99;
    private float timer;

    private bool isGameOver;

    private PlayerScript[] players;
    
    // Start is called before the first frame update
    void Start()
    {
        timerTxt.text = Convert.ToString(timeCount); 
        players = FindObjectsByType<PlayerScript>(FindObjectsSortMode.None);
        gameOverTxt.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1 && timeCount > 0 && !isGameOver)
        {
            timeCount--;
            timerTxt.text = Convert.ToString(timeCount);

            timer = 0;
        }

        if (players[1].currentVitality <= 0)
        {
            if (players[0].name == "TestPlayer")
            {
                gameOverTxt.text = "P1 Wins";
            }
            else if (players[0].name == "TestPlayer2")
            {
                gameOverTxt.text = "P2 Wins";
            }

            isGameOver = true;
        }
        else if (players[0].currentVitality <= 0)
        {
            if (players[1].name == "TestPlayer")
            {
                gameOverTxt.text = "P1 Wins";
            }
            else if (players[1].name == "TestPlayer2")
            {
                gameOverTxt.text = "P2 Wins";
            }

            isGameOver = true;
        }

        if (timeCount <= 0)
        {
            isGameOver = true;

            if (players[0].currentVitality > players[1].currentVitality)
            {
                if (players[0].name == "TestPlayer")
                {
                    gameOverTxt.text = "P1 Wins";
                }
                else if (players[0].name == "TestPlayer2")
                {
                    gameOverTxt.text = "P2 Wins";
                }
            }
            else if (players[1].currentVitality > players[0].currentVitality)
            {
                if (players[1].name == "TestPlayer")
                {
                    gameOverTxt.text = "P1 Wins";
                }
                else if (players[1].name == "TestPlayer2")
                {
                    gameOverTxt.text = "P2 Wins";
                }
            }
            else if (players[0].currentVitality == players[1].currentVitality)
            {
                gameOverTxt.text = "Next Hit Wins!";

                for (int i = 0; i < players.Length; i++)
                {
                    players[i].currentVitality = 1;
                    players[i].vitalityBar.fillAmount = players[i].currentVitality / players[i].maxVitality;
                }
            }
        }
    }
}
