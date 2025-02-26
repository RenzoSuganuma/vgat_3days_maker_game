using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音声データを管理するスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "ClipDataSO", menuName = "Create Scriptable Objects/ClipDataSO")]
public class ClipDataSO : ScriptableObject
{
    public List<ClipData> Clips;

    /// <summary>
    /// クリップデータを獲得する
    /// </summary>
    public ClipData GetClipData(int index)
    {
        return Clips[index];
    }
}
