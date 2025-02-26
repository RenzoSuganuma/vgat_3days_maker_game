using UnityEngine;

public sealed class SceneLoaderImpl : MonoBehaviour
{
    public static SceneLoaderImpl Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void StartGame()
    {
        Foundation.StartGame();
    }

    public void EndGame()
    {
        Foundation.EndGame();
    }
}
