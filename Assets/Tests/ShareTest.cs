/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatShare.Tests {

    using UnityEngine;
    using System.IO;
    using System.Threading.Tasks;

    public class ShareTest : MonoBehaviour {

        async void Start () {
            await Task.Delay(2000);
            // Get assets to share
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            var basePath = Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.streamingAssetsPath;
            var videoPath = Path.Combine(basePath, "pexels_video.mp4");
            // Share
            var payload = new SharePayload();
            payload.AddImage(screenshot);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared items: {success}");
        }
    }
}