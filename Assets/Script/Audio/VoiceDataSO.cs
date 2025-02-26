using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボイスデータのためのスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "VoiceData", menuName = "Create Scriptable Objects/VoiceDataSO")]
public class VoiceDataSO : ScriptableObject
{
    [SerializeField] private List<VoiceDatas> _voiceData = new List<VoiceDatas>();

    /// <summary>
    /// インデックスで指定したボイスデータから、ランダムなクリップと設定されたvolumeを返す
    /// </summary>
    public VoiceData GetRandomVoiceClip(VoiceNameEnum voiceName)
    {
        VoiceDatas data = _voiceData[(int)voiceName];
        int rand = Random.Range(0, data.VoiceDatasList.Count);
        return data.VoiceDatasList[rand];
    }
}
