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
    [SerializeField] private GameObject GameOverNormalGroup;
    [SerializeField] private TextMeshProUGUI GameOverNormalScoretext;
    [SerializeField] private TextMeshProUGUI GameOverNormalStreaktext;
    [SerializeField] private TextMeshProUGUI GameOverNormalTiertext;
    [Space(15)]
    [SerializeField] private TextMeshProUGUI highScorePauseText;
    [SerializeField] private TextMeshProUGUI highScoreGameOverText;
    [Space(15)]
    [SerializeField] private GameObject GameOverNewHighScoreGroup;
    [SerializeField] private TextMeshProUGUI GameOverNewScoretext;
    [SerializeField] private TextMeshProUGUI GameOverNewStreaktext;
    [SerializeField] private TextMeshProUGUI GameOverNewTiertext;
    [Space(15)]
    [SerializeField] private TextMeshProUGUI FeedbackText;
    [SerializeField] private TextMeshProUGUI FeedbackTextCombo;
    [SerializeField] private TextMeshProUGUI FeedbackTextComboDamage;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Slider HealthUI;
    [SerializeField] private GameObject ball1Prefab;
    [Space(15)]
    [SerializeField] private float ball1Spawntimer = 7f;
    [SerializeField] private GameObject AiPaddle;
    [Space(10)]
    [SerializeField] private int startBalls = 2;
    [SerializeField] private int minBalls = 2;
    [SerializeField] private int maxBalls = 6;
    [Space(10)]
    [SerializeField] private int extraPoints = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int dmgPerMiss = 25;
    [Space(10)]
    [SerializeField] private float baseRecoveryRate = 5f;
    [SerializeField] private float extraRecoveryPerTier = 0.5f;
    [Space(10)]
    [SerializeField] private float speedTimer = 30f;
    [SerializeField] private float increaseBallSpeed = 0.12f;
    [SerializeField] private float maxSpeedBonus = 4.5f;
    [SerializeField] private float protectionDuration = 2f;
    [SerializeField] private AudioClip EventSoundStreak;
    [Space(25)]
    [SerializeField] private Material[] ballcolours;

    
    private string highScoreKey = "HighScore";
    
    private int highScore;
    private int score;
    private int streak = 0;
    private int extraBallsPerTier = 1;
    private float timerball;
    private int Tier = 1;
    private float currentHealth;
    private bool isGameOver = false;
    private float _speedTimer;
    private float currentSpeedBonus = 0;
    private Coroutine feedbackcoroutine;
    private Coroutine feedbackcombocoroutine;
    private Coroutine feedbackdamagecoroutine;
    private bool isProtected = false;
    private int nextTierMilestone = 20;
    private float safeBallSpawnTimer;
    private AIPaddleController aIPaddleController;

    private List<GameObject> activeBalls = new List<GameObject>();  
    private List<GameObject> deletingEvent = new List<GameObject>();
       
    private void Start()
    {
        safeBallSpawnTimer = ball1Spawntimer;

        currentHealth = maxHealth;
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
            Ball1Instantiate();
            timerball = 0f;
        }

        // recovery rate
        currentHealth += RecoveryRate() * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        SliderDanger();

        // increase Ball Speed after increase play time; escalation system; decrease spawntimer as well
        _speedTimer += Time.deltaTime;

        if(_speedTimer >= speedTimer)
        {
            if (currentSpeedBonus < maxSpeedBonus)
            {
                foreach (GameObject ball in activeBalls)
                {
                    BallController ballController = ball.GetComponent<BallController>();
                    ballController.IncreaseBallSpeed(increaseBallSpeed);
                }

                currentSpeedBonus += increaseBallSpeed;
            }
         
            if (ball1Spawntimer > 1.3f)
            {
                ball1Spawntimer -= 0.3f;
            }

            _speedTimer = 0f;
        }
    }

    // player gets damage
    private void AddDamage(float amount)
    {
        if (!isProtected)
        {
            streak -= 5;
            streak = Mathf.Max(0, streak);    
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            SliderDanger();
            ShowFeedbackDamage("Streak -5");
            if (currentHealth <= 0)
            {
                GameOver();
            }
            isProtected = true;
            StartCoroutine(ProtectionCoroutine()); 
        }
    }

    private IEnumerator ProtectionCoroutine()
    {
        yield return new WaitForSeconds(protectionDuration);
        isProtected = false;
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
        GameOverNormalGroup.SetActive(false);
        GameOverNewHighScoreGroup.SetActive(false);

        if (score > highScore)
        {
            highScore = score;  
            PlayerPrefs.SetInt(highScoreKey, score);
            GameOverNewHighScoreGroup.gameObject.SetActive(true);
        }
        else
        {
            GameOverNormalGroup.gameObject.SetActive(true);
        }
            UpdateHighScoreUI();
    }

    private void Ball1Instantiate()
    {
        GameObject ball1 = Instantiate(ball1Prefab);
        BallController ball1Controller = ball1.GetComponent<BallController>();
        ball1Controller.IncreaseBallSpeed(currentSpeedBonus);
        ball1Controller.SpawnBall();
        activeBalls.Add(ball1);

        aIPaddleController.SetBalls(activeBalls);

    }

    public void AddPlayerPointonGoal(GameObject scoringBall)
    {
        streak++;
        score += streak;
        UpdateScoreUI();
        Milestone();
        activeBalls.Remove(scoringBall);
        Destroy(scoringBall);
        ShowFeedback("GOAL!");
    }

    public void AddPlayerPointonPaddlehit()
    {
        streak++;
        score += streak;
        UpdateScoreUI();
        Milestone();
    }

    // Event System
    private void Milestone()
    {
        if (streak >= nextTierMilestone)
        {
            score += (extraPoints + 100);

            timerball = 0f;
            ReduceBalls(1f);
   
            ShowFeedbackCombo("+" + streak + " PERFECT!");
            ShowFeedback("PERFECT! CENTER CLEAR!");
            
            Tier++;
            nextTierMilestone += 20;
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
        if (!isProtected)
        {
            // player gets damage
            AddDamage(dmgPerMiss);
        }

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
        GameOverNormalScoretext.text = "Score: " + score.ToString();
        GameOverNormalTiertext.text = "Tier: " + Tier.ToString();
        GameOverNormalStreaktext.text = "Streak: " + streak.ToString();
        GameOverNewScoretext.text = "Score: " + score.ToString();
        GameOverNewTiertext.text = "Tier: " + Tier.ToString();
        GameOverNewStreaktext.text = "Streak: " + streak.ToString();
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
       
        if (feedbackcombocoroutine != null)
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

    private void ShowFeedbackDamage(string feedback)
    {
        FeedbackTextComboDamage.text = feedback;

        if (feedbackdamagecoroutine != null)
        {
            StopCoroutine(feedbackdamagecoroutine);
        }
        feedbackdamagecoroutine = StartCoroutine(FeedbackDamageRoutine());
    }

    private IEnumerator FeedbackDamageRoutine()
    {
        FeedbackTextComboDamage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        FeedbackTextComboDamage.gameObject.SetActive (false);
        feedbackdamagecoroutine = null;
    }


    // reset score and ball position
    public void RestartGame()
    {
        isGameOver = false;
        isProtected = false;
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        nextTierMilestone = 20;
        currentSpeedBonus = 0;
        _speedTimer = 0f;
        ball1Spawntimer = safeBallSpawnTimer;
        GameOverPanel.SetActive(false);
        SliderDanger();
        ClearBalls();
        score = 0;
        streak = 0;
        Tier = 1;
        UpdateScoreUI();
        Ball1Instantiate();
        timerball = 0f;
        UpdateHighScoreUI();
        GameOverNewHighScoreGroup.gameObject.SetActive(false);
        GameOverNormalGroup.gameObject.SetActive(false);

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
        int balls = startBalls + (extraBallsPerTier * (Tier - 1)); 
        return Mathf.Min(balls, maxBalls);
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
