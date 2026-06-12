using UnityEngine;
using UnityEngine.SceneManagement;

public class TrailerLoader : MonoBehaviour
{
    [SerializeField] private string sceneName = "Trailer_Szene";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}