using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotoAlbumManager : MonoBehaviour
{
    [Header("Scene Navigation")]
    public string cameraSceneName = "CameraScene"; // Assuming your camera scene is named this

    [Header("UI Elements")]
    public Button backButton;

    [Tooltip("The UI container where photo thumbnails will be created. This should have a GridLayoutGroup component.")]
    public RectTransform photoContainer;

    [Tooltip("The prefab for displaying each photo. This should have a RawImage component.")]
    public GameObject photoPrefab;

    void Start()
    {
        if (photoContainer == null || photoPrefab == null)
        {
            Debug.LogError("Photo Container or Photo Prefab is not assigned in the inspector!");
            return;
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(GoToCameraScene);
        }

        LoadPhotos();
    }

    void LoadPhotos()
    {
        // Get all .png files from the persistent data path
        string[] photoPaths = Directory.GetFiles(Application.persistentDataPath, "*.png");

        if (photoPaths.Length == 0)
        {
            Debug.Log("No photos found in " + Application.persistentDataPath);
            // Optionally, display a "No Photos" message to the user here.
            return;
        }

        foreach (string path in photoPaths)
        {
            // Create a new instance of the photo prefab
            GameObject photoInstance = Instantiate(photoPrefab, photoContainer);

            // Load the image data from the file
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2); // Create a temporary texture
            
            // Load the image data into the texture
            if (texture.LoadImage(fileData)) // LoadImage returns true if successful
            {
                RawImage rawImage = photoInstance.GetComponent<RawImage>();
                if (rawImage != null)
                {
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogError("Photo Prefab does not have a RawImage component!");
                }
            }
            else
            {
                Debug.LogError("Failed to load image at path: " + path);
                Destroy(photoInstance); // Clean up the failed instance
            }
        }
    }

    void GoToCameraScene()
    {
        SceneManager.LoadScene(cameraSceneName);
    }
}
