using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanelImpl : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<SceneLoaderImpl>().StartGame();
        }
    }
}
