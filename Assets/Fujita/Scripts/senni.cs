using UnityEngine;
using UnityEngine.SceneManagement;

public class senni : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("InGame");
    }
}
