using System.Collections;
using System.Threading;
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

    private void Awake()
    {
        Time.timeScale = 1f;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();

        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;

        AudioController.Instance.PlayMenuTheme();

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

}
