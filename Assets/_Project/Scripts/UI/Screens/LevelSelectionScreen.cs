using UnityEngine;

public class LevelSelectionScreen : BaseScreen
{
    [SerializeField] private ConfirmationPopup confirmationPopup;
    [SerializeField] private GameDataSO gameData;

    public void OnLevelSelected(string levelName, Sprite levelIcon)
    {
        gameData.selectedLevelName = levelName;
        gameData.selectedLevelIcon = levelIcon;
        
        confirmationPopup.Show();
    }

    public override void Hide()
    {
        base.Hide();
        confirmationPopup.Hide();
    }
}
