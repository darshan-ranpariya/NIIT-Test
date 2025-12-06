using UnityEngine;
using UnityEngine.UI;

public class WelcomeScreen : BaseScreen
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        // Simply tell the UIManager to show the next screen.
        // The UIManager will handle hiding the current one.
        _uiManager.ShowScreen<LoginScreen>();
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(OnContinueClicked);
    }
}
