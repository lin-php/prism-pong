using System.Collections;
using UnityEngine;

public class MiddleClearEvent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer deletingZone;

    public static MiddleClearEvent Instance;
    private Color originalColor;

    public void EventPlay()
    {
      
        StartCoroutine(EventSlow());
        StartCoroutine(DeletingZoneFlash());

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
