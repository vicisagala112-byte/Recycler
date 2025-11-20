using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Explore");   // Pastikan nama “Explore” sesuai Build Settings
    }
}
