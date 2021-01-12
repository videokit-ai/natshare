/* 
*   NatShare
*   Copyright (c) 2021 Yusuf Olokoba
*/

namespace NatSuite.Tests {

    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class Utility {

        /// <summary>
        /// Get a file path for an asset in streaming assets.
        /// Specifically on Android, this method handles extracting the file from the APK and placing it on the file system.
        /// </summary>
        public static async Task<string> PathFromStreamingAssets (string relativePath) {
            var absolutePath = Path.Combine(Application.streamingAssetsPath, relativePath);
            var persistentPath = Path.Combine(Application.persistentDataPath, relativePath);
            switch (Application.platform) {
                case RuntimePlatform.Android:
                    // Check persistent storage
                    if (File.Exists(persistentPath))
                        return persistentPath;
                    // Download from APK/AAB
                    var request = UnityWebRequest.Get(absolutePath);
                    request.SendWebRequest();
                    while (!request.isDone)
                        await Task.Yield();
                    // Copy to persistent storage
                    new FileInfo(persistentPath).Directory.Create();
                    File.WriteAllBytes(persistentPath, request.downloadHandler.data);
                    return persistentPath;
                default:
                    return absolutePath;
            }
        }
    }
}