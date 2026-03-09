using TMPro;
using UnityEngine;

public class UI_RoomInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _userCountText;

    private void Start()
    {
        ChatManager.OnUserCountChanged += OnUserCountChanged;
    }

    private void OnDestroy()
    {
        ChatManager.OnUserCountChanged -= OnUserCountChanged;
    }

    private void OnUserCountChanged(int count)
    {
        if (_userCountText != null)
            _userCountText.text = $"{count}";
    }
}