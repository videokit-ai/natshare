/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Core {

	using UnityEngine;
	using System;

	public interface INatShare {
		bool Share (Texture2D image);
		bool Share (string path);
		bool SaveToCameraRoll (Texture2D image);
        bool SaveToCameraRoll (string videoPath);
        void GetThumbnail (string videoPath, Action<Texture2D> callback, float time);
	}
}