using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("City");
        Debug.Log("Started");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Started");
    }
}
