using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Game Started - loading town level");
        SceneManager.LoadScene((int)LevelNames.Town);
    }

    public void QuitApplication()
    {
        Debug.Log("Game Ended - quiting the aplication");
        Application.Quit();
    }
}
