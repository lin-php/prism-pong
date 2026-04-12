using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallController ballController;

    private int playerScore;
    private int aiScore;


    // add points if goaled

    public void AddPlayerPoint()
    {
        playerScore++;
        ballController.SpawnBall();
    }

    public void AddAiPoint()
    {
        aiScore++;
        ballController.SpawnBall();
    }
}
