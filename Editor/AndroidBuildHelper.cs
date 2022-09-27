/* 
*   NatShare
*   Copyright (c) 2022 NatML Inc. All rights reserved.
*/

namespace NatML.Sharing.Editor {

    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using Internal;

    internal sealed class AndroidBuildHelper : IPreprocessBuildWithReport {

        public int callbackOrder => 0;

        void IPreprocessBuildWithReport.OnPreprocessBuild (BuildReport report) {
            // Check
            if (report.summary.platform != BuildTarget.Android)
                return;
            // Embed
            EmbedAndroidX();
        }

        private static void EmbedAndroidX () {
            var guids = AssetDatabase.FindAssets("natshare-core");
            if (guids.Length == 0)
                return;
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var importer = PluginImporter.GetAtPath(path) as PluginImporter;
            importer.SetCompatibleWithPlatform(BuildTarget.Android, NatShareSettings.instance.EmbedAndroidX);
        }
    }
}