using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class APIManager : MonoBehaviour
{
    [SerializeField] private string gasURL;

    [Header("UI References")]
    [SerializeField] private GameObject recipePopup; // The popup panel
    [SerializeField] private TMP_Text recipeText;    // TMP_Text inside popup
    [SerializeField] private TMP_InputField timeInput; // User enters time in minutes

    private string selectedDifficulty = "Easy"; // Default difficulty

    // Difficulty button hooks
    public void SetDifficultyEasy()   { selectedDifficulty = "Easy"; }
    public void SetDifficultyMedium() { selectedDifficulty = "Medium"; }
    public void SetDifficultyHard()   { selectedDifficulty = "Hard"; }

    // Called by Generate Recipes button
    public void OnGenerateRecipesClick()
    {
        StartCoroutine(SendDataToGAS());
    }

    private IEnumerator SendDataToGAS()
    {
        string foodList = string.Join(", ", FoodData.Instance.storedFoods);

        // Validate time input
        int timeLimit = 30; // default to 30 minutes
        if (timeInput != null && !string.IsNullOrWhiteSpace(timeInput.text))
        {
            if (int.TryParse(timeInput.text, out int parsedTime) && parsedTime > 0)
            {
                timeLimit = parsedTime;
            }
            else
            {
                Debug.LogWarning("Invalid time input, defaulting to 30 minutes.");
            }
        }

        // Build prompt
        string prompt =
            $"With the list of food [{foodList}], suggest 3 {selectedDifficulty} recipes " +
            $"that can be made in about {timeLimit} minutes. " +
            $"Keep the response under 100 words.";

        // Send request
        WWWForm form = new WWWForm();
        form.AddField("parameter", prompt);
        UnityWebRequest www = UnityWebRequest.Post(gasURL, form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            Debug.Log("Gemini Response:\n" + response);

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

    // Close button for popup
    public void ClosePopup()
    {
        if (recipePopup != null)
            recipePopup.SetActive(false);
    }
}
