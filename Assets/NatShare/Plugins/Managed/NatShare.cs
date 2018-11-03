/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU {

	using UnityEngine;
	using Platforms;
	using Docs;

	[Doc(@"NatShare")]
	public static class NatShare {

		#region --Client API--

		/// <summary>
        /// Share an texture with the native sharing UI.
        /// Returns true if the image can be shared.
        /// </summary>
        /// <param name="image">Image to be shared</param>
		/// <param name="callback">Optional. Callback to be invoked once sharing is complete</param>
        [Doc(@"ShareImage")]
		public static bool Share (Texture2D image, ShareCallback callback = null) {
			if (!image) {
				Debug.LogError("NatShare Error: Texture being shared is null");
				return false;
			}
			return Implementation.Share(image.EncodeToPNG(), callback);
		}

		/// <summary>
        /// Share plain text with the native sharing UI.
        /// Returns true if the text can be shared.
        /// </summary>
        /// <param name="media">Media to be shared</param>
		/// <param name="callback">Optional. Callback to be invoked once sharing is complete</param>
		[Doc(@"ShareMedia")]
		public static bool Share (string media, ShareCallback callback = null) {
			if (string.IsNullOrEmpty(media)) {
				Debug.LogError("NatShare Error: Media being shared is null or empty");
				return false;
			}
			return Implementation.Share(media, callback);
		}

		/// <summary>
        /// Save an image to the camera roll.
        /// Returns true if the image can be saved to the camera roll.
        /// </summary>
        /// <param name="image">Image to be saved</param>
		/// <param name="album">Optional. Album where to save image</param>
        [Doc(@"SaveToCameraRoll")]
		public static bool SaveToCameraRoll (Texture2D image, string album = "") {
			if (!image) {
				Debug.LogError("NatShare Error: Texture being saved is null");
				return false;
			}
			return Implementation.SaveToCameraRoll(image.EncodeToPNG(), album ?? "");
		}

		/// <summary>
        /// Save a media file to the camera roll.
        /// Returns true if the file is found and can be saved to the camera roll.
        /// </summary>
        /// <param name="path">Path to media file</param>
		/// <param name="album">Optional. Album where to save image</param>
		/// <param name="copy">Optional. Should file be copied or moved to the camera roll?</param>
        [Doc(@"SaveToCameraRoll")]
		public static bool SaveToCameraRoll (string path, string album = "", bool copy = true) {
			if (string.IsNullOrEmpty(path)) {
				Debug.LogError("NatShare Error: Path to media file is invalid");
				return false;
			}
			return Implementation.SaveToCameraRoll(path, album ?? "", copy);
		}

		/// <summary>
        /// Get a thumbnail texture for a recorded video.
        /// If the thumbnail cannot be loaded, the function will return `null`.
        /// </summary>
        /// <param name="videoPath">Path to recorded video</param>
        /// <param name="time">Optional. Time to get thumbnail from in video</param>
        [Doc(@"GetThumbnail", @"GetThumbnailDiscussion"), Code(@"Thumbnail")]
		public static Texture2D GetThumbnail (string videoPath, float time = 0f) {
			if (string.IsNullOrEmpty(videoPath)) {
				Debug.LogError("NatShare Error: Path to video file is invalid");
				return null;
			}
			return Implementation.GetThumbnail(videoPath, time);
		}
		#endregion


		#region --Initializer--

		public static readonly INatShare Implementation;

		static NatShare () {
			Implementation =
			#if UNITY_STANDALONE || UNITY_EDITOR
			new NatShareNull();
			#elif UNITY_IOS
			new NatShareiOS();
			#elif UNITY_ANDROID
			new NatShareAndroid();
			#else
			new NatShareNull();
			#endif
		}
		#endregion
	}
}