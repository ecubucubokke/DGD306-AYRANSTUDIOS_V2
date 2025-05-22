using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameMode
    {
        SinglePlayer,
        TwoPlayer
    }
    
    public GameMode currentGameMode { get; private set; }
    
    [Header("Game Over Settings")]
    [SerializeField] private float gameOverDelay = 3f;
    [SerializeField] private GameObject gameOverPanelPrefab;
    private GameObject gameOverPanel;
    private Canvas mainCanvas;
    
    private bool player1Dead = false;
    private bool player2Dead = false;
    
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Menu")
        {
            SetupGameOverPanel();
            ResetGameState();
        }
    }

    private void ResetGameState()
    {
        player1Dead = false;
        player2Dead = false;
    }

    private void SetupGameOverPanel()
    {
        // Find or create main canvas
        mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            GameObject canvasObj = new GameObject("Main Canvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // Create game over panel if it doesn't exist
        if (gameOverPanel == null && gameOverPanelPrefab != null)
        {
            gameOverPanel = Instantiate(gameOverPanelPrefab, mainCanvas.transform);
            gameOverPanel.SetActive(false);
        }
    }
    
    public void SetGameMode(GameMode mode)
    {
        currentGameMode = mode;
    }
    
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
    
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void PlayerDied(PlayerInputManager.PlayerNumber playerNumber)
    {
        if (currentGameMode == GameMode.SinglePlayer)
        {
            GameOver();
            return;
        }

        // Two player mode
        if (playerNumber == PlayerInputManager.PlayerNumber.Player1)
        {
            player1Dead = true;
        }
        else
        {
            player2Dead = true;
        }

        // If either player is dead in two player mode, it's game over
        if (player1Dead || player2Dead)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSecondsRealtime(gameOverDelay);
        ReturnToMenu();
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