/* 
*   NatShare
*   Copyright (c) 2023 NatML Inc. All rights reserved.
*/

namespace NatML.Sharing.Editor {

    using System.Collections.Generic;
    using UnityEditor;
    using Internal;

    internal static class NatShareSettingsProvider {

        [SettingsProvider]
        public static SettingsProvider CreateProvider () => new SettingsProvider(@"Project/NatML/NatShare", SettingsScope.Project) {
            label = @"NatShare",
            guiHandler = searchContext => {
                EditorGUILayout.LabelField(@"Android Settings", EditorStyles.boldLabel);
                NatShareSettings.instance.EmbedAndroidX = EditorGUILayout.Toggle(@"Embed AndroidX Library", NatShareSettings.instance.EmbedAndroidX);
            },
            keywords = new HashSet<string>(new[] { @"NatML", @"NatCorder", @"NatDevice", @"Hub", @"NatML Hub", @"NatShare" }),
        };
    }
}