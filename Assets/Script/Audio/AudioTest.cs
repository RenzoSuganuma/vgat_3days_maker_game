using UnityEngine;

/// <summary>
/// Audioのテスト用スクリプト
/// </summary>
public class AudioTest : MonoBehaviour
{
    public void Update()
    {
        // BGM
        if(Input.GetKeyDown(KeyCode.A)) AudioManager.Instance.PlayBGM(SceneNameEnum.Title);
        if(Input.GetKeyDown(KeyCode.S)) AudioManager.Instance.PlayBGM(SceneNameEnum.InGame);
        if(Input.GetKeyDown(KeyCode.D)) AudioManager.Instance.PlayBGM(SceneNameEnum.Result);

        // SE
        if(Input.GetKeyDown(KeyCode.Q)) AudioManager.Instance.PlaySE(SENameEnum.Falling);
        if(Input.GetKeyDown(KeyCode.W)) AudioManager.Instance.PlaySE(SENameEnum.RollUp);
        if(Input.GetKeyDown(KeyCode.E)) AudioManager.Instance.PlaySE(SENameEnum.Swing);
        if(Input.GetKeyDown(KeyCode.R)) AudioManager.Instance.PlaySE(SENameEnum.TimeUp);
        if(Input.GetKeyDown(KeyCode.T)) AudioManager.Instance.PlaySE(SENameEnum.Ranking);

        // Voice
        if(Input.GetKeyDown(KeyCode.Z)) AudioManager.Instance.PlayRandomVoice(VoiceNameEnum.dB50);
        if(Input.GetKeyDown(KeyCode.X)) AudioManager.Instance.PlayRandomVoice(VoiceNameEnum.dB60);
        if(Input.GetKeyDown(KeyCode.C)) AudioManager.Instance.PlayRandomVoice(VoiceNameEnum.dB70);
        if(Input.GetKeyDown(KeyCode.V)) AudioManager.Instance.PlayRandomVoice(VoiceNameEnum.dB80);
    }
}
