using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField] 
    private GameObject credits;
    [SerializeField]
    private SettingsData settingsdata;
    [SerializeField]
    private Toggle WallJumpToggle;
    [SerializeField]
    private Toggle DoubleJumpToggle;
    public void MainMenu()
    {
        #if UNITY_EDITOR 
        Debug.Log("Hi");
        #endif
        SceneManager.LoadScene("MainMenu");
        
    }
    public void SetWallJumpValue()
    {
        settingsdata.EnableWallJump = WallJumpToggle.isOn;
    }
    
    public void SetDoubleJumpValue()
    {
        settingsdata.EnableDoubleJump = DoubleJumpToggle.isOn;
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
        #if UNITY_EDITOR 
        Debug.Log("Exiting");
        #endif
        
        Application.Quit();
    }
}
