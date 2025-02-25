using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class FadePanelImpl : MonoBehaviour
{
    [SerializeField] float _duration;

    Image _fadePanel;

    private void Awake()
    {
        Foundation.TaskOnChangedScene += FadeInOut;
    }

    private void Start()
    {
        _fadePanel = GetComponentInChildren<Image>();
        FadeInOut("");
    }

    private void OnDestroy()
    {
        Foundation.TaskOnChangedScene -= FadeInOut;
    }

    private void FadeInOut(string obj)
    {
        _ = obj;

        // Fade In/Out
        _fadePanel.DOFade(1, _duration / 2.0f);
        _fadePanel.DOFade(0, _duration / 2.0f);
    }

    private void FadeOut(string obj)
    {
        _ = obj;

        // Fade In/Out
        _fadePanel.DOFade(0, _duration);
    }

    private void FadeIn(string obj)
    {
        _ = obj;

        // Fade In/Out
        _fadePanel.DOFade(1, _duration);
    }
}
