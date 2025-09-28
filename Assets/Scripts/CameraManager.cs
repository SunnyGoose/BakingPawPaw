using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraManager : MonoBehaviour
{
    WebCamTexture webcam;
    public RawImage img;
    public Button cameraButton;

    void Start()
    {
        cameraButton.onClick.AddListener(OpenCamera);
    }

    public void OpenCamera()
    {
        webcam = new WebCamTexture();
        img.texture = webcam;
        webcam.Play();
    }
}
