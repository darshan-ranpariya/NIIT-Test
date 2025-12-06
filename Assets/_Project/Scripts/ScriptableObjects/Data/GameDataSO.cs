using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "NIIT/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("User Data")]
    public string username;

    [Header("Level Selection")]
    public string selectedLevelName;
    public Sprite selectedLevelIcon;
}
