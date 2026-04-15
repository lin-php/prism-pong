
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
    private GameObject bestBall;
    private float bestX;
    private float targetY;
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

        if (balls == null || balls.Count == 0) return;
        
        // checking all balls for new target
        TargetedBall();
        if (currentBall == null)
        {
            targetY = 0f;
        }
        else
        {
            targetY = currentBall.transform.position.y; 
        }

        // movement
        float difference = (targetY + currentOffset) - pos;
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

    private void TargetedBall()
    {
        bestBall = null;
        bestX = Mathf.NegativeInfinity;

        if (balls == null || balls.Count == 0) return;
        
        foreach (GameObject ball in balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

            if (rb == null || rb.linearVelocity.x <= 0)
            {
                  continue;
            }

            float x = ball.transform.position.x;

            if (x > bestX)
            {
                  bestX = x;
                  bestBall = ball;
            }
        }
         
        currentBall = bestBall;

        
    }

}
