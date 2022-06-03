using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{   /// <summary>
    /// creates variables and Methods to enable MenuButton functionality
    /// </summary>
    [SerializeField]
    private SettingsData settingsdata;
    [SerializeField]
    private Toggle WallJumpToggle;
    [SerializeField]
    private Toggle DoubleJumpToggle;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField] 
    private GameObject credits;
    [SerializeField] 
    private GameObject gameMenu;
    public void SetWallJumpValue()  
    {
        settingsdata.EnableWallJump = WallJumpToggle.isOn;
    }
    public void SetDoubleJumpValue()    
    {
        settingsdata.EnableDoubleJump = DoubleJumpToggle.isOn;
    }
    public void MainMenu()  
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayGame()
    {
        Debug.Log("A");
        //SceneManager.LoadScene("GameScene");
        //SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("GameScene");
    }
    public void Settings()  
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Highscore() 
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
        SceneManager.LoadScene("MainMenu");
    }
}
