using UnityEngine;

public sealed class SceneLoaderImpl : MonoBehaviour
{
    public void StartGame()
    {
        Foundation.StartGame();
    }

    public void EndGame()
    {
        Foundation.EndGame();
    }
}
