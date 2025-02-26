using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーがジャンプするタイミングで画像の差分を切り替える
/// </summary>
public class PlayerJumpingSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] private Image _image;
    [SerializeField] private float _animSpeed = 0.06f;
    private int _index = 0;

    /// <summary>
    /// ランダムにプレイヤーの画像を変更する
    /// </summary>
    [ContextMenu("Jump")]
    public async UniTask Jump()
    {
        _index = 0; // 初期化

        for (int i = 0; i < _sprites.Count; i++)
        {
            _image.sprite = _sprites[_index];
            _index++;
            await UniTask.Delay(TimeSpan.FromSeconds(_animSpeed));
        }
    }
}
