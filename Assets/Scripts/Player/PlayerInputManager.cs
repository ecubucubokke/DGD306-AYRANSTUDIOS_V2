using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public enum PlayerNumber
    {
        Player1,
        Player2
    }

    public PlayerNumber playerNumber;

    [Header("Input Settings")]
    public string horizontalAxis;
    public string verticalAxis;
    public string jumpButton;
    public string fireButton;

    private void Awake()
    {
        // Oyuncu numarasına göre input ayarlarını yap
        if (playerNumber == PlayerNumber.Player1)
        {
            horizontalAxis = "Horizontal";
            verticalAxis = "Vertical";
            jumpButton = "Jump";
            fireButton = "Fire1";
        }
        else
        {
            horizontalAxis = "Horizontal2";
            verticalAxis = "Vertical2";
            jumpButton = "Jump2";
            fireButton = "Fire2";
        }
    }

    private void Start()
    {
        // Tek oyunculu modda Player2'yi devre dışı bırak
        if (playerNumber == PlayerNumber.Player2 && 
            GameManager.Instance.currentGameMode == GameManager.GameMode.SinglePlayer)
        {
            gameObject.SetActive(false);
        }
    }
} 