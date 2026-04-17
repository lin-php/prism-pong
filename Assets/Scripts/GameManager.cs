using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI streakText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI TierUI;
    [SerializeField] private GameObject ball1Prefab;
    [SerializeField] private GameObject ball2Prefab;
    [SerializeField] private float ball1Spawntimer = 4f;
    [SerializeField] private GameObject AiPaddle;
    [SerializeField] private int maxBallsAreSpawned = 8;
    [SerializeField] private int minBalls = 2;
    [SerializeField] private int extraPoints = 100;

    private int score;
    private int streak = 0;
    private int multiplier = 1;
    private float timerball;
    private int Tier = 1;
    
    private AIPaddleController aIPaddleController;

    private List<GameObject> activeBalls = new List<GameObject>();  

    // add points if goaled
    private void Start()
    {
        aIPaddleController = AiPaddle.GetComponent<AIPaddleController>();

        timerball = 0f;
        UpdateScoreUI();
        Ball1Instantiate();
        aIPaddleController.SetBalls(activeBalls);
    }

    private void Update()
    {
        timerball += Time.deltaTime;

        // checks how many balls are ingame, will spawn more balls if place is available

        if (timerball >= ball1Spawntimer && activeBalls.Count < maxBallsAreSpawned)
        { 
            GameObject ball1 = Instantiate(ball1Prefab);
            BallController ball2Controller = ball1.GetComponent<BallController>();
            ball2Controller.SpawnBall();
            activeBalls.Add(ball1);

            aIPaddleController.SetBalls(activeBalls);
            timerball = 0f;
        }

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
        activeBalls.Remove(scoringBall);
        Destroy(scoringBall);
    }

    public void AddPlayerPointonPaddlehit()
    {
        streak++;
        multiplier = streak;
        score += multiplier;

        if (streak % 20 == 0)
        {
            score += (extraPoints + 100);

            ClearBalls();
            timerball = 0f;
            Ball1Instantiate();
            Debug.Log("MEGA HIT!!!");
            Tier++;
        }
        else if (streak % 15 == 0)
        {
            score += (extraPoints + 50);
        }
        else if (streak % 10 == 0)
        {
            score += extraPoints;
        }
        else if (streak % 5 == 0)
        {
            score += (extraPoints / 2);
        }

        UpdateScoreUI();
    }

    public void AiGoalHit(GameObject scoringBall)
    {
        streak = 0;
        multiplier = 1;
        ReduceBalls(0.3f);
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
        
    }
   
    // reset score and ball position
    public void RestartGame()
    {
        ClearBalls();
        score = 0;
        UpdateScoreUI();
        Ball1Instantiate();
    }

    private void ClearBalls()
    {
        foreach (GameObject ball in activeBalls) 
        {
            Destroy(ball);
        }

        activeBalls.Clear();
    }

    // Reduces the number of active balls by a given percentage,
    // while ensuring that at least a minimum number of balls remain in the game.
    private void ReduceBalls(float percentage)
    {
        int ballsToRemove = Mathf.FloorToInt(activeBalls.Count * percentage);

        int maxRemovable = activeBalls.Count - minBalls;

        ballsToRemove = Mathf.Min(ballsToRemove, maxRemovable);

        for (int i = 0; i < ballsToRemove; i++)
        {
            if (activeBalls.Count == 0) return;

            GameObject removeBall = activeBalls[activeBalls.Count - 1];

            activeBalls.Remove(removeBall);
            Destroy(removeBall);
        }
    }

}
