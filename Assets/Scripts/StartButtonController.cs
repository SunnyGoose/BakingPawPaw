using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnStartClick() {
        SceneManager.LoadScene(0);
    }
}