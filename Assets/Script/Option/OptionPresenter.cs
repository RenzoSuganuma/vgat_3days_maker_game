using UnityEngine;

public class OptionPresenter : MonoBehaviour
{
    [SerializeField] private ControlPresenter _controlPresenter;
    [SerializeField] private OptionView _optionView;
    private OptionModel _optionModel;
    private AudioVolumeSettings _defaultSettings;

    private void Start()
    {
        // ゲーム設定（デフォルト音量）をロード
        _defaultSettings = Resources.Load<GameSettings>("GameSettings")?.AudioVolumeSettings;

        if (_defaultSettings == null)
        {
            Debug.LogError("GameSettingsが見つかりません。デフォルト値を使用します。");
            _defaultSettings = new AudioVolumeSettings(); // デフォルト値を適用
        }

        // OptionModelを初期化（デフォルト値をロードした後にセーブデータを適用）
        _optionModel = new OptionModel(_defaultSettings);

        // コントロールパネルを初期化
        _controlPresenter.Initialize(_optionModel);
        _optionView.Initialize(CloseOptionPanel);
    }

    public void OpenOptionPanel()
    {
        _optionView.ShowOptionPanel();
    }

    public void CloseOptionPanel()
    {
        _optionView.HideOptionPanel();
    }
}
