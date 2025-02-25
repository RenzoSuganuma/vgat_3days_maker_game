using UnityEngine;
using UnityEngine.SceneManagement;

public static class Foundation
{
    public const string TITLE_SCENE_NAME = "Title";
    public const string INGAME_SCENE_NAME = "InGame";
    public const string RESULT_SCENE_NAME = "Result";

    public static void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static AsyncOperation DisposeScene(string sceneName)
    {
        return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
    }
}
