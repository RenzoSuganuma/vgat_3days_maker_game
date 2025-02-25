using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class Foundation
{
    public const string TITLE_SCENE_NAME = "Title";
    public const string INGAME_SCENE_NAME = "InGame";
    public const string RESULT_SCENE_NAME = "Result";

    /// <summary>
    /// シーンロード時のイベント
    /// </summary>
    public static event Action<string> TaskOnLoadScene;

    public static event Action<string> TaskOnStartGame;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FadePanel()
    {
        SceneManager.LoadScene(TITLE_SCENE_NAME);
        var p = Resources.Load<GameObject>("pref_FadeCanvas");
        var obj = Object.Instantiate(p);
        Object.DontDestroyOnLoad(obj);
    }

    public static void StartGame()
    {
        StartGameAsync().Forget();
    }

    public static void EndGame()
    {
        EndGameAsync().Forget();
    }

    private static async UniTask StartGameAsync()
    {
        await LoadSceneAdditiveAsync(INGAME_SCENE_NAME);
        await LoadSceneAdditiveAsync(RESULT_SCENE_NAME);

        // Activate InGame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(INGAME_SCENE_NAME));

        DisposeScene(TITLE_SCENE_NAME);
    }

    private static async UniTask EndGameAsync()
    {
        // Activate InGame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RESULT_SCENE_NAME));

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
