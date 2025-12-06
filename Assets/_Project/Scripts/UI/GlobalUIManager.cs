using UnityEngine;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviour
{
    [Header("Options Overlay")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Slider volumeSlider;

    [Header("Back Button")]
    [SerializeField] private Button backButton;

    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    private void OnEnable()
    {
        // Subscribe to the central UIManager event
        UIManager.OnNavigationStackChanged += UpdateBackButtonState;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        UIManager.OnNavigationStackChanged -= UpdateBackButtonState;
    }

    void Start()
    {
        optionsPanel.SetActive(false);
        optionsButton.onClick.AddListener(ToggleOptionsPanel);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        backButton.onClick.AddListener(OnBackClicked);
        volumeSlider.value = AudioListener.volume;
    }

    private void ToggleOptionsPanel()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void OnBackClicked()
    {
        _uiManager.GoBack();
    }

    // This method now listens to the UIManager, not a specific screen.
    private void UpdateBackButtonState(int stackCount)
    {
        // If there is more than 1 screen in the stack, we can go back.
        backButton.gameObject.SetActive(stackCount > 1);
    }

    private void OnDestroy()
    {
        optionsButton.onClick.RemoveListener(ToggleOptionsPanel);
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        backButton.onClick.RemoveListener(OnBackClicked);
    }
}
