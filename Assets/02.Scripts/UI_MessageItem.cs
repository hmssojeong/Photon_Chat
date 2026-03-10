using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MessageItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _userNameText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private TMP_Text _timeText;

    private const float MAX_BUBBLE_WIDTH = 350f;

    public void Setup(ChatMessage message)
    {
        if (_userNameText != null) _userNameText.text = message.Sender;
        if (_timeText != null) _timeText.text = message.Time.ToString("tt h:mm");

        if (_messageText != null)
        {
            _messageText.text = message.Message;

            _messageText.textWrappingMode = TextWrappingModes.NoWrap;
            _messageText.ForceMeshUpdate();
            float naturalWidth = _messageText.preferredWidth;

            LayoutElement layoutElement = _messageText.GetComponent<LayoutElement>();
            if (layoutElement == null)
                layoutElement = _messageText.gameObject.AddComponent<LayoutElement>();

            if (naturalWidth > MAX_BUBBLE_WIDTH)
            {
                _messageText.textWrappingMode = TextWrappingModes.Normal;
                layoutElement.preferredWidth = MAX_BUBBLE_WIDTH;
            }
            else
            {
                _messageText.textWrappingMode = TextWrappingModes.NoWrap;
                layoutElement.preferredWidth = naturalWidth;
            }
        }
    }
}