using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Slider volumeSlider;

    
    private void Awake()
    {

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();

        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;

        AudioController.Instance.PlayMenuTheme();

    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
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

}
