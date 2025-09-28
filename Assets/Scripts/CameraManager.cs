using UnityEngine;
using UnityEngine.UI;


public class CameraManager : MonoBehaviour
{
    WebCamTexture webcam;
    public RawImage img;
    public Button cameraButton; // Assign this in the Inspector

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
