using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class APIManager : MonoBehaviour
{
    [SerializeField] private string gasURL;
    [SerializeField] private GameObject recipePopup; // Assign your popup panel
    [SerializeField] private TMP_Text recipeText;    // Assign TMP_Text inside popup

    // Call this from your "Generate Recipes" button
    public void OnGenerateRecipesClick()
    {
        StartCoroutine(SendDataToGAS());
    }

    private IEnumerator SendDataToGAS()
    {
        string foodList = string.Join(", ", FoodData.Instance.storedFoods); // Use FoodData

        string prompt = $"With the list of food [{foodList}] give me ideas for foods I can make, but only list 3 items and make your response under 100 words.";

        WWWForm form = new WWWForm();
        form.AddField("parameter", prompt);
        UnityWebRequest www = UnityWebRequest.Post(gasURL, form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            Debug.Log("Gemini Response:\n" + response);

            // Show the popup with the response
            if (recipePopup != null && recipeText != null)
            {
                recipeText.text = response;
                recipePopup.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Popup or TMP_Text not assigned in Inspector!");
            }
        }
        else
        {
            Debug.LogError("Error from GAS/Gemini: " + www.error);
        }
    }

    // Optional: call this from the Close button on the popup
    public void ClosePopup()
    {
        if (recipePopup != null)
            recipePopup.SetActive(false);
    }
}
