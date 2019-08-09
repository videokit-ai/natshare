/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Tests {

    using UnityEngine;
    using System.Collections;
    using System.IO;

    public class SaveTest : MonoBehaviour {

        IEnumerator Start () {
            yield return new WaitForEndOfFrame();
            // Take a screenshot
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            // Get video path // Since Android streaming assets is finicky, manually place the file
            var basePath = Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.streamingAssetsPath;
            var videoPath = Path.Combine("file://" + basePath, "pexels_video.mp4");
            // Save to camera roll
            using (var payload = new SavePayload("NatShare", () => Debug.Log("Items saved to camera roll"))) {
                payload.AddImage(screenshot);
                payload.AddMedia(videoPath);
            }
        }
    }
}