/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

	using AOT;
	using UnityEngine;
	using System;
	using System.Runtime.InteropServices;

	public class NatShareiOS : INatShare {

		private ShareCallback callback;

		public NatShareiOS () {
			NatShareBridge.RegisterCallbacks(OnShare);
		}

		bool INatShare.Share (byte[] pngData, ShareCallback callback) {
			this.callback = callback;
			return NatShareBridge.ShareImage(pngData, pngData.Length);
		}

		bool INatShare.Share (string media, ShareCallback callback) {
			this.callback = callback;
			Uri uri;
			if (Uri.TryCreate(media, UriKind.Absolute, out uri))
				return NatShareBridge.ShareMedia(media);
			else
				return NatShareBridge.ShareText(media);
		}

		bool INatShare.SaveToCameraRoll (byte[] pngData, string album) {
			return NatShareBridge.SaveToCameraRoll(pngData, pngData.Length, album);
		}

		bool INatShare.SaveToCameraRoll (string path, string album, bool copy) {
			return NatShareBridge.SaveToCameraRoll(path, album, copy);
		}

		Texture2D INatShare.GetThumbnail (string videoPath, float time) {
			IntPtr pixelBuffer = IntPtr.Zero; int width = 0, height = 0;
            if (!NatShareBridge.GetThumbnail(videoPath, time, ref pixelBuffer, ref width, ref height)) {
                Debug.LogError("NatShare Error: Failed to get thumbnail for video at path: "+videoPath);
				return null;
            }
            var thumbnail = new Texture2D(width, height, TextureFormat.BGRA32, false);
            thumbnail.LoadRawTextureData(pixelBuffer, width * height * 4);
            thumbnail.Apply();
            NatShareBridge.FreeThumbnail(pixelBuffer);
            return thumbnail;
		}

		[MonoPInvokeCallback(typeof(NatShareBridge.ShareCallback))]
		static void OnShare (bool completed) {
			/**
			 * We don't report the `completed` value to clients because we can't do so on Android.
			 * For more info, see the note in `NatShareAndroid::onShare`.
			 */
			var callback = (NatShare.Implementation as NatShareiOS).callback;
			if (callback != null) callback();
		}
	}
}
