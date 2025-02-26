using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class Foundation
{
    public const string TITLE_SCENE_NAME = "Title";
    public const string INGAME_SCENE_NAME = "InGame";
    public const string RESULT_SCENE_NAME = "Result";

    public static string CurrentScene => SceneManager.GetActiveScene().name;

    public static List<GameObject>[] InGameLane { get; private set; }

    /// <summary>
    /// シーンロード時のイベント
    /// </summary>
    public static event Action<string> TaskOnLoadScene;

    public static event Action<string> TaskOnStartGame;
    public static event Action<string> TaskOnChangedScene;

    public static event Action OnGameOver;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FadePanel()
    {
        #if DONT_LOAD_TITLE_SCENE
        return;
        #endif

        SceneManager.LoadScene(TITLE_SCENE_NAME);

        var p = Resources.Load<GameObject>("pref_FadeCanvas");
        var obj = Object.Instantiate(p);
        Object.DontDestroyOnLoad(obj);

        var sl = Object.Instantiate(new GameObject());
        sl.name = nameof(SceneLoaderImpl);
        sl.AddComponent<SceneLoaderImpl>();
        Object.DontDestroyOnLoad(sl);
    }

    public static void StartGame()
    {
        StartGameAsync().Forget();
    }

    public static void EndGame()
    {
        EndGameAsync().Forget();
    }

    public static void NotifyGameOver()
    {
        OnGameOver?.Invoke();
    }

private static async UniTask StartGameAsync()
    {
        // 配列確保
        InGameLane = new List<GameObject>[6];
        for (int i = 0; i < InGameLane.Length; i++)
        {
            InGameLane[i] = new List<GameObject>();
        }

        await LoadSceneAdditiveAsync(INGAME_SCENE_NAME);
        await LoadSceneAdditiveAsync(RESULT_SCENE_NAME);

        // Activate InGame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(INGAME_SCENE_NAME));

        TaskOnChangedScene?.Invoke(INGAME_SCENE_NAME);

        DisposeScene(TITLE_SCENE_NAME);
    }

    private static async UniTask EndGameAsync()
    {
        // Activate InGame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RESULT_SCENE_NAME));

        TaskOnChangedScene?.Invoke(RESULT_SCENE_NAME);

        InGameLane = Array.Empty<List<GameObject>>();
        DisposeScene(INGAME_SCENE_NAME);
    }

    private static void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        TaskOnLoadScene?.Invoke(sceneName);
    }

    private static async UniTask LoadSceneAdditiveAsync(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        TaskOnLoadScene?.Invoke(sceneName);
    }

    private static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        TaskOnLoadScene?.Invoke(sceneName);
    }

    private static AsyncOperation DisposeScene(string sceneName)
    {
        return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
    }
}
