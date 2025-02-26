using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class TitlePanelImpl : MonoBehaviour
{
    [SerializeField] private CanvasGroup _hideOnScneChange;
    [SerializeField] private List<GameObject> _ddols = new();

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

        Foundation.TaskOnChangedScene += s =>
        {
            if (s is Foundation.INGAME_SCENE_NAME)
            {
                if (_hideOnScneChange != null)
                {
                    _hideOnScneChange.alpha = 0;
                }

                FindAnyObjectByType<MissionsDisplay>().GetComponent<CanvasGroup>().alpha = 1;
            }
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindAnyObjectByType<VoiceInputHandler>().StartSpeechRecognition();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            FindAnyObjectByType<VoiceInputHandler>().StopSpeechRecognition();
        }
    }
}
