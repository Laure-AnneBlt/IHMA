using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VideoReceiver : MonoBehaviourPun, IPunObservable
{
    public Renderer displayRenderer; // Assign the renderer (e.g., Plane or UI RawImage)
    private Texture2D receivedTexture;

    void Start()
    {
        // Initialize texture for received frames
        receivedTexture = new Texture2D(2, 2); // Will be resized as needed
    }

    [PunRPC]
    public void ReceiveVideoFrame(byte[] frameData)
    {
        // Decode the received frame
        receivedTexture.LoadImage(frameData);

        // Apply the texture to the renderer's material
        if (displayRenderer != null)
        {
            displayRenderer.material.mainTexture = receivedTexture;
        }
    }

    // IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // No need to sync anything here since the data transfer is handled via RPCs
        // Leave this method empty
    }
}
