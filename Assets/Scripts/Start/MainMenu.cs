using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main"); // ��� �̸��̳� SceneManager.GetActiveScene().buildIndex + 1 �ϸ� ��
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
