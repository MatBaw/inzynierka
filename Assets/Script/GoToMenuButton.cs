using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenuButton : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MenuScene";

    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}