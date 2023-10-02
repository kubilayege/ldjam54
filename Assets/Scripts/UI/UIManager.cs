using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonBehaviour<UIManager>
{
    // TODO: manage UI

    [SerializeField] private GameUI mainMenuUI;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameUI settingsUI;
    [SerializeField] private GameUI winUI;
    [SerializeField] private GameUI loseUI;

    private GameUI _currentUI;

    protected override async void Awake()
    {
        base.Awake();

        // GameManager.Instance.OnLose += OnLose;
        // GameManager.Instance.OnWin += OnWin;
        //
        // await SwitchTo(mainMenuUI);
    }

    private async UniTask SwitchTo(GameUI ui)
    {
        if (_currentUI != null)
        {
            await _currentUI.FadeOut();
        }

        await ui.FadeIn();

        _currentUI = ui;
    }
    
    
    public async void PlayButtonPressed()
    {
        SceneManager.LoadScene("Game");
        // await SwitchTo(gameUI);
    }
    
    public async void SettingsButtonPressed()
    {
        await SwitchTo(settingsUI);
    }

    private async void OnLose(LoseData loseData)
    {
        await SwitchTo(winUI);
        // winUI
    }

    private async void OnWin(WinData winData)
    {
        await SwitchTo(winUI);
    }
}