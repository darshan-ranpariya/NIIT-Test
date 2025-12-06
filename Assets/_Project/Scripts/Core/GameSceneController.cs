using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameDataSO gameData;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI welcomeLabel;
    [SerializeField] private Image levelIconImage;
    [SerializeField] private Button returnToMenuButton;

    [Header("Configuration")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        if (gameData == null)
        {
            Debug.LogError("GameDataSO not assigned!");
            return;
        }

        // Display the data from the ScriptableObject
        welcomeLabel.text = $"Welcome, {gameData.username}!\nYou are playing {gameData.selectedLevelName}.";
        
        if (gameData.selectedLevelIcon != null)
        {
            levelIconImage.sprite = gameData.selectedLevelIcon;
            levelIconImage.gameObject.SetActive(true);
        }
        else
        {
            levelIconImage.gameObject.SetActive(false);
        }

        returnToMenuButton.onClick.AddListener(OnReturnToMenu);
    }

    private void OnReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void OnDestroy()
    {
        returnToMenuButton.onClick.RemoveListener(OnReturnToMenu);
    }
}
