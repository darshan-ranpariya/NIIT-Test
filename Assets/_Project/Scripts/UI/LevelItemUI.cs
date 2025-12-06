using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemUI : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private string levelName;
    [SerializeField] private Sprite levelIcon;

    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button selectionButton;

    [SerializeField]private LevelSelectionScreen levelSelectionScreen;


    private void Start()
    {
        // Setup UI from data
        iconImage.sprite = levelIcon;
        nameText.text = levelName;
        selectionButton.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        levelSelectionScreen.OnLevelSelected(levelName, levelIcon);
    }

    private void OnDestroy()
    {
        selectionButton.onClick.RemoveListener(OnClicked);
    }
}
