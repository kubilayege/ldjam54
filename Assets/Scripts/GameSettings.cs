using UnityEngine;

[CreateAssetMenu(menuName = "Create GameSettings", fileName = "GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    // TODO: Holds RoundSettings, GridSettings, DifficultySettings SO and other stuff
    public RoundSettings RoundSettings;
    public DifficultySettings DifficultySettings;
    public GridSettings GridSettings;
    
    
}
