using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
        InitializeOptionSettings();
        _optionView.Initialize(CloseOptionPanel);
    }

    private void InitializeOptionSettings()
    {
        _controlPresenter.Initialize(_optionModel);
    }

    /// <summary>
    /// オプションパネルを開く
    /// </summary>
    public void OpenOptionPanel()
    {
        _optionView.ShowOptionPanel();
    }

    /// <summary>
    /// オプションパネルを閉じる
    /// </summary>
    public void CloseOptionPanel()
    {
        _optionView.HideOptionPanel();
    }
}
