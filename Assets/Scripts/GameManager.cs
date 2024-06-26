using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private TMP_Text gameOverTxt;
    [SerializeField] private TMP_Text startTxt;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject resumeButton;

    private int timeCount = 99;
    private float timer;

    private bool isGameOver;
    private bool isStarting;

    private PlayerScript[] players;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
        timerTxt.text = Convert.ToString(timeCount); 
        players = FindObjectsByType<PlayerScript>(FindObjectsSortMode.None);
        gameOverTxt.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isStarting)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        
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
            GameEndScreen();
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
            GameEndScreen();
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
                
                GameEndScreen();
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
                
                GameEndScreen();
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

    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Rematch()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void GameEndScreen()
    {
        menu.SetActive(true);
        resumeButton.SetActive(false);
        Time.timeScale = 0;
    }
    
    IEnumerator StartGame()
    {
        isStarting = true;
        Time.timeScale = 0;

        yield return StartCoroutine(WaitForRealSeconds(1));
        startTxt.text = "Ready?";
        yield return StartCoroutine(WaitForRealSeconds(1));
        startTxt.text = "3";
        yield return StartCoroutine(WaitForRealSeconds(1));
        startTxt.text = "2";
        yield return StartCoroutine(WaitForRealSeconds(1));
        startTxt.text = "1";
        yield return StartCoroutine(WaitForRealSeconds(1));
        startTxt.text = "Go!";

        Time.timeScale = 1;
        isStarting = false;
        
        yield return new WaitForSeconds(1);
        startTxt.text = " ";
    }
    
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
