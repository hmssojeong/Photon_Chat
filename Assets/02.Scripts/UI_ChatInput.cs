using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField _messageInputField;
    [SerializeField] private Button _sendButton;

    private void Start()
    {
        _sendButton.onClick.AddListener(SendMessage);
    }
    public void SendMessage()
    {
        string message = _messageInputField.text;
        if (string.IsNullOrEmpty(message)) return;
        
        ChatManager.Instance.SendChatMessage(message);
        
        _messageInputField.text = string.Empty;
    }
}
