using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameMode
    {
        SinglePlayer,
        TwoPlayer
    }
    
    public GameMode currentGameMode { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetGameMode(GameMode mode)
    {
        currentGameMode = mode;
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Oyun sahnenizin adını buraya yazın
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void Start()
    {
        Debug.Log("MenuController başlatıldı!");
    }

    private void StartGame(GameManager.GameMode mode)
    {
        Debug.Log("StartGame çağrıldı: " + mode);
        GameManager.Instance.SetGameMode(mode);
        GameManager.Instance.StartGame();
    }
} 