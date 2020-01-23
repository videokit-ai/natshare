/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatShare.Tests {

    using UnityEngine;
    using System.IO;
    using System.Threading.Tasks;

    public class SaveTest : MonoBehaviour {

        async void Start () {
            await Task.Delay(2000);
            // Get assets to share
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            var basePath = Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.streamingAssetsPath;
            var videoPath = Path.Combine(basePath, "animation.gif");
            // Save to camera roll
            var payload = new SavePayload("NatShare");
            payload.AddMedia(videoPath);
            var success = await payload.Commit();
            Debug.Log($"Successfully saved items to camera roll: {success}");
        }
    }
}