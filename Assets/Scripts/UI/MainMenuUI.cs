using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public void OnPlayButtonPress()
    {
        UIManager.Instance.PlayButtonPressed();
    }

    public void SettingsButtonPress()
    {
        UIManager.Instance.SettingsButtonPressed();
    }
}