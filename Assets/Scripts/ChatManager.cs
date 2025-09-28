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
    public string gasUrl = "https://script.google.com/macros/s/AKfycbxaNoyB8ROfygfT_cAYvixj-i4JalWCp70C7lwhm23wyAmOLuhMWz7o2xXBI2MEfBKBnw/exec";
    
    private List<ChatMessage> chatHistory = new List<ChatMessage>();

    void Start()
    {
        if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);
        
        messageInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        
        AddAIMessage("Meow~! I’m your baking pawpaw, ready to knead up answers and whisk away your worries. What shall we bake together today, nya? ");
    }

    void OnInputFieldEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessage();
            messageInputField.ActivateInputField();
        }
    }

    public void SendMessage()
    {
        string message = messageInputField.text.Trim();
        
        if (string.IsNullOrEmpty(message))
            return;

        AddUserMessage(message);
        messageInputField.text = "";
        
        StartCoroutine(SendToAI(message));
    }

    void AddUserMessage(string message)
    {
        if (userMessagePrefab == null || chatContent == null)
        {
            Debug.LogError("User Message Prefab or Chat Content is not assigned!");
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

        StartCoroutine(ScrollToBottomNextFrame());
    }

    void AddAIMessage(string message)
    {
        if (aiMessagePrefab == null || chatContent == null)
        {
            Debug.LogError("AI Message Prefab or Chat Content is not assigned!");
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

        StartCoroutine(ScrollToBottomNextFrame());
    }

    IEnumerator SendToAI(string message)
    {
        AddAIMessage("Typing...");
        GameObject typingMsg = chatContent.GetChild(chatContent.childCount - 1).gameObject;

        WWWForm form = new WWWForm();
        form.AddField("parameter", message);

        using (UnityWebRequest request = UnityWebRequest.Post(gasUrl, form))
        {
            yield return request.SendWebRequest();

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

    IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // wait one frame so layout updates
        Canvas.ForceUpdateCanvases();
        if (chatScrollView != null)
        {
            chatScrollView.verticalNormalizedPosition = 0f;
        }
    }

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

