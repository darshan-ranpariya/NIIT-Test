using UnityEngine;
using UnityEngine.UI;

public class InstructionsScreen : BaseScreen
{
    [SerializeField] private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        _uiManager.ShowScreen<LoadingScreen>();
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartClicked);
    }
}
