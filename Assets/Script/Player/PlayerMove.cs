using DG.Tweening;
using UnityEngine;

/// <summary>
/// プレイヤーが振り子から振り子に移るスクリプト
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [Header("ジャンプの設定")]
    [SerializeField, Tooltip("ジャンプ時間")] private float _jumpDuration = 1.0f;
    [SerializeField, Tooltip("最大高さ")] private float height = 3.0f;
    [SerializeField, Tooltip("重力加速度")] private float _gravity = 9.8f;

    private Transform _target;
    private Vector3 _initialLocalPos;

    public bool IsJumping { get; private set; } // ジャンプ中か

    private void Start()
    {
        _initialLocalPos = transform.localPosition;
    }

    /// <summary>
    /// プレイヤーが次の振り子に飛び移るメソッド
    /// </summary>
    private void Move()
    {
        JumpToNextPendulum();
    }

    /// <summary>
    /// 次の振り子に飛び移る
    /// </summary>
    public void JumpToNextPendulum()
    {
        if (IsJumping) return;

        transform.SetParent(_target); // 一時的に親を変更
        IsJumping = true;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos = _initialLocalPos;

        float peakTime = _jumpDuration / 2; // 頂点に達する時間
        float initialVelocityY = (2 * height) / peakTime; // 放物線の初速

        float elapsedTime = 0f;

        DOTween.To(
            () => elapsedTime,
            x => elapsedTime = x,
            _jumpDuration,
            _jumpDuration
        ).SetEase(Ease.Linear).OnUpdate(() =>
        {
            float t = elapsedTime / _jumpDuration;
            float x = Mathf.Lerp(startPos.x, endPos.x, t);
            float z = Mathf.Lerp(startPos.z, endPos.z, t);
            float y = startPos.y + (initialVelocityY * elapsedTime) - (0.5f * _gravity * elapsedTime * elapsedTime);

            transform.localPosition = new Vector3(x, y, z);
        })
        .OnComplete(Catch);
    }

    /// <summary>
    /// プレイヤーの位置をブランコを掴む位置に補正する
    /// </summary>
    private void Catch()
    {
        transform.localPosition = _initialLocalPos;
        transform.localRotation = Quaternion.identity;
        IsJumping = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // テスト用
        {
            Move();
        }
    }
}
