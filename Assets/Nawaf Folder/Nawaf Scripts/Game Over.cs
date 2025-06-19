using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }

    public void RestartGame()
    {
        // Reload the previous gameplay scene
        SceneManager.LoadScene(PlayerPrefs.GetString("LastGameScene", "GameScene")); // Replace "GameScene" with your default gameplay scene name
    }
}