using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene(0);
    }
    public void showOptions()
    {
    }
    public void showCredits()
    {

    }
    public void quitGame()
    {
        Application.Quit();
    }
}
