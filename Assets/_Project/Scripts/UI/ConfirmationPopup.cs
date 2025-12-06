using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopup : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button readInstructionsButton;
    [SerializeField] private Button closeButton; // Optional, good for UX

    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = UIManager.inst;
    }

    private void Start()
    {
        startGameButton.onClick.AddListener(OnStartGame);
        readInstructionsButton.onClick.AddListener(OnReadInstructions);
        if(closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnStartGame()
    {
        _uiManager.ShowScreen<LoadingScreen>();
    }

    private void OnReadInstructions()
    {
        _uiManager.ShowScreen<InstructionsScreen>();
    }

    private void OnDestroy()
    {
        startGameButton.onClick.RemoveListener(OnStartGame);
        readInstructionsButton.onClick.RemoveListener(OnReadInstructions);
        if(closeButton != null)
        {
            closeButton.onClick.RemoveListener(Hide);
        }
    }
}
