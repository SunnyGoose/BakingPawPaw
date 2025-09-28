using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Kitchen : MonoBehaviour
{
    public void SwitchScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void SwitchSceneToCab()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void SwitchSceneToPAPA()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void SwitchSceneToPcam()
    {
        SceneManager.LoadSceneAsync(6); 
    }
}
