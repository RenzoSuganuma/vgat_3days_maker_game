using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの振り子に掴まっているときのアニメーションを追加する
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private SpriteRenderer _image;
    [SerializeField, Tooltip("キャラクターの画像を変更しておく時間")] private float _duration = 1f;
    private PendulumController _pendulumController;

    private  CompositeDisposable _disposable = new  CompositeDisposable();

    private void Start()
    {
        _image.sprite = _sprites[0];
    }

    private void OnDestroy()
    {
        _disposable?.Dispose();

        if(_pendulumController == null) return;
        _pendulumController.OnEdgeLeft -= OnEdge;
        _pendulumController.OnEdgeRight -= OnRelease;
    }

    /// <summary>
    /// 振り子のスクリプトの参照をセットする
    /// </summary>
    public void SetPendulumController(PendulumController pendulumController)
    {
        _pendulumController = pendulumController;
        _pendulumController.OnEdgeLeft += OnEdge;
        _pendulumController.OnEdgeRight += OnRelease;
    }

    /// <summary>
    /// 左端に到達したときのメソッド
    /// </summary>
    private void OnEdge()
    {
        ChangeSprite(2, 40).Forget();
    }

    /// <summary>
    /// 右端に到達したときのもの
    /// </summary>
    private void OnRelease()
    {
        ChangeSprite(1, -40).Forget();
    }

    /// <summary>
    /// スプライトを変更する
    /// </summary>
    private async UniTask ChangeSprite(int index, int angle)
    {
        _image.sprite = _sprites[index];

        float elapsedTime = 0f;
        var rotationProgress = new ReactiveProperty<float>(0f);

        rotationProgress
            .Select(progress => Mathf.Lerp(0f, angle, progress))
            .Subscribe(angle =>
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            })
            .AddTo(_disposable);

        while (elapsedTime < _duration)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, -40f, elapsedTime / _duration));
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(); // フレームごとに待機
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, -40f);
        await UniTask.Delay((int)(_duration * 1000)); // アニメーション時間を待つ
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

    }
}
