using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// パーティクルを再生する機能
/// </summary>
public class ParticleGenerater : MonoBehaviour
{
    [SerializeField] GameObject _catchParticle;
    [SerializeField] GameObject _confettiParticle;

    private void Start()
    {
        // 最初パーティクルは非表示にしておく

        if (_catchParticle != null)
        {
            _catchParticle.SetActive(false);
        }

        if (_confettiParticle != null)
        {
            _confettiParticle.SetActive(false);
        }
    }

    /// <summary>
    /// 次の振り子を掴んだ時のパーティクルを再生
    /// </summary>
    [ContextMenu("Generate: CatchParticle")] // テスト用
    public async UniTask PlayCatchParticle()
    {
        _catchParticle.SetActive(true);

        await UniTask.Delay(500); // 再生時間は0.5秒

        _catchParticle.SetActive(false);
    }

    /// <summary>
    /// 紙吹雪のパーティクルを再生
    /// </summary>
    [ContextMenu("Generate: ConfettiParticle")]
    public async UniTask PlayConfettiParticle()
    {
        _confettiParticle.SetActive(true);

        await UniTask.Delay(1000); // 再生時間は1秒

        _confettiParticle.SetActive(false);
    }
}
