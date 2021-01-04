using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkLobbyType
{
    SteamLobby = 0, 
    LocalLobby = 1
}

public class NotworkSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject SteamLobby;

    [SerializeField]
    private GameObject LocalLobby;


    public NetworkLobbyType LobbyType;

    void Awake()
    {
        Instantiate((LobbyType == NetworkLobbyType.SteamLobby ? SteamLobby : LocalLobby));
    }
}
