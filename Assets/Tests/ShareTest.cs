/* 
*   NatShare
*   Copyright (c) 2021 Yusuf Olokoba
*/

namespace NatSuite.Tests {

    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using Sharing;

    public class ShareTest : MonoBehaviour {

        async void Start () {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            var tests = new Func<Task>[] {
                //TestShareText,
                //TestShareImage,
                TestShareVideo,
                //TestShareTextImage,
                //TestShareTextVideo,
                //TestShareImageVideo,
                //TestShareTextImageVideo
            };
            foreach (var test in tests) {
                await Task.Delay(3_000);
                await test();
            }
        }

        async Task TestShareText () {
            // Payload
            var text = "Sharing is caring!";
            // Commit
            var payload = new SharePayload();
            payload.AddText(text);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared text: {success}");
        }

        async Task TestShareImage () {
            // Payload
            var image = ScreenCapture.CaptureScreenshotAsTexture();
            // Commit
            var payload = new SharePayload();
            payload.AddImage(image);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared image: {success}");
            // Cleanup
            Texture2D.Destroy(image);
        }

        async Task TestShareVideo () {
            // Payload
            var videoPath = await Utility.PathFromStreamingAssets("pexels_video.mp4");
            // Commit
            var payload = new SharePayload();
            payload.AddMedia(videoPath);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared video: {success}");
        }

        async Task TestShareTextImage () {
            // Payload
            var text = "Sharing is caring!";
            var image = ScreenCapture.CaptureScreenshotAsTexture();
            // Commit
            var payload = new SharePayload();
            payload.AddText(text);
            payload.AddImage(image);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared text and image: {success}");
            // Cleanup
            Texture2D.Destroy(image);
        }

        async Task TestShareImageVideo () {
            // Payload
            var image = ScreenCapture.CaptureScreenshotAsTexture();
            var videoPath = await Utility.PathFromStreamingAssets("pexels_video.mp4");
            // Commit
            var payload = new SharePayload();
            payload.AddImage(image);
            payload.AddMedia(videoPath);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared image and video: {success}");
            // Cleanup
            Texture2D.Destroy(image);
        }

        async Task TestShareTextVideo () {
            // Payload
            var text = "Sharing is caring!";
            var videoPath = await Utility.PathFromStreamingAssets("pexels_video.mp4");
            // Commit
            var payload = new SharePayload();
            payload.AddText(text);
            payload.AddMedia(videoPath);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared text and video: {success}");
        }

        async Task TestShareTextImageVideo () {
            // Payload
            var text = "Sharing is caring!";
            var image = ScreenCapture.CaptureScreenshotAsTexture();
            var videoPath = await Utility.PathFromStreamingAssets("pexels_video.mp4");
            // Commit
            var payload = new SharePayload();
            payload.AddText(text);
            payload.AddImage(image);
            payload.AddMedia(videoPath);
            var success = await payload.Commit();
            Debug.Log($"Successfully shared image and video: {success}");
            // Cleanup
            Texture2D.Destroy(image);
        }
    }
}