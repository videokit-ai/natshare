/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU {

	using UnityEngine;
	using System;

	public interface INatShare {
		bool Share (byte[] pngData, string message);
		bool Share (string path, string message);
		bool SaveToCameraRoll (byte[] pngData);
        bool SaveToCameraRoll (string videoPath);
        void GetThumbnail (string videoPath, Action<Texture2D> callback, float time);
	}
}