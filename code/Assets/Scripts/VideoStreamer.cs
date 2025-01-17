using UnityEngine;
using Photon.Pun;

public class VideoStreamer : MonoBehaviourPun, IPunObservable
{
    public Camera videoCamera; // Assign the camera to capture video
    private RenderTexture renderTexture;
    private Texture2D frameTexture;
    private const int FrameWidth = 256;
    private const int FrameHeight = 256;

    void Start()
    {
        // Initialize RenderTexture and Texture2D
        renderTexture = new RenderTexture(FrameWidth, FrameHeight, 24);
        videoCamera.targetTexture = renderTexture;
        frameTexture = new Texture2D(FrameWidth, FrameHeight, TextureFormat.RGB24, false);
    }

    void Update()
    {
        // Capture and send frames at regular intervals
        if (Time.frameCount % 5 == 0)
        {
            CaptureAndSendFrame();
        }
    }

    private void CaptureAndSendFrame()
    {
        // Capture frame
        RenderTexture.active = renderTexture;
        frameTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        frameTexture.Apply();
        RenderTexture.active = null;

        // Encode to JPG
        byte[] frameData = frameTexture.EncodeToJPG(50);

        // Send via RPC
        photonView.RPC("ReceiveVideoFrame", RpcTarget.Others, frameData);
    }

    // IPunObservable implementation (required for PhotonView observation)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // No state synchronization required since frames are sent via RPC
        // Leave this empty or add any relevant state sync if needed
    }
}



//using UnityEngine;
//using Photon.Pun;

//public class VideoStreamer : MonoBehaviourPun
//{
//    public Camera videoCamera;
//    private RenderTexture renderTexture;
//    private Texture2D frameTexture;
//    private const int FrameWidth = 512; // Adjusted resolution
//    private const int FrameHeight = 512;

//    void Start()
//    {
//        renderTexture = new RenderTexture(FrameWidth, FrameHeight, 24);
//        videoCamera.targetTexture = renderTexture;
//        frameTexture = new Texture2D(FrameWidth, FrameHeight, TextureFormat.RGB24, false);
//    }

//    void Update()
//    {
//        // Send frames every 10 frames
//        if (Time.frameCount % 10 == 0)
//        {
//            CaptureAndSendFrame();
//        }
//    }

//    private void CaptureAndSendFrame()
//    {
//        if (videoCamera == null)
//        {
//            Debug.LogError("VideoCamera is not assigned!");
//            return;
//        }

//        // Capture frame
//        RenderTexture.active = renderTexture;
//        frameTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
//        RotateTexture180(frameTexture); // Fix 180-degree rotation
//        frameTexture.Apply();
//        RenderTexture.active = null;

//        // Encode to JPG with high quality
//        byte[] frameData = frameTexture.EncodeToJPG(75);

//        // Send frame via Photon RPC
//        photonView.RPC("ReceiveVideoFrame", RpcTarget.Others, frameData);
//    }

//    private void RotateTexture180(Texture2D texture)
//    {
//        Color[] pixels = texture.GetPixels();
//        Color[] rotatedPixels = new Color[pixels.Length];
//        int width = texture.width;
//        int height = texture.height;

//        for (int y = 0; y < height; y++)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                // Flip both horizontally and vertically
//                rotatedPixels[x + y * width] = pixels[(width - x - 1) + (height - y - 1) * width];
//            }
//        }

//        texture.SetPixels(rotatedPixels);
//        texture.Apply();
//    }
//}
