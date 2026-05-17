using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Slider volumeSlider;
    [SerializeField] private Animator screenFade;

    private AsyncOperation asyncLoad;
    private bool isLoading = false;
    private float targetVolume;

    private void Awake()
    {
        Time.timeScale = 1f;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();

        float volume = PlayerPrefs.GetFloat("Volume", 0.7f);
        volumeSlider.value = volume;

        targetVolume = volume;
        AudioListener.volume = 0f;

        AudioController.Instance.PlayMenuTheme();
    }

    private void Start()
    {
        StartCoroutine(FadeAudioIn());
    }


    public void StartGame()
    {
        if (isLoading) return;
        isLoading = true;
        StartCoroutine(StartGameTransition());
    }

    public void SliderVolume(float volume)
    {
        volumeSlider.value = volume;
        targetVolume = volume;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator StartGameTransition()
    {
        screenFade.SetTrigger("FadeOut");

        asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(1.5f);

        asyncLoad.allowSceneActivation = true;
    }

    private IEnumerator FadeAudioIn()
    {
        while (AudioListener.volume < targetVolume)
        {
            AudioListener.volume += Time.deltaTime * 0.8f;
            yield return null;
        }

        AudioListener.volume = targetVolume;    
    }

}
