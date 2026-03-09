using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener   // 채팅 이벤트를 수신하는 옵저버 인터페이스
{
    public static ChatManager Instance {get; private set;}
    private ChatClient _chatClient;
    private const string CHANNEL_SKKU = "skku2";
    private const string CHANNEL_NOTICE = "notice";
    private const string NickName = "용진";
    private List<ChatMessage> _messages;
    
    // 새로운 챗 메시지가 수신되면 실행되는 이벤트
    public static event Action<ChatMessage> OnNewMessage;
    // 채널 인원 수가 변경되면 실행되는 이벤트
    public static event Action<int> OnUserCountChanged;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _chatClient = new ChatClient(this);
        _chatClient.ChatRegion = "ASIA";
        var auth = new AuthenticationValues(NickName);
        _chatClient.Connect("8952ee8f-6b2e-45c9-9cdb-b60a2f906585", "1.0", auth);
    }

    private void Update()
    {
        // ChatClient는 MonoBehaviour가 아니므로, 매 프레임 서비스 펌프를 호출해줘야
        // 네트워크 메시지가 처리되고, 아래 콜백 메서드들이 실행된다
        _chatClient.Service();
    }

    // IChatClientListener는 11개의 콜백으로 채팅 이벤트를 처리
    // 1. 연결 상태 변환
    public void OnConnected()
    {
        Debug.Log("[Photon Chat] 서버에 연결됐습니다");

        var options = new ChannelCreationOptions { PublishSubscribers = true };
        _chatClient.Subscribe(CHANNEL_SKKU, 0, -1, options);
        _chatClient.Subscribe(CHANNEL_NOTICE, 0, -1, options);
    }

    public void OnDisconnected()
    {
        Debug.Log("[Photon Chat] 서버에 연결 해제됐습니다");
    }

    public void OnChatStateChange(ChatState state)
    {
        // 스피너 같은 로딩창을 띄워주는 용도
        
        Debug.Log($"[Photon Chat] 상태 변경 ▶ {state}");
    }

    // 2. 채널 입장/퇴장
    
    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log($"[Photon Chat]채널 {channels[i]} 구독 ({results[i]})");
        }
        
        foreach (var channel in _chatClient.PublicChannels)
        {
            // 여기서 내가 구동중인 채널 목록을 알 수 있다.
        }

        BroadcastUserCount();
    }
    
    public void OnUnsubscribed(string[] channels)
    {
        foreach (string channel in channels)
        {
            Debug.Log($"[Photon Chat]채널 {channels} 구독해지");
        }
    }

    // 3. 다른 유저의 온라인 상태
    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"[Photon Chat]채널 {channel}에 {user} 입장");
        if (channel == CHANNEL_SKKU)
        {
            OnNewMessage?.Invoke(ChatMessage.CreateSystem($"{user} 님이 입장했습니다"));
            BroadcastUserCount();
        }
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"[Photon Chat]채널 {channel}에 {user} 퇴장");
        if (channel == CHANNEL_SKKU)
        {
            OnNewMessage?.Invoke(ChatMessage.CreateSystem($"{user} 님이 퇴장했습니다"));
            BroadcastUserCount();
        }
    }
    // 친구/팔로우 리스트 중 특정 유저가 상태 변경 시
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }
    
    // 4. 메시지 수신
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        // 포톤챗이 네트워크 최적화를 위해 같은 프레임에 여러 개의 메시지를 받으면
        // 매 번 함수를 호출을 하는게 아니라 배열을 묶어서 한번에 전달하기도 한다
        for (int i = 0; i < messages.Length; i++)
        {
            Debug.Log($"[Photon Chat]채널 [{channelName}] {senders[i]}: {messages[i]}");

            if (senders[i] == NickName)
            {
                OnNewMessage?.Invoke(ChatMessage.CreateMine(senders[i], messages[i].ToString()));
            }
            else
            {
                OnNewMessage?.Invoke(ChatMessage.CreateOther(senders[i], messages[i].ToString()));
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // 귓속말
        throw new System.NotImplementedException();
    }
    
    // 포톤책 내부에서 디버그 로그가 발생할 때 호출된다
    // level에서 지정한 심각도 이상만 들어오며, 개발 단계에서 로그 확인용이다
    public void DebugReturn(DebugLevel level, string message)
    {
        switch (level)
        {
            case DebugLevel.ERROR:
                Debug.LogError($"[Photon Error] : {message}");
                break;
            case DebugLevel.WARNING:
                Debug.LogWarning($"[Photon Warning] : {message}");
                break;
            default:
                Debug.Log($"[Photon Info] : {message}");
                break;
        }
    }

    private void BroadcastUserCount()
    {
        if (_chatClient.TryGetChannel(CHANNEL_SKKU, out ChatChannel channel))
        {
            OnUserCountChanged?.Invoke(channel.Subscribers.Count);
        }
    }

    public void SendChatMessage(string message)
    {
        if (_chatClient == null) return;
        if (_chatClient.CanChat == false) return;

        _chatClient.PublishMessage(CHANNEL_SKKU, message);
    }

}
