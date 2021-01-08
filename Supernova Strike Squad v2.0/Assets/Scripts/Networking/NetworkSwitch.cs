using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkLobbyType
{
    SteamLobby = 0, 
    LocalLobby = 1
}

public class NetworkSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject SteamLobby;

    [SerializeField]
    private GameObject LocalLobby;


    public NetworkLobbyType LobbyType;

    void Awake()
    {
    }

    public void GenerateNetworkController()
    {
        Instantiate((LobbyType == NetworkLobbyType.SteamLobby ? SteamLobby : LocalLobby));
    }
}
