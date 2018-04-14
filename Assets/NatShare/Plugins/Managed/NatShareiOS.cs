/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Core {

	using UnityEngine;
	using System;

	public class NatShareiOS : INatShare {

		bool INatShare.Share (Texture2D image) {
			return false;
		}

		bool INatShare.Share (string path) {
			return false;
		}

		bool INatShare.SaveToCameraRoll (Texture2D image) {
			return false;
		}

		bool INatShare.SaveToCameraRoll (string videoPath) {
			return false;
		}

		void INatShare.GetThumbnail (string videoPath, Action<Texture2D> callback, float time) {

		}
	}
}