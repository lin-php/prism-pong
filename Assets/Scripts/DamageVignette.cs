
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DamageVignette : MonoBehaviour
{

    [SerializeField] private Volume volume;
    [SerializeField] private float fadeSpeed = 0.5f;

    private Vignette vignette;
    private Coroutine vignetteCoroutine;

    void Awake()
    {
        volume.profile.TryGet(out vignette);
    }

    private void Start()
    {
        vignette.active = false;
    }

    public void PlayDamageVignette()
    {
        if (vignetteCoroutine != null) 
        {
            StopCoroutine(vignetteCoroutine);
        }

        vignetteCoroutine = StartCoroutine(Vignette());
    }

    private IEnumerator Vignette()
    {
        vignette.active = true;

        vignette.intensity.value = 1f;
        
        while (vignette.intensity.value > 0)
        {
            vignette.intensity.value -= Time.deltaTime * fadeSpeed; 
            yield return null;
        }
        vignette.active = false;
        vignetteCoroutine = null;
    }

    public void StopDamageVignette()
    {
        if (vignetteCoroutine != null)
        {
            StopCoroutine(vignetteCoroutine);
        }

        vignette.active = false;
       
    }

}
