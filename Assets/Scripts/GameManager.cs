using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallController ballController;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;

    private int playerScore;
    private int aiScore;


    // add points if goaled

    public void AddPlayerPoint()
    {
        playerScore++;
        UpdateScoreUI();
        ballController.SpawnBall();
    }

    public void AddAiPoint()
    {
        aiScore++;
        UpdateScoreUI();
        ballController.SpawnBall();
    }

    // UI Score update
    private void UpdateScoreUI()
    {
        playerScoreText.text = playerScore.ToString();
        aiScoreText.text = aiScore.ToString();
    }
    private void Start()
    {
        UpdateScoreUI();
    }
}
