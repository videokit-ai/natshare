/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU {

	using UnityEngine;
	using System;
	using Platforms;
	using Docs;

	[Doc(@"NatShare")]
	public static class NatShare {

		#region --Client API--

		/// <summary>
        /// Share plain text with the native sharing UI.
        /// Returns true if the text can be shared.
        /// </summary>
        /// <param name="text">Text to be shared</param>
		public static bool ShareText (string text) {
			if (string.IsNullOrEmpty(text)) {
				Debug.LogError("NatShare Error: Text being shared is null");
				return false;
			}
			return Implementation.Share(text);
		}

		/// <summary>
        /// Share an texture with the native sharing UI.
        /// Returns true if the image can be shared.
        /// </summary>
        /// <param name="image">Image to be shared</param>
		/// <param name="message">Optional. Message to be shared with image</param>
        [Doc(@"ShareImage")]
		public static bool ShareImage (Texture2D image, string message = "") {
			if (!image) {
				Debug.LogError("NatShare Error: Texture being shared is null");
				return false;
			}
			return Implementation.Share(image.EncodeToPNG(), message);
		}

		/// <summary>
        /// Share a media file with the native sharing UI.
        /// Returns true if media file is found and can be shared.
        /// </summary>
        /// <param name="path">Path to media file</param>
		/// <param name="message">Optional. Message to be shared with image</param>
        [Doc(@"ShareMedia")]
		public static bool ShareMedia (string path, string message = "") {
			if (string.IsNullOrEmpty(path)) {
				Debug.LogError("NatShare Error: Path to media file is invalid");
				return false;
			}
			return Implementation.Share(path, message);
		}

		/// <summary>
        /// Save an image to the camera roll.
        /// Returns true if the image can be saved to the camera roll.
        /// </summary>
        /// <param name="image">Image to be saved</param>
        [Doc(@"SaveToCameraRoll")]
		public static bool SaveToCameraRoll (Texture2D image) {
			if (!image) {
				Debug.LogError("NatShare Error: Texture being saved is null");
				return false;
			}
			return Implementation.SaveToCameraRoll(image.EncodeToPNG());
		}

		/// <summary>
        /// Save a media file to the camera roll.
        /// Returns true if the file is found and can be saved to the camera roll.
        /// </summary>
        /// <param name="path">Path to media file</param>
        [Doc(@"SaveToCameraRoll")]
		public static bool SaveToCameraRoll (string path) {
			if (string.IsNullOrEmpty(path)) {
				Debug.LogError("NatShare Error: Path to media file is invalid");
				return false;
			}
			return Implementation.SaveToCameraRoll(path);
		}

		/// <summary>
        /// Get a thumbnail texture for a recorded video.
        /// If the thumbnail cannot be loaded, the callback will be invoked with null.
        /// </summary>
        /// <param name="videoPath">Path to recorded video</param>
        /// <param name="callback">Callback that will be invoked with the thumbnail texture</param>
        /// <param name="time">Optional: Time to get thumbnail from in video</param>
        [Doc(@"GetThumbnail", @"GetThumbnailDiscussion"), Code(@"Thumbnail")]
		public static void GetThumbnail (string videoPath, Action<Texture2D> callback, float time = 0f) {
			if (string.IsNullOrEmpty(videoPath)) {
				Debug.LogError("NatShare Error: Path to video file is invalid");
				callback(null);
				return;
			}
			Implementation.GetThumbnail(videoPath, callback, time);
		}
		#endregion


		#region --Initializer--

		private static readonly INatShare Implementation;

		static NatShare () {
			Implementation =
			#if UNITY_STANDALONE || UNITY_EDITOR
			new NatShareNull();
			#elif UNITY_IOS
			new NatShareiOS();
			#elif UNITY_ANDROID
			new NatShareAndroid();
			#elif UNITY_WEBGL
			new NatShareWebGL();
			#else
			new NatShareNull();
			#endif
		}
		#endregion
	}
}