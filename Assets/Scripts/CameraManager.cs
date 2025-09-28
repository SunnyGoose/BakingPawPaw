using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class CameraManager : MonoBehaviour
{
    [Header("Scene Navigation")]
    public string photoAlbumSceneName = "PhotoAlbumScene";
    WebCamTexture webcam;
    public RawImage img;
    public Button takePictureButton;


    void Awake()
    {
        if (takePictureButton != null)
        {
            takePictureButton.onClick.AddListener(TakePicture);
        }
    }

    void TakePicture()
    {
        if (webcam != null && webcam.isPlaying)
        {
            Texture2D photo = new Texture2D(webcam.width, webcam.height);
            photo.SetPixels(webcam.GetPixels());
            photo.Apply();

            // Encode to PNG
            byte[] bytes = photo.EncodeToPNG();
            Destroy(photo);

            // Save to file
            string filename = System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            string path = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllBytes(path, bytes);
            Debug.Log("Photo saved to: " + path);
            Debug.Log("Picture taken!");
        }            

        else
        {
            Debug.LogWarning("Webcam is not active or playing.");
        }
    }
    public Button cameraButton;


    [Header("UI Buttons")]
    public Button albumButton;

    void Start()
    {
        if (cameraButton != null)
        {
            cameraButton.onClick.AddListener(OpenCamera);
        }
        if (albumButton != null)
        {
            albumButton.onClick.AddListener(GoToAlbumScene);
        }
    }

    public void OpenCamera()
    {
        webcam = new WebCamTexture();
        img.texture = webcam;
        webcam.Play();
    }

    public void GoToAlbumScene()
    {
        // It's good practice to stop the webcam before leaving the scene
        if (webcam != null && webcam.isPlaying)
        {
            webcam.Stop();
        }
        SceneManager.LoadScene(photoAlbumSceneName);
    }
}
