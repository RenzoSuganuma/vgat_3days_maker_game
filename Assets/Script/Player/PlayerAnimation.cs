using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの振り子に掴まっているときのアニメーションを追加する
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Image _image;
    [SerializeField, Tooltip("キャラクターの画像を変更しておく時間")] private float _duration = 1f;
    private PendulumController _pendulumController;
    private Vector3 pos;

    private void Start()
    {
        _image.sprite = _sprites[0];
        pos = transform.localPosition;
    }

    private void OnDestroy()
    {
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
        ChangeSprite(2).Forget();
        float posX = transform.localPosition.x - 50f; // 画像をずらす（手の位置がずれてしまうため）
        transform.localPosition = new Vector3(posX, transform.localPosition.y, transform.localPosition.z);
    }

    /// <summary>
    /// 右端に到達したときのもの
    /// </summary>
    private void OnRelease()
    {
        ChangeSprite(1).Forget();
        float posX = transform.localPosition.x + 50f;　// 画像をずらす（手の位置がずれてしまうため）
        transform.localPosition = new Vector3(posX, transform.localPosition.y, transform.localPosition.z);
    }

    /// <summary>
    /// スプライトを変更する
    /// </summary>
    private async UniTask ChangeSprite(int index)
    {
        _image.sprite = _sprites[index];

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));

        transform.localPosition = pos;
        _image.sprite = _sprites[0]; // 初期のスプライトに戻す
    }
}
