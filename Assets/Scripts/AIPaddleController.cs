
using UnityEngine;
using System.Collections.Generic;


public class AIPaddleController : MonoBehaviour
{

    [SerializeField] private float speed = 6f;
    [SerializeField] private float offsetRange;
    [SerializeField] private float offsetChangeInterval = 5f;
    [SerializeField] private float deadzone = 0.2f;
    [SerializeField] private float offsetSmoothSpeed = 1f;

    private List<GameObject> balls;

    private float currentOffset;
    private float targetOffset;

    private GameObject currentBall;

    private float timer;

    private void Start()
    {
        RandomOffset();
        currentOffset = targetOffset;
        timer = offsetChangeInterval;
    }

    public void SetBalls(List<GameObject> activeBalls)
    {
        balls = activeBalls;
    }

    private void RandomOffset()
    {
        // new mistake in random range
        targetOffset = Random.Range(-offsetRange, offsetRange);
    }

    void Update()
    {
        // smooth movement
        currentOffset = Mathf.MoveTowards(currentOffset, targetOffset, offsetSmoothSpeed * Time.deltaTime); 

        // timer for new offset

        timer -= Time.deltaTime;

        if (timer <= 0) 
        {
            RandomOffset();
            timer = offsetChangeInterval;
        }

        // take current position and ball height

        float pos = transform.position.y;

        if (balls == null || balls.Count == 0)
        {
            return;

        }
 
        currentBall = balls[0];
        
        float ballY = currentBall.transform.position.y;

        // movement
        float difference = (ballY + currentOffset) - pos;
        float direction = 0f;

        if (difference > deadzone)
        {
             direction = 1f;
        }
        else if (difference < -deadzone)
        {
             direction = -1f;
        }
     
        pos += direction * speed * Time.deltaTime;

        float clampedY = Mathf.Clamp(pos, -4.5f, 4.5f);

        transform.position = new Vector2 (transform.position.x, clampedY);

    }
}
