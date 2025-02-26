using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボイスデータのためのスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "VoiceData", menuName = "Create Scriptable Objects/VoiceDataSO")]
public class VoiceDataSO : ScriptableObject
{
    [SerializeField] private List<VoiceData> _voiceData = new List<VoiceData>();

    /// <summary>
    /// インデックスで指定したボイスデータを返す
    /// </summary>
    public VoiceData GetVoiceData(VoiceNameEnum voiceName)
    {
        return _voiceData[(int)voiceName];
    }

    /// <summary>
    /// インデックスで指定したボイスデータから、ランダムなクリップと設定されたvolumeを返す
    /// </summary>
    public (AudioClip Clip, float Volume) GetRandomVoiceClip(VoiceNameEnum voiceName)
    {
        VoiceData data = _voiceData[(int)voiceName];
        int rand = Random.Range(0, data.Clips.Count);
        return (data.Clips[rand], data.Volume);
    }
}
