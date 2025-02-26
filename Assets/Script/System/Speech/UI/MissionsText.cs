using TMPro;
using UnityEngine;

public class MissionsText : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshProUGUI;

    /// <summary>
    /// 読み上げるフレーズを表示
    /// </summary>
    public void SetMissionText(string phrase)
    {
        _textMeshProUGUI.text = $"🔊 読み上げてください: {phrase}";
        Debug.Log($"🔊 読み上げてください: {phrase}");
    }

    /// <summary>
    /// 正しい発音をした場合
    /// </summary>
    public void MissionSuccess()
    {
        _textMeshProUGUI.text = "🎉 成功！正しく発音されました！";
        Debug.Log("🎉 成功！正しく発音されました！");
    }

    /// <summary>
    /// 間違った発音をした場合
    /// </summary>
    public void MissionFail()
    {
        _textMeshProUGUI.text = "❌ 失敗！もう一度発音してください";
        Debug.Log("❌ 失敗！もう一度発音してください");
    }
}
