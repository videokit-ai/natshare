/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

	using UnityEngine;
	using System;
	using System.Collections;
	using System.Runtime.InteropServices;

	public class NatShareWebGL : INatShare {

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
			IntPtr[] thumbnailData = new IntPtr[3]; int unused = 0;
            var thumbnailHandle = GCHandle.Alloc(thumbnailData, GCHandleType.Pinned);
            NatShareBridge.GetThumbnail(videoPath, time, ref thumbnailData[0], ref unused, ref unused);
            var callbackObject = new GameObject("NatShareWebGL Delegate").AddComponent<ThumbnailDelegate>();
            callbackObject.StartCoroutine(GetThumbnail(thumbnailHandle, callback, callbackObject));
		}

		private IEnumerator GetThumbnail (GCHandle thumbnailHandle, Action<Texture2D> callback, ThumbnailDelegate callbackObject) {
            yield return new WaitUntil(() => Marshal.ReadIntPtr(thumbnailHandle.AddrOfPinnedObject()) != IntPtr.Zero);
            MonoBehaviour.Destroy(callbackObject); // We don't need this anymore
            var pixelBuffer = Marshal.ReadIntPtr(thumbnailHandle.AddrOfPinnedObject());
            var width = Marshal.ReadInt32(new IntPtr(thumbnailHandle.AddrOfPinnedObject().ToInt32() + sizeof(int)));
            var height = Marshal.ReadInt32(new IntPtr(thumbnailHandle.AddrOfPinnedObject().ToInt32() + 2 * sizeof(int)));
            var thumbnail = new Texture2D(width, height, TextureFormat.RGBA32, false);
            thumbnail.LoadRawTextureData(pixelBuffer, width * height * 4);
            thumbnail.Apply();
            NatShareBridge.FreeThumbnail(thumbnailHandle.AddrOfPinnedObject());
            thumbnailHandle.Free();
            callback(thumbnail);
        }

        private class ThumbnailDelegate : MonoBehaviour {}
	}
}