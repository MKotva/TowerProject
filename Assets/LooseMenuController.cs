using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseMenuController : MonoBehaviour
{
    [SerializeField] private ScreenBlanketController screenBlanketController;
    [SerializeField] private string firstSceneName = "MainMenu";

    private void Start()
    {
        screenBlanketController.FadeFromBlack();   
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void ResetGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (!string.IsNullOrEmpty(firstSceneName))
        {
            SceneManager.LoadScene(firstSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
