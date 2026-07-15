 using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject DeathMenu;
    void Awake()
    {
        instance = this;
    }
    public void PlayerDied()
    {
        DeathMenu.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
