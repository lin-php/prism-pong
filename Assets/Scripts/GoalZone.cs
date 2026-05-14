using UnityEngine;

public class GoalZone : MonoBehaviour
{

    [SerializeField] private bool isLeftGoal;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioClip goalPlayerZone;
    [SerializeField] private AudioClip goalAiZone;
    [SerializeField] private GameObject goalBurstPrefab;
    [SerializeField] private Vector3 goalBurstRotation;

    // goal system with trigger

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {

            BallController ballcontroller = collider.GetComponent<BallController>();
            GameObject goalBurst = Instantiate(goalBurstPrefab, collider.transform.position, Quaternion.Euler(goalBurstRotation));
            ParticleDeath particleEffect = goalBurst.GetComponent<ParticleDeath>();
            particleEffect.SetColor(ballcontroller.ParticleColor, ballcontroller.ParticleEmissionColor);

            if (isLeftGoal)
            {
                gameManager.AiGoalHit(collider.gameObject);
                AudioController.Instance.SoundOnHit(goalPlayerZone, 0.9f);
                CameraShake.Instance.Shake(0.15f, 0.15f);
            }
            else
            {
                gameManager.AddPlayerPointonGoal(collider.gameObject); 
                AudioController.Instance.SoundOnHit(goalAiZone, 1f);
                CameraShake.Instance.Shake(0.15f, 0.15f);
            }
        }
    }

}
