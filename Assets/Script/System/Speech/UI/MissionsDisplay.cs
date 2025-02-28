﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MissionsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _maxDbText;
    [SerializeField] private TMP_Text _wordsText;
    [SerializeField] private TMP_Text _playerText;
    [SerializeField] private TMP_Text _distanceScoreText;
    [SerializeField] private TMP_Text _heightScoreText;
    [SerializeField] private TMP_Text _timeScoreText;
    [SerializeField] private TMP_Text _nextText;

    [SerializeField] private GameObject _successImage;
    [SerializeField] private GameObject _failImage;

    [SerializeField] private GameObject _messagePanel;
    [SerializeField] List<Sprite> _messageSprites = new List<Sprite>();

    private Color _defaultTextColor;

    public TMP_Text NextText => _nextText;

    private void Awake()
    {
        // 初期化
        _maxDbText.text = "dB";
        _wordsText.text = "";
        _playerText.text = "";
        _distanceScoreText.text = "距離: 0000m";
        _heightScoreText.text = "高さ: 0000m";
        _timeScoreText.text = "時間: 00s";
        _nextText.text = "Next";

        _successImage.SetActive(false);
        _failImage.SetActive(false);
        _messagePanel.SetActive(false);

        _defaultTextColor = _wordsText.color;
    }

    /// <summary>
    /// 最大音量を表示
    /// </summary>
    public void SetMaxDbText(float maxDb)
    {
        _maxDbText.text = $"最大音量:\n {maxDb:F2} dB";
    }

    /// <summary>
    /// 読み上げるフレーズを表示
    /// </summary>
    public void SetMissionText(string phrase)
    {
        _wordsText.text = $"{phrase}";

        var c = _wordsText.color;
        c.a = 1;
        _wordsText.color = c;

        _successImage.SetActive(false);
        _failImage.SetActive(false);
    }


    /// <summary>
    /// プレイヤーの発音を表示
    /// </summary>
    public void SetPlayerText(string text)
    {
        _playerText.text = text;
    }

    /// <summary>
    /// 次のミッションのフレーズを表示
    /// </summary>
    public void SetNextText(string text)
    {
         _nextText.text = text;
    }

    /// <summary>
    /// 距離スコアを設定
    /// </summary>
    public void SetDistanceScore(float distance)
    {
        _distanceScoreText.text = $"距離: {distance}m";
    }


    /// <summary>
    /// 高さスコアを設定
    /// </summary>
    public void SetHeightScore(float height)
    {
        _heightScoreText.text = $"高さ: {height}m";
    }

    /// <summary>
    /// 時間スコアを設定
    /// </summary>
    public void SetTimeScore(string time)
    {
        _timeScoreText.text = "時間: " + time + "s";
    }

    /// <summary>
    /// 正しい発音をした場合の UI アニメーション
    /// </summary>
    public void MissionSuccess()
    {
        Debug.Log("正解！");

        _successImage.SetActive(true);
        _failImage.SetActive(false);

        // フェードイン・アウトアニメーション
        _wordsText.DOFade(1f, 0.3f).From(0f).SetLoops(2, LoopType.Yoyo);
    }

    /// <summary>
    /// 間違った発音をした場合の UI アニメーション
    /// </summary>
    public void MissionFail()
    {
        Debug.Log("❌ 間違い！");

        _failImage.SetActive(true);
        _successImage.SetActive(false);

        // 赤く点滅するアニメーション
        _wordsText.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            _wordsText.color = _defaultTextColor; // 元の色に戻す
        });
    }

    /// <summary>
    /// メッセージパネルを表示する場合のUIアニメーション
    /// </summary>
    public void ShowMessagePanel()
    {
        _messagePanel.SetActive(true);
        _messagePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        // 3秒後に非表示にする
        DOVirtual.DelayedCall(3f,
            () =>
            {
                _messagePanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    _messagePanel.SetActive(false);
                });
            });
    }

    /// <summary>
    /// メッセージパネルの画像を設定する
    /// </summary>
    public void SetMessagePanel(VoiceNameEnum index)
    {
        _messagePanel.GetComponent<Image>().sprite = _messageSprites[(int)index];
    }
}
