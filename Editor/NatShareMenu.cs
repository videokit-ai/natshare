/* 
*   NatShare
*   Copyright (c) 2023 NatML Inc. All rights reserved.
*/

namespace NatML.Sharing.Editor {

    using UnityEditor;
    using Internal;

    internal static class NatShareMenu {

        private const int BasePriority = 500;

        [MenuItem(@"NatML/NatShare " + NatShareSettings.Version, false, BasePriority)]
        private static void Version () { }

        [MenuItem(@"NatML/NatShare " + NatShareSettings.Version, true, BasePriority)]
        private static bool DisableVersion () => false;

        [MenuItem(@"NatML/View NatShare Docs", false, BasePriority + 1)]
        private static void OpenDocs () => Help.BrowseURL(@"https://docs.natml.ai/natshare");

        [MenuItem(@"NatML/Open a NatShare Issue", false, BasePriority + 2)]
        private static void OpenIssue () => Help.BrowseURL(@"https://github.com/natmlx/NatShare");
    }
}