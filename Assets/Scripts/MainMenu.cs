using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play() {
           SceneManager.LoadScene("MainGame");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit");
    }
}
