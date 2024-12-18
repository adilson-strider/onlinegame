using SocketIOClient.Newtonsoft.Json;
using SocketIOClient;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance;
    public string connectionURL;

    public SocketIOUnity socket;

    void Start()
    {
        var uri = new Uri(connectionURL);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };
        socket.OnPing += (sender, e) =>
        {
            Debug.Log("Ping");
        };
        socket.OnPong += (sender, e) =>
        {
            Debug.Log("Pong: " + e.TotalMilliseconds);
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"{System.DateTime.Now} Reconnecting: attempt = {e}");
        };
        Debug.Log("Connecting...");
        socket.Connect();

    }

    public void SearchBattle()
    {
        var searchBattleObj = new JObject
        {
            { "characterId", GameManager.Instance.chosenCharId },
            { "jwt", GameManager.Instance.jwtToken }
        };

        // Enviar o objeto 'searchBattleObj' com o evento 'searchBattle'
        socket.Emit("searchBattle", searchBattleObj);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
