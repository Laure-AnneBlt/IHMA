using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // Ensures scene sync across players
        PhotonNetwork.ConnectUsingSettings(); // Connects to the Photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");
        PhotonNetwork.JoinRandomRoom(); // Attempts to join a random room
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        PhotonNetwork.LoadLevel("InitScene"); // Replace with your actual scene name
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room available, creating a new one");
        PhotonNetwork.CreateRoom(null); // Creates a new room if joining fails
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room!");
    }
}
