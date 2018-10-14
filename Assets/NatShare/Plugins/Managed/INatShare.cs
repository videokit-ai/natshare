/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU {

	using UnityEngine;
	using System;

	public interface INatShare {
		bool ShareText (string text, ShareCallback callback);
		bool ShareImage (byte[] pngData, string message, ShareCallback callback);
		bool ShareMedia (string path, string message, ShareCallback callback);
		bool SaveToCameraRoll (byte[] pngData);
        bool SaveToCameraRoll (string videoPath);
        void GetThumbnail (string videoPath, Action<Texture2D> callback, float time);
	}
}