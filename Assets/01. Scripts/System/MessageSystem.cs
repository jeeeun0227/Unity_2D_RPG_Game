using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem
{
    // Singleton
    static MessageSystem _instance;
    public static MessageSystem Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new MessageSystem();
                _instance.Init();
            }
            return _instance;
        }
    }

    void Init()
    {
    }

    // Message

    Queue<MessageParam> _messageQueue = new Queue<MessageParam>();

    public void Send(MessageParam msgParam)
    {
        _messageQueue.Enqueue(msgParam);
    }

    public void ProcessMessage()
    {
        while(0 != _messageQueue.Count)
        {
            MessageParam msgParam = _messageQueue.Dequeue();
            msgParam.receiver.ReceiveObjectMessage(msgParam);
        }
    }
}
