using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatMessages : MonoBehaviour
{
    [SerializeField] private GameObject _myMessagePrefab;
    [SerializeField] private GameObject _otherMessagePrefab;
    [SerializeField] private GameObject _systemMessagePrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private ScrollRect _scrollRect;

    private void Start()
    {
        ChatManager.OnNewMessage += OnNewMessage;
    }

    private void OnDestroy()
    {
        ChatManager.OnNewMessage -= OnNewMessage;
    }

    private void OnNewMessage(ChatMessage message)
    {

        GameObject prefab = message.Type switch
        {
            EChatType.Mine => _myMessagePrefab,
            EChatType.Other => _otherMessagePrefab,
            EChatType.System => _systemMessagePrefab,
            _ => null,
        };

        if (prefab == null) return;

        GameObject go = Instantiate(prefab, _content);
        go.GetComponent<UI_MessageItem>().Setup(message);

        // 스크롤 뷰를 가장 하단으로
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

}
