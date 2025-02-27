using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanelImpl : MonoBehaviour
{
    [SerializeField] private CanvasGroup _hideOnScneChange;
    [SerializeField] private List<GameObject> _ddols = new();
    [SerializeField] private Button _buttonStart;

    private bool _test;

    private void Start()
    {
        FindAnyObjectByType<VoiceInputHandler>().IsCorrectVoice.AsObservable().Subscribe(b =>
        {
            var hoge = FindAnyObjectByType<VoiceInputHandler>();
            if (b && hoge.RecognizedText.Value == "スタート")
                FindAnyObjectByType<SceneLoaderImpl>().StartGame();
        });

        foreach (var ddol in _ddols)
        {
            DontDestroyOnLoad(ddol);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindAnyObjectByType<VoiceInputHandler>().StopSpeechRecognition();
            FindAnyObjectByType<VoiceInputHandler>().StartSpeechRecognition();

            _test = true;
        }

        if (_test && Input.GetKeyUp(KeyCode.Space))
        {
            _test = false;
            FindAnyObjectByType<VoiceInputHandler>().StopSpeechRecognition();
        }
    }
}
