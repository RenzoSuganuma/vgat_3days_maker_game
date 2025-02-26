using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// パーティクルを再生する機能
/// </summary>
public class ParticleGenerate : MonoBehaviour
{
    [SerializeField] GameObject _catchParticle;
    [SerializeField] GameObject _confettiParticle;

    private void Start()
    {
        // 最初パーティクルは非表示にしておく
        _catchParticle.SetActive(false);
        _confettiParticle.SetActive(false);
    }

    /// <summary>
    /// 次の振り子を掴んだ時のパーティクルを再生
    /// </summary>
    public async UniTask PlayCatchParticle()
    {
        _catchParticle.SetActive(true);

        await UniTask.Delay(500); // 再生時間は0.5秒

        _catchParticle.SetActive(false);
    }

    public async UniTask PlayConfettiParticle()
    {
        _confettiParticle.SetActive(true);
    }
}
