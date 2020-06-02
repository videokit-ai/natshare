/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Tests {

    using UnityEngine;
    using System.IO;
    using System.Threading.Tasks;
    using Sharing;

    public class SaveTest : MonoBehaviour {

        async void Start () {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            await Task.Delay(2_000);
            // Get assets to share
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            var basePath = Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.streamingAssetsPath;
            var videoPath = Path.Combine(basePath, "animation.gif");
            // Save to camera roll
            var saved = await new SavePayload("NatShare")
                .AddImage(screenshot)
                //.AddMedia(videoPath)
                .Commit();
            Debug.Log($"Successfully saved items to camera roll: {saved}");
        }
    }
}