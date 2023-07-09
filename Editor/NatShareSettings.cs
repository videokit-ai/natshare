/* 
*   NatShare
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Sharing.Internal {

    using UnityEngine;
    using UnityEditor;

    [FilePath(@"ProjectSettings/NatShare.asset", FilePathAttribute.Location.ProjectFolder)]
    public class NatShareSettings : ScriptableSingleton<NatShareSettings> {

        #region --Data--
        [SerializeField]
        private bool androidx = true;
        #endregion


        #region --Client API--
        /// <summary>
        /// Whether to embed the `androidx` support library in the build.
        /// </summary>
        public bool EmbedAndroidX {
            get => androidx;
            set {
                androidx = value;
                Save(false);
            }
        }
        #endregion


        #region --Operations--
        public const string API = @"ai.natml.natshare";
        public const string Version = @"1.3.1";
        #endregion
    }
}