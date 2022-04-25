using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject gamePanel, gameOverPanel;

    [SerializeField] private TextMeshProUGUI scoreText, goScoreText, goStatusText, timerText, healthText;
    [SerializeField] private Button restartBtn;

    GameManager gameManager;
    int score;
    float TimeLeft;
    bool TimerOn = false;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.gameManager.updateScore += UpdateScore;
        this.gameManager.updateHealth += UpdateHealth;
        this.gameManager.gameStatus += UpdateGameStatus;
    }

    private void OnDisable()
    {
        this.gameManager.updateScore -= UpdateScore;
        this.gameManager.updateHealth -= UpdateHealth;
        this.gameManager.gameStatus -= UpdateGameStatus;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUI();
        DontDestroyOnLoad(gameObject);
        restartBtn.onClick.AddListener(RestartGame);
        TimerOn = true;
    }

    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;
                gameManager.SetGameStatus(false);
            }
        }
    }
    void UpdateScore()
    {
        score += 10;
        scoreText.text = "SCORE: " + score;
    }

    void UpdateHealth(int health)
    {
        healthText.text = "Health: " + health;
    }

    void UpdateGameStatus(bool gameWon)
    {
        gamePanel.SetActive(false);
        goScoreText.text = "Score: " + score;
        goStatusText.text = gameWon == true ? "Won the game!!" : "Lost the game!!";
        gameOverPanel.SetActive(true);
    }

    void RestartGame()
    {
        SetUI();
        gameManager.RestartGame();
    }

    void SetUI()
    {
        score = 000;
        TimeLeft = 200;
        gameManager.playerController.health = 30;
        scoreText.text = "Score: " + score;
        timerText.text = "Time left: " + TimeLeft;
        healthText.text = "Health: " + gameManager.playerController.health;
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime);

        timerText.text = "Time left: " + seconds.ToString();
    }
}
