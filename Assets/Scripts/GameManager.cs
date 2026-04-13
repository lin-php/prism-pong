using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallController ballController;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private GameObject ball1Prefab;
    [SerializeField] private GameObject ball2Prefab;
    [SerializeField] private float ball2Spawntimer = 5f;

    private int playerScore;
    private int aiScore;
    private float timerball2;
    private bool ball2isSpawned = false;

    // add points if goaled

    private void Start()
    {
        timerball2 = 0f;
        UpdateScoreUI();
    }

    private void Update()
    {
        timerball2 += Time.deltaTime;

        if (!ball2isSpawned && timerball2 >= ball2Spawntimer)
        { 
            GameObject ball2 = Instantiate(ball2Prefab);
            BallController ball2Controller = ball2.GetComponent<BallController>();
            ball2Controller.SpawnBall();

            ball2isSpawned = true;
        }
    }

    public void AddPlayerPoint()
    {
        playerScore++;
        UpdateScoreUI();
        ballController.SpawnBall();
        timerball2 = 0f;
        ball2isSpawned = false;
    }

    public void AddAiPoint()
    {
        aiScore++;
        UpdateScoreUI();
        ballController.SpawnBall();
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
        playerScore = 0;
        aiScore = 0;
        UpdateScoreUI();
        ballController.SpawnBall();
    }

}
