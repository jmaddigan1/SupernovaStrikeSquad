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
    private GameObject SteamLobby = null;

    [SerializeField]
    private GameObject LocalLobby = null;


    public NetworkLobbyType LobbyType;

    void Awake()
    {
        Instantiate((LobbyType == NetworkLobbyType.SteamLobby ? SteamLobby : LocalLobby));
    }

    public void GenerateNetworkController()
    {
    }
}
