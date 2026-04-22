using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI streakText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI TierUI;
    [SerializeField] private TextMeshProUGUI GameOverScoretext;
    [SerializeField] private TextMeshProUGUI highScorePauseText;
    [SerializeField] private TextMeshProUGUI highScoreGameOverText;
    [SerializeField] private TextMeshProUGUI newHighScoreTextGameOver;
    [SerializeField] private TextMeshProUGUI FeedbackText;
    [SerializeField] private TextMeshProUGUI FeedbackTextCombo;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Slider HealthUI;
    [SerializeField] private GameObject ball1Prefab;
    [SerializeField] private float ball1Spawntimer = 7f;
    [SerializeField] private GameObject AiPaddle;
    [SerializeField] private int baseMaxBalls = 6;
    [SerializeField] private int minBalls = 2;
    [SerializeField] private int extraPoints = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int dmgPerMiss = 25;
    [SerializeField] private float baseRecoveryRate = 5f;
    [SerializeField] private float extraRecoveryPerTier = 0.5f;
    [SerializeField] private float speedTimer = 30f;
    [SerializeField] private float increaseBallSpeed = 0.10f;
    [SerializeField] private AudioClip EventSoundStreak;

    
    private string highScoreKey = "HighScore";
    
    private int highScore;
    private int score;
    private int streak = 0;
    private int multiplier = 1;
    private int extraBallsPerTier = 1;
    private float timerball;
    private int Tier = 1;
    private float currentHealth;
    private bool isGameOver = false;
    private float _speedTimer;
    private float currentSpeedBonus = 0;
    private Coroutine feedbackcoroutine;
    private Coroutine feedbackcombocoroutine;
    
    private AIPaddleController aIPaddleController;

    private List<GameObject> activeBalls = new List<GameObject>();  
    private List<GameObject> deletingEvent = new List<GameObject>();

    // add points if goaled
    private void Start()
    {
        currentHealth = 100;
        SliderDanger();
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        UpdateHighScoreUI();

        timerball = 0f;
        UpdateScoreUI();
        Ball1Instantiate();
        aIPaddleController.SetBalls(activeBalls);
    }
    private void Awake()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        AudioListener.volume = volume;
        aIPaddleController = AiPaddle.GetComponent<AIPaddleController>();
    }

    private void Update()
    {
        if (isGameOver) return;

        timerball += Time.deltaTime;

        // checks how many balls are ingame, will spawn more balls if place is available

        if (timerball >= ball1Spawntimer && activeBalls.Count < MaxBalls())
        { 
            GameObject ball1 = Instantiate(ball1Prefab);
            BallController ball1Controller = ball1.GetComponent<BallController>();
            ball1Controller.IncreaseBallSpeed(currentSpeedBonus);
            ball1Controller.SpawnBall();
            activeBalls.Add(ball1);

            aIPaddleController.SetBalls(activeBalls);
            timerball = 0f;
        }

        // recovery rate
        currentHealth += RecoveryRate() * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        SliderDanger();

        // increase Ball Speed after increase play time; escalation system; increase spawntimer as well
        _speedTimer += Time.deltaTime;

        if(_speedTimer >= speedTimer)
        {
            foreach (GameObject ball in activeBalls) 
            {
                BallController ballController = ball.GetComponent<BallController>();
                ballController.IncreaseBallSpeed(increaseBallSpeed);
            }

            currentSpeedBonus += increaseBallSpeed;

            if (ball1Spawntimer > 1f)
            {
                ball1Spawntimer -= 0.2f;
            }

            _speedTimer = 0f;
        }
    }

    // player gets damage
    private void AddDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        SliderDanger();
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void SliderDanger()
    {
        HealthUI.value = currentHealth;
    }

    // Game Over
    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);

        if(score > highScore)
        {
            highScore = score;  
            PlayerPrefs.SetInt(highScoreKey, score);
            newHighScoreTextGameOver.gameObject.SetActive(true);
        }
        else
        {
            newHighScoreTextGameOver.gameObject.SetActive(false);
        }
            UpdateHighScoreUI();
    }

    private void Ball1Instantiate()
    {
        GameObject ball1 = Instantiate(ball1Prefab);    
        BallController ball1Controller = ball1.GetComponent<BallController>();  
        ball1Controller.SpawnBall();
        activeBalls.Add(ball1); 
    }

    public void AddPlayerPointonGoal(GameObject scoringBall)
    {
        score += multiplier;
        streak++;
        multiplier = streak;
        UpdateScoreUI();
        Milestone();
        activeBalls.Remove(scoringBall);
        Destroy(scoringBall);
        ShowFeedback("GOAL!");
    }

    public void AddPlayerPointonPaddlehit()
    {
        streak++;
        multiplier = streak;
        score += multiplier;
        UpdateScoreUI();
        Milestone();
    }

    // Event System
    private void Milestone()
    {
        if (streak % 20 == 0)
        {
            score += (extraPoints + 100);

            timerball = 0f;
            ReduceBalls(1f);
   
            ShowFeedbackCombo("+" + streak + " PERFECT!");
            ShowFeedback("PERFECT! CENTER CLEAR!");
            
            Tier++;
            AudioController.Instance.SoundOnHit(EventSoundStreak, 1f);
        }
        else if (streak % 15 == 0)
        {
            score += (extraPoints + 50);
            ShowFeedbackCombo("+" + streak + " AWESOME!");
        }
        else if (streak % 10 == 0)
        {
            score += extraPoints;
            ShowFeedbackCombo("+" + streak + " GREAT!");
        }
        else if (streak % 5 == 0)
        {
            score += (extraPoints / 2);
            ShowFeedbackCombo("+" + streak + " GOOD!");
        }
        else
        {
            ShowFeedbackCombo("+" + streak);
        }
        UpdateScoreUI();
    }

    public void AiGoalHit(GameObject scoringBall)
    {
        streak = 0;
        multiplier = 1;

        // player gets damage
        AddDamage(dmgPerMiss);

        activeBalls.Remove(scoringBall);
        Destroy(scoringBall);
        UpdateScoreUI();
    }

    // UI Score update
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        streakText.text = "Streak: " + streak.ToString();  
        TierUI.text = "Tier: " + Tier.ToString();
        GameOverScoretext.text = "Score: " + score.ToString();
    }

    private void UpdateHighScoreUI()
    {
        highScorePauseText.text = "High Score: " + highScore.ToString();
        highScoreGameOverText.text = "High Score: " + highScore.ToString();
    }
   
    // Feedback Text method and coroutine for show/hide feedback text
    private void ShowFeedback(string feedback)
    {
        FeedbackText.text = feedback;

        if(feedbackcoroutine != null) 
        {
            StopCoroutine(feedbackcoroutine);  
        }

        feedbackcoroutine = StartCoroutine(FeedbackRoutine());
    }
    private IEnumerator FeedbackRoutine()
    {
        FeedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        FeedbackText.gameObject.SetActive(false);
        feedbackcoroutine = null;
    }

    private void ShowFeedbackCombo(string feedback)
    {
        FeedbackTextCombo.text = feedback;

        if(feedbackcombocoroutine != null)
        {
            StopCoroutine(feedbackcombocoroutine);
        }
        feedbackcombocoroutine = StartCoroutine(FeedbackComboRoutine());
    }

    private IEnumerator FeedbackComboRoutine()
    {
        FeedbackTextCombo.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        FeedbackTextCombo.gameObject.SetActive(false);
        feedbackcombocoroutine = null;
    }

    // reset score and ball position
    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        currentHealth = 100f;
        GameOverPanel.SetActive(false);
        SliderDanger();
        ClearBalls();
        score = 0;
        multiplier = 1;
        streak = 0;
        Tier = 1;
        UpdateScoreUI();
        Ball1Instantiate();
        timerball = 0f;
        UpdateHighScoreUI();
        newHighScoreTextGameOver.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ClearBalls()
    {
        foreach (GameObject ball in activeBalls) 
        {
            Destroy(ball);
        }

        activeBalls.Clear();
    }

    private int MaxBalls()
    {
        return baseMaxBalls + (extraBallsPerTier * (Tier - 1));
    }

    private float RecoveryRate()
    {
        return baseRecoveryRate + (extraRecoveryPerTier *(Tier - 1));
    }

    
    // Reduces the number of active balls by a given percentage,
    // destroy EVENT; each destroyed ball give +10 score
    private void ReduceBalls(float percentage)
    {
        int destroyedBalls = 0;
        DeletingZone();

        int ballsToRemove = Mathf.FloorToInt(deletingEvent.Count * percentage);
        int maxRemovable = activeBalls.Count - minBalls;
        ballsToRemove = Mathf.Min(ballsToRemove, maxRemovable);

        for (int i = 0; i < ballsToRemove; i++)
        {
            if (deletingEvent.Count == 0) break;

            GameObject removeBall = deletingEvent[deletingEvent.Count - 1];
            
            deletingEvent.Remove(removeBall);
            activeBalls.Remove(removeBall);
            Destroy(removeBall);
            destroyedBalls++;
        }
        score += destroyedBalls * 10;
        UpdateScoreUI();    
    }

    private void DeletingZone()
    {
        deletingEvent.Clear();

        foreach (GameObject ball in activeBalls)
        {
            Vector2 position = ball.transform.position;

            if(position.x > -3f && position.x < 3f)
            {
                deletingEvent.Add(ball);
            }
        }
    }
}
