using System;
using UnityEngine;

public enum EChatType
{
    Mine,
    Other,
    System,
}

public class ChatMessage 
{
    public readonly EChatType Type;
    public readonly string Sender;
    public readonly string Message;
    public readonly DateTime Time;

    private ChatMessage(EChatType type, string sender, string message)
    {
        Type = type;
        Sender = sender;
        Message = message;
        Time = DateTime.Now;
    }

    // 객체의 생성을 캡슐화
    public static ChatMessage CreateMine(string sender, string message)
    {
        return new ChatMessage(EChatType.Mine, sender, message);
    }

    public static ChatMessage CreateOther(string sender, string message)
    {
        return new ChatMessage(EChatType.Other, sender, message);
    }

    public static ChatMessage CreateSystem(string message)
    {
        return new ChatMessage(EChatType.System, "", message);
    }
}
