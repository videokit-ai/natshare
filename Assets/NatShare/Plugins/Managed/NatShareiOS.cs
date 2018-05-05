/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

	using UnityEngine;
	using System;
	using System.Runtime.InteropServices;

	public class NatShareiOS : INatShare {

		bool INatShare.Share (byte[] pngData, string message) {
			return NatShareBridge.Share(pngData, pngData.Length, message);
		}

		bool INatShare.Share (string path, string message) {
			return NatShareBridge.Share(path, message);
		}

		bool INatShare.SaveToCameraRoll (byte[] pngData) {
			return NatShareBridge.SaveToCameraRoll(pngData, pngData.Length);
		}

		bool INatShare.SaveToCameraRoll (string videoPath) {
			return NatShareBridge.SaveToCameraRoll(videoPath);
		}

		void INatShare.GetThumbnail (string videoPath, Action<Texture2D> callback, float time) {
			IntPtr pixelBuffer = IntPtr.Zero; int width = 0, height = 0;
            if (!NatShareBridge.GetThumbnail(videoPath, time, ref pixelBuffer, ref width, ref height)) {
                Debug.LogError("NatShare Error: Failed to get thumbnail for video at path: "+videoPath);
                callback(null);
            }
            var thumbnail = new Texture2D(width, height, TextureFormat.BGRA32, false);
            thumbnail.LoadRawTextureData(pixelBuffer, width * height * 4);
            thumbnail.Apply();
            NatShareBridge.FreeThumbnail(pixelBuffer);
            callback(thumbnail);
		}
	}
}