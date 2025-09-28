using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FridgeScene : MonoBehaviour
{
    public void SwitchScene2()
    {
        SceneManager.LoadSceneAsync(0); 
    }
}
