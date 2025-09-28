using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public ScrollRect chatScrollView;
    public Transform chatContent;
    public TMP_InputField messageInputField;
    public Button sendButton;
    
    [Header("Message Prefabs")]
    public GameObject userMessagePrefab;
    public GameObject aiMessagePrefab;
    
    [Header("Settings")]
    public string gasUrl = "https://script.google.com/macros/s/AKfycbycWkxgzskgbRUUk6XCujtYT82rygkuriOCZOUy-W8R_8b8nIJvO0HWvpnbfwoQW6X3zg/exec";
    
    private List<ChatMessage> chatHistory = new List<ChatMessage>();

    void Start()
    {
        // Setup button click (optional - you can remove this if you don't want a send button)
        if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);
        
        // Setup enter key for input field
        messageInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        
        // Add welcome message
        AddAIMessage("Meow~! Welcome to my cozy bakery. I’m your friendly cat assistant, kneading up answers as warm and fluffy as fresh bread. What can I whisk up for you today, nya? ");
    }

    void OnInputFieldEndEdit(string text)
    {
        // Check if Enter key was pressed (not Escape or Tab)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessage();
            // Keep the input field selected for continuous typing
            messageInputField.ActivateInputField();
        }
    }

    public void SendMessage()
    {
        string message = messageInputField.text.Trim();
        
        if (string.IsNullOrEmpty(message))
            return;

        // Add user message to chat
        AddUserMessage(message);
        
        // Clear input field
        messageInputField.text = "";
        
        // Send to AI
        StartCoroutine(SendToAI(message));
    }

    void AddUserMessage(string message)
    {
        // Debug what's missing
        if (userMessagePrefab == null)
        {
            Debug.LogError("User Message Prefab is not assigned in ChatManager!");
            return;
        }
        
        if (chatContent == null)
        {
            Debug.LogError("Chat Content is not assigned in ChatManager!");
            return;
        }
        
        GameObject messageObj = Instantiate(userMessagePrefab, chatContent);
        TextMeshProUGUI messageText = messageObj.GetComponentInChildren<TextMeshProUGUI>();
        
        if (messageText == null)
        {
            Debug.LogError("User Message Prefab doesn't have a TextMeshProUGUI component!");
            return;
        }
        
        messageText.text = message;
        
        chatHistory.Add(new ChatMessage { isUser = true, message = message });
        
        // Scroll to bottom
        Canvas.ForceUpdateCanvases();
        
        if (chatScrollView == null)
        {
            Debug.LogError("Chat Scroll View is not assigned in ChatManager!");
            return;
        }
        
        chatScrollView.verticalNormalizedPosition = 0f;
    }

    void AddAIMessage(string message)
    {
        // Debug what's missing
        if (aiMessagePrefab == null)
        {
            Debug.LogError("AI Message Prefab is not assigned in ChatManager!");
            return;
        }
        
        if (chatContent == null)
        {
            Debug.LogError("Chat Content is not assigned in ChatManager!");
            return;
        }
        
        GameObject messageObj = Instantiate(aiMessagePrefab, chatContent);
        TextMeshProUGUI messageText = messageObj.GetComponentInChildren<TextMeshProUGUI>();
        
        if (messageText == null)
        {
            Debug.LogError("AI Message Prefab doesn't have a TextMeshProUGUI component!");
            return;
        }
        
        messageText.text = message;
        
        chatHistory.Add(new ChatMessage { isUser = false, message = message });
        
        // Scroll to bottom
        Canvas.ForceUpdateCanvases();
        chatScrollView.verticalNormalizedPosition = 0f;
    }

    IEnumerator SendToAI(string message)
    {
        // Show typing indicator (optional)
        AddAIMessage("Meow...");
        GameObject typingMsg = chatContent.GetChild(chatContent.childCount - 1).gameObject;

        // Create form data
        WWWForm form = new WWWForm();
        form.AddField("parameter", message);

        using (UnityWebRequest request = UnityWebRequest.Post(gasUrl, form))
        {
            yield return request.SendWebRequest();

            // Remove typing indicator
            DestroyImmediate(typingMsg);
            chatHistory.RemoveAt(chatHistory.Count - 1);

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                Debug.Log("AI Response: " + response);
                AddAIMessage(response);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                AddAIMessage("Sorry, I'm having trouble connecting. Please try again.");
            }
        }
    }

    // Optional: Clear chat history
    public void ClearChat()
    {
        foreach (Transform child in chatContent)
        {
            Destroy(child.gameObject);
        }
        chatHistory.Clear();
        AddAIMessage("Chat cleared. How can I help you?");
    }
}

[System.Serializable]
public class ChatMessage
{
    public bool isUser;
    public string message;
}