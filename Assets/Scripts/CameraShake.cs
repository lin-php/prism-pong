using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance;

    private Vector3 originalPosition;

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

        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float duration = 0.15f;
        float strength = 0.15f;

        float timer = 0f;

        while (timer < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

}
