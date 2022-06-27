using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// creates variables and Methods to enable MenuButton functionality
    /// </summary>
    [SerializeField] private SettingsData settingsdata;

    [SerializeField] private Toggle wallJumpToggle;
    [SerializeField] private Toggle doubleJumpToggle;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject gameMenu;

    public void SetWallJumpValue()
    {
        settingsdata.enableWallJump = wallJumpToggle.isOn;
    }

    public void SetDoubleJumpValue()
    {
        settingsdata.enableDoubleJump = doubleJumpToggle.isOn;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Highscore() //added for Later implementation
    {
    }

    public void Credits()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameMenu()
    {
        SceneManager.UnloadSceneAsync("GameScene");
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}