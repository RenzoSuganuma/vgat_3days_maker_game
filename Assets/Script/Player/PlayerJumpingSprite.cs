using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーがジャンプするタイミングで画像の差分を切り替える
/// </summary>
public class PlayerJumpingSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    private Image _image;

    /// <summary>
    /// ランダムにプレイヤーの画像を変更する
    /// </summary>
    public void SpriteChange()
    {
        int rand = Random.Range(0, _sprites.Count);
        _image.sprite = _sprites[rand];
    }
}
