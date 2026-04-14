using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
   
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private GameObject ball1Prefab;
    [SerializeField] private GameObject ball2Prefab;
    [SerializeField] private float ball2Spawntimer = 5f;
    [SerializeField] private GameObject AiPaddle;

    private int playerScore;
    private int aiScore;
    private float timerball2;
    private bool ball2isSpawned = false;
    private AIPaddleController aIPaddleController;

    private List<GameObject> activeBalls = new List<GameObject>();  

    // add points if goaled
    private void Start()
    {
        aIPaddleController = AiPaddle.GetComponent<AIPaddleController>();

        timerball2 = 0f;
        UpdateScoreUI();
        Ball1Instantiate();
    }

    private void Update()
    {
        timerball2 += Time.deltaTime;

        if (!ball2isSpawned && timerball2 >= ball2Spawntimer)
        { 
            GameObject ball2 = Instantiate(ball2Prefab);
            BallController ball2Controller = ball2.GetComponent<BallController>();
            ball2Controller.SpawnBall();
            activeBalls.Add(ball2);

            aIPaddleController.SetBalls(activeBalls);

            ball2isSpawned = true;
        }
    }

    private void Ball1Instantiate()
    {
        GameObject ball1 = Instantiate(ball1Prefab);    
        BallController ball1Controller = ball1.GetComponent<BallController>();  
        ball1Controller.SpawnBall();
        activeBalls.Add(ball1); 
    }

    public void AddPlayerPoint()
    {
        playerScore++;
        UpdateScoreUI();
        ClearBalls();
        Ball1Instantiate();
        timerball2 = 0f;
        ball2isSpawned = false;
    }

    public void AddAiPoint()
    {
        aiScore++;
        UpdateScoreUI();
        ClearBalls();
        Ball1Instantiate();
        timerball2 = 0f;
        ball2isSpawned = false;
    }

    // UI Score update
    private void UpdateScoreUI()
    {
        playerScoreText.text = playerScore.ToString();
        aiScoreText.text = aiScore.ToString();
    }
   
    // reset score and ball position
    public void RestartGame()
    {
        ClearBalls();
        playerScore = 0;
        aiScore = 0;
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

}
