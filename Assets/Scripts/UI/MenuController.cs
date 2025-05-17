using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button twoPlayerButton;
    [SerializeField] private Button quitButton;
    
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    private void Start()
    {
        // Buton dinleyicilerini ekle
        singlePlayerButton.onClick.AddListener(() => StartGame(GameManager.GameMode.SinglePlayer));
        twoPlayerButton.onClick.AddListener(() => StartGame(GameManager.GameMode.TwoPlayer));
        quitButton.onClick.AddListener(QuitGame);
        
        // Ana menüyü göster
        ShowMainMenu();
    }
    
    private void StartGame(GameManager.GameMode mode)
    {
        GameManager.Instance.SetGameMode(mode);
        GameManager.Instance.StartGame();
    }
    
    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }
    
    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 