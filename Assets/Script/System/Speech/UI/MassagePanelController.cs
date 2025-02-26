/// <summary>
/// 音声入力を受け取った時の吹きだし変更・ボイス再生を管理する
/// </summary>
public class SpeechBubbleManager
{
    private MissionsDisplay _missionsDisplay;

    public SpeechBubbleManager(MissionsDisplay missionsDisplay)
    {
        _missionsDisplay = missionsDisplay;
    }

    /// <summary>
    /// 吹きだし変更・ボイス再生の処理
    /// </summary>
    public void ChangeDialogueAndPlayVoice(VoiceNameEnum voiceName)
    {
        _missionsDisplay.SetMessagePanel(voiceName); // 吹きだしの画像を変更
        _missionsDisplay.ShowMessagePanel(); // 吹きだしの画像を表示
        AudioManager.Instance.PlayRandomVoice(voiceName); // ボイス再生
    }
}
