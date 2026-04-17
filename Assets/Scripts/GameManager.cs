using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
   
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject ball1Prefab;
    [SerializeField] private GameObject ball2Prefab;
    [SerializeField] private float ball1Spawntimer = 3f;
    [SerializeField] private GameObject AiPaddle;
    [SerializeField] private int maxBallsareSpawned = 10;
    [SerializeField] private int multiplier = 1;
    [SerializeField] private int minBalls = 2;

    private int score;
    
    private float timerball;
    
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

        if (timerball >= ball1Spawntimer && activeBalls.Count < maxBallsareSpawned)
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
        score++;
        UpdateScoreUI();
        activeBalls.Remove(scoringBall);
        Destroy(scoringBall);
    }

    public void AddPlayerPointonPaddlehit()
    {
        score += multiplier;

        if (multiplier < 10)
        {
            multiplier++;
        }
        
        UpdateScoreUI();
    }

    public void AiGoalHit(GameObject scoringBall)
    {
        multiplier = 1;
        ReduceBalls(0.3f);
        activeBalls.Remove(scoringBall);
        Destroy(scoringBall);
        timerball = 0f;
    }

    // UI Score update
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        // aiScoreText.text = aiScore.ToString();
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
