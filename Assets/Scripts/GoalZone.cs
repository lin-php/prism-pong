using UnityEngine;

public class GoalZone : MonoBehaviour
{

    [SerializeField] private bool isLeftGoal;
    [SerializeField] private GameManager gameManager;


    // goal system with trigger

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            if (isLeftGoal)
            {
                Debug.Log("AI bekommt Punkt");
                gameManager.AddAiPoint();
            }
            else
            {
                Debug.Log("Player bekommt Punkt");
                gameManager.AddPlayerPoint();  
            }
        }
    }

}
