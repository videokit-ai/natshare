/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Tests {

    using UnityEngine;
    using System.Collections;
    using System.IO;

    public class ShareTest : MonoBehaviour {

        IEnumerator Start () {
            yield return new WaitForEndOfFrame();
            // Take a screenshot
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            // Get video path // Since Android streaming assets is finicky, manually place the file
            var basePath = Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.streamingAssetsPath;
            var videoPath = Path.Combine(basePath, "pexels_video.mp4");
            // Share
            using (var payload = new SharePayload("Sharing is caring :)", () => Debug.Log("Items shared"))) {
                payload.AddImage(screenshot);
                payload.AddMedia(videoPath);
            }
        }
    }
}