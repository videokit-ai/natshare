/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU {

	using UnityEngine;

	public interface INatShare {
		bool Share (byte[] pngData, ShareCallback callback);
		bool Share (string media, ShareCallback callback);
		bool SaveToCameraRoll (byte[] pngData, string album);
        bool SaveToCameraRoll (string path, string album, bool copy);
        Texture2D GetThumbnail (string videoPath, float time);
	}
}