using DG.Tweening;
using UnityEngine;

/// <summary>
/// 観客の画像をビートに合わせて移動させる
/// </summary>
public class AudienceBeatSync : MonoBehaviour
{
    [SerializeField] private Transform _sprite;
    [SerializeField] private ObjectBeatSync _objectSync;

    private void Start()
    {
        _objectSync.OnBeat += OnBeat;
    }

    private void OnDestroy()
    {
        _objectSync.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        _sprite.DOMoveY(transform.position.y + 0.2f, 0.24f).SetLoops(2, LoopType.Yoyo);
    }
}
