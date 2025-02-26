using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボイスデータのためのスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "VoiceDataSO", menuName = "Create Scriptable Object/VoiceDataSO")]
public class VoiceDataSO : ScriptableObject
{
    [SerializeField] private List<VoiceData> _voiceData = new List<VoiceData>();

    /// <summary>
    /// インデックスで指定したボイスデータを返す
    /// </summary>
    public VoiceData GetVoiceData(int index)
    {
        return _voiceData[index];
    }

    /// <summary>
    /// インデックスで指定したボイスデータから、ランダムなクリップと設定されたvolumeを返す
    /// </summary>
    public (AudioClip Clip, float Volume) GetRandomVoiceClip(int index)
    {
        VoiceData data = _voiceData[index];
        int rand = Random.Range(0, data.Clips.Count);
        return (data.Clips[rand], data.Volume);
    }
}
