
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MiddleClearEvent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer deletingZone;
    
    public static MiddleClearEvent Instance;
    private Color originalColor;


    public void EventPlay(List<GameObject>balls)
    {
      
        StartCoroutine(EventSlow());
        StartCoroutine(DeletingZoneFlash());

        foreach (GameObject ball in balls)
        {
            StartCoroutine(BallImpactRoutine(ball));
        }
    }

    private IEnumerator BallImpactRoutine(GameObject ball)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        Vector2 directionToCenter = ((Vector2)Vector3.zero - rb.position).normalized;
        rb.AddForce(directionToCenter * 20f, ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(0.1f);

        rb.linearVelocity *= 0.05f;
        
    }

    private IEnumerator EventSlow()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 1f;
    }

    private IEnumerator DeletingZoneFlash()
    {
        deletingZone.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.01f);
        deletingZone.gameObject.SetActive(false);
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

        originalColor = deletingZone.color;
    }

}
