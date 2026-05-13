
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MiddleClearEvent : MonoBehaviour
{
    
    [SerializeField] private GameObject topFlash;
    [SerializeField] private GameObject bottomFlash;
    [SerializeField] private GameObject leftFlash;
    [SerializeField] private GameObject rightFlash; 
    
    public static MiddleClearEvent Instance;
    
    public void EventPlay(List<GameObject>balls)
    {
      
        StartCoroutine(EventSlow());
        
        foreach (GameObject ball in balls)
        {
            StartCoroutine(BallImpactRoutine(ball));
        }

        StartCoroutine(Flash());
        CameraShake.Instance.Shake(1.1f, 0.1f);
    }

    private IEnumerator BallImpactRoutine(GameObject ball)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        Animator animator = ball.GetComponent <Animator>();

        if (rb == null) yield break;

        /*
        Vector2 directionToCenter = ((Vector2)Vector3.zero - rb.position).normalized;
        rb.AddForce(directionToCenter * 2.5f, ForceMode2D.Impulse);
        */

        if (animator != null)
        {
            animator.SetTrigger("Impact");
        }

        rb.linearVelocity *= 0.08f;
        
    }

    // not sure of using it for now;
    private IEnumerator EventSlow()
    {
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 1f;
    }

    private IEnumerator Flash()
    {
        topFlash.SetActive(true);
        bottomFlash.SetActive(true); 
        leftFlash.SetActive(true);
        rightFlash.SetActive(true);
        yield return new WaitForSecondsRealtime(0.05f);
        topFlash.SetActive(false);
        bottomFlash.SetActive(false);
        leftFlash.SetActive(false);
        rightFlash.SetActive(false);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        topFlash.SetActive(false);
        bottomFlash.SetActive(false);
        leftFlash.SetActive(false);
        rightFlash.SetActive(false);

    }

}
