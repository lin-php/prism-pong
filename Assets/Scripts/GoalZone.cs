using UnityEngine;

public class GoalZone : MonoBehaviour
{

    [SerializeField] private bool isLeftGoal;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioClip goalPlayerZone;
    [SerializeField] private AudioClip goalAiZone;


    // goal system with trigger

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            if (isLeftGoal)
            {
                gameManager.AiGoalHit(collider.gameObject);
                AudioController.Instance.SoundOnHit(goalPlayerZone);
            }
            else
            {
                gameManager.AddPlayerPointonGoal(collider.gameObject); 
                AudioController.Instance.SoundOnHit(goalAiZone);
            }
        }
    }

}
