using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;

    [SerializeField] private GameObject ball;

    // simple AI movement

    void Update()
    {
        float pos = transform.position.y;

        float ballY = ball.transform.position.y;

        pos = Mathf.MoveTowards(pos, ballY, speed * Time.deltaTime);

        float clampedY = Mathf.Clamp(pos, -4.5f, 4.5f);

        transform.position = new Vector2 (transform.position.x, clampedY);    

    }
}
