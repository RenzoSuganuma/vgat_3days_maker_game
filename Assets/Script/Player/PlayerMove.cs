using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// プレイヤーが振り子から振り子に移るスクリプト
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [Header("ジャンプの設定")]
    [SerializeField, Tooltip("ジャンプ時間")] private float _jumpDuration = 1.0f;
    [SerializeField, Tooltip("最大高さ")] private float height = 3.0f;
    [SerializeField, Tooltip("重力加速度")] private float _gravity = 9.8f;
    [SerializeField] private PlayerJumpingSprite _playerJumpingSprite;
    [SerializeField] private PlayerAnimation _animation;
    [SerializeField] private ParticleGenerater _particleGenerater;

    public ParticleGenerater ParticleGenerater => _particleGenerater;
    private Vector3 _initialLocalPos;

    public bool IsJumping { get; private set; } // ジャンプ中か

    private void Start()
    {
        _initialLocalPos = transform.localPosition;
        _animation.SetPendulumController(transform.parent.GetComponent<PendulumController>());
    }

    /// <summary>
    /// 次の振り子に飛び移る
    /// </summary>
    public void JumpToNextPendulum(Transform target)
    {
        if (IsJumping) return;

        transform.SetParent(target); // 親を変更
        _animation.SetPendulumController(target.gameObject.GetComponent<PendulumController>());
        IsJumping = true;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos = _initialLocalPos;

        _playerJumpingSprite.Jump().Forget(); // プレイヤーの画像を変更する

        /*
        float peakTime = _jumpDuration / 2f; // 頂点に達する時間
        float gravity = (2 * height) / (peakTime * peakTime); // 自然な重力値
        float initialVelocityY = Mathf.Sqrt(2 * gravity * height); // 放物線の初速
        */

        DOVirtual.Float(0, _jumpDuration, _jumpDuration, elapsedTime =>
            {
                float t = elapsedTime / _jumpDuration;

                // X, Zはイージングを使って補間
                float curvedT = t * (2 - t);
                float x = Mathf.Lerp(startPos.x, endPos.x, curvedT);
                float z = Mathf.Lerp(startPos.z, endPos.z, curvedT);

                // Yは放物運動
                float y = Mathf.Lerp(startPos.y, endPos.y, curvedT);
                //float y = startPos.y + (initialVelocityY * elapsedTime) - (0.5f * gravity * elapsedTime * elapsedTime);

                transform.localPosition = new Vector3(x, y, z);
            })
            .SetEase(Ease.Linear)
            .OnComplete(Catch);
    }

    /// <summary>
    /// プレイヤーの位置をブランコを掴む位置に補正する
    /// </summary>
    private void Catch()
    {
        _particleGenerater.PlayCatchParticle(); // 振り子を掴んだ時のパーティクルを再生する
        transform.localPosition = _initialLocalPos;
        transform.localRotation = Quaternion.identity;
        IsJumping = false;
    }
}
