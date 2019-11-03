/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare {

    using UnityEditor;
    using UnityEditor.Callbacks;
	using UnityEditor.iOS.Xcode;
    using System.IO;

    public static class NatShareEditor {

        private const string LibraryUsageDescription = @"Allow this app to save media to your photo library"; // Change this as necessary

        [PostProcessBuild]
		static void SetPermissions (BuildTarget buildTarget, string path) {
			if (buildTarget != BuildTarget.iOS)
				return;
			var plistPath = path + "/Info.plist";
			var plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			var rootDictionary = plist.root;
			rootDictionary.SetString(@"NSPhotoLibraryUsageDescription", LibraryUsageDescription);
			rootDictionary.SetString(@"NSPhotoLibraryAddUsageDescription", LibraryUsageDescription);
			File.WriteAllText(plistPath, plist.WriteToString());
		}
    }
}