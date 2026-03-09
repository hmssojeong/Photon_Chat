using TMPro;
using UnityEngine;

public class UI_MessageItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _userNameText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private TMP_Text _timeText;

    public void Setup(ChatMessage message)
    {
        if (_userNameText != null) _userNameText.text = message.Sender;
        if (_messageText != null) _messageText.text = message.Message;
        if (_timeText != null) _timeText.text = message.Time.ToString("tt h:mm");
    }
}