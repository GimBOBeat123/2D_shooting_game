using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main"); // 장면 이름이나 SceneManager.GetActiveScene().buildIndex + 1 하면 됨
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
