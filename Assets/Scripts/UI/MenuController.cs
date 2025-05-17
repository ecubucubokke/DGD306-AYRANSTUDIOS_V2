using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button twoPlayerButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Options Panel")]
    [SerializeField] private Button backButton;
    
    private void Start()
    {
        // Buton dinleyicilerini ekle
        singlePlayerButton.onClick.AddListener(() => StartGame(GameManager.GameMode.SinglePlayer));
        twoPlayerButton.onClick.AddListener(() => StartGame(GameManager.GameMode.TwoPlayer));
        optionsButton.onClick.AddListener(ShowOptions);
        quitButton.onClick.AddListener(QuitGame);
        
        // Options panel geri butonu
        if (backButton != null)
            backButton.onClick.AddListener(ShowMainMenu);
        
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

    private void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
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