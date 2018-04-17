/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Core {

	using UnityEngine;
	using System;

	public interface INatShare {
		bool Share (byte[] pngData);
		bool Share (string path);
		bool SaveToCameraRoll (byte[] pngData);
        bool SaveToCameraRoll (string videoPath);
        void GetThumbnail (string videoPath, Action<Texture2D> callback, float time);
	}
}