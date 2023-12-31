using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public HighScores m_HighScores;

    // Reference to the overlay Text to display winning text, etc
    public TMP_Text m_MessageText;
    public TMP_Text m_TimerText;
    public TMP_Text m_TimeToBeat;

    public GameObject[] m_Tanks;

    public GameObject m_HighScorePanel;
    public TMP_Text m_HighScoresText;
    public Button m_NewGameButton;
    public Button m_HighScoresButton;
    public Slider m_Health;


    private float m_gameTime = 0;
    public float GameTime { get { return m_gameTime; } }
    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };
    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }
    private void Awake()
    {
        m_GameState = GameState.Start;
    }
    private void Start()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }
        m_TimerText.gameObject.SetActive(false);
        m_Health.gameObject.SetActive(false);

        m_MessageText.text = "Get Ready";

        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);

    }
    void Update()
    {
        switch (m_GameState)
        {
            case GameState.Start:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_TimerText.gameObject.SetActive(true);
                    m_Health.gameObject.SetActive(true);
                    m_MessageText.text = "";
                    m_GameState = GameState.Playing;
                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }

                }
                break;
            case GameState.Playing:
                bool isGameOver = false;
                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}",
                         (seconds / 60), (seconds % 60));

                if (OneTankLeft() == true)
                {
                    isGameOver = true;
                }
                else if (IsPlayerDead() == true)
                {
                    isGameOver = true;
                }
                if (isGameOver == true)
                {
                    m_GameState = GameState.GameOver;
                    m_TimerText.gameObject.SetActive(false);
                    m_Health.gameObject.SetActive(false);

                    m_NewGameButton.gameObject.SetActive(true);
                    m_HighScoresButton.gameObject.SetActive(true);

                    if (IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN";
                    }
                    else
                    {
                        m_MessageText.text = "WINNER!";
                        // save the score
                        m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime));
                        m_HighScores.SaveScoresToFile();

                    }

                }
                break;
            case GameState.GameOver:
                /*if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_gameTime = 0;
                    m_GameState = GameState.Playing;
                    m_MessageText.text = "";
                    m_TimerText.gameObject.SetActive(true);
                    m_Health.gameObject.SetActive(true);

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                    ActivateEnemyRestart();
                }*/
                break;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private bool OneTankLeft()
    {
        int numTanksLeft = 0;
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }
        return numTanksLeft <= 1;
    }
    private bool IsPlayerDead()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == false)
            {
                if (m_Tanks[i].tag == "Player")
                    return true;
            }
        }
        return false;
    }
    public void ActivateEnemyRestart()
    {
        // Find all GameObjects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Loop through all the enemies and activate the GameRestart function
        foreach (GameObject enemy in enemies)
        {
            // Get the component with the TankHealth script and activate the GameRestart function
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.GameRestart();
            }
        }
    }
    public void OnNewGame()
    {
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(false);
        m_gameTime = 0;
        m_GameState = GameState.Playing;
        m_TimerText.gameObject.SetActive(true);
        m_Health.gameObject.SetActive(true);
        m_MessageText.text = "";
        ActivateEnemyRestart();
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(true);
        }
    }
    public void OnHighScores()
    {
        m_MessageText.text = "";
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);
        string text = "";
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n",
           (seconds / 60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }
}