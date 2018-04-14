/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Core {

	using UnityEngine;
	using System;

	public static class NatShare {

		#region --Client API--

		public static bool Share (this Texture2D image) {
			return Implementation.Share(image);
		}

		public static bool Share (this string videoPath) {
			return Implementation.Share(videoPath);
		}

		public static bool SaveToCameraRoll (this Texture2D image) {
			return Implementation.SaveToCameraRoll(image);
		}

		public static bool SaveToCameraRoll (this string videoPath) {
			return Implementation.SaveToCameraRoll(videoPath);
		}

		public static void GetThumbnail (this string videoPath, Action<Texture2D> callback, float time = 0f) {
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