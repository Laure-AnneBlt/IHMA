using UnityEngine;
using Photon.Pun;

public class PhotonInitializer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // Connect to Photon using the app's settings
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        // Join or create a room
        PhotonNetwork.JoinOrCreateRoom("ScreenShareRoom", new Photon.Realtime.RoomOptions { MaxPlayers = 10 }, Photon.Realtime.TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
    }
}
