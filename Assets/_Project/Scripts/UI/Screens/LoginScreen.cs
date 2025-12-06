using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : BaseScreen
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameDataSO gameData;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        errorText.gameObject.SetActive(false);
    }

    private void OnLoginClicked()
    {
        if (string.IsNullOrWhiteSpace(usernameInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            errorText.text = "Username and password cannot be empty.";
            errorText.gameObject.SetActive(true);
            return;
        }

        // Validation passed
        errorText.gameObject.SetActive(false);
        gameData.username = usernameInput.text;
        
        _uiManager.ShowScreen<LevelSelectionScreen>();
    }

    public override void Show()
    {
        base.Show();
        // Clear fields when the screen is shown
        usernameInput.text = "";
        passwordInput.text = "";
        errorText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        loginButton.onClick.RemoveListener(OnLoginClicked);
    }
}
