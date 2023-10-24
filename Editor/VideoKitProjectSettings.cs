/* 
*   VideoKit
*   Copyright © 2023 NatML Inc. All Rights Reserved.
*/

#nullable enable

namespace VideoKit.Editor {

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEditor;
    using Internal;

    /// <summary>
    /// VideoKit settings for the current Unity project.
    /// </summary>
    [FilePath(@"ProjectSettings/VideoKit.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class VideoKitProjectSettings : ScriptableSingleton<VideoKitProjectSettings> {

        #region --Data--
        [SerializeField, FormerlySerializedAs(@"licenseKey")]
        private string accessKey = @"";

        [SerializeField]
        private bool androidx = true;

        [SerializeField]
        private string photoLibraryUsageDescription = @"";
        #endregion


        #region --Client API--
        /// <summary>
        /// VideoKit access key.
        /// </summary>
        internal string AccessKey {
            get => accessKey;
            set {
                // Check
                if (value == accessKey)
                    return;
                // Set
                accessKey = value;
                Save(false);
                // Update
                VideoKit.SetSessionToken(null);
            }
        }

        /// <summary>
        /// Whether to embed the `androidx` support library in the build.
        /// </summary>
        public bool EmbedAndroidX {
            get => androidx;
            set {
                // Check
                if (value == androidx)
                    return;
                // Set
                androidx = value;
                Save(false);
            }
        }

        /// <summary>
        /// Photo library usage description presented to the user when sharing a media asset.
        /// This only applies on iOS.
        /// </summary>
        public string PhotoLibraryUsageDescription {
            get => photoLibraryUsageDescription;
            set {
                // Check
                if (value == photoLibraryUsageDescription)
                    return;
                // Set
                photoLibraryUsageDescription = value;
                Save(false);
            }
        }

        /// <summary>
        /// Create VideoKit settings from the current project settings.
        /// </summary>
        internal static VideoKitSettings CreateSettings () {
            var accessKey = !string.IsNullOrEmpty(instance.AccessKey) ? instance.AccessKey : VideoKitSettings.FallbackAccessKey;
            var settings = ScriptableObject.CreateInstance<VideoKitSettings>();
            settings.Initialize(accessKey);
            return settings;
        }
        #endregion


        #region --Operations--

        [InitializeOnLoadMethod]
        private static void OnLoad () => VideoKitSettings.Instance = CreateSettings();

        [InitializeOnEnterPlayMode]
        private static void OnEnterPlaymodeInEditor (EnterPlayModeOptions options) => VideoKitSettings.Instance = CreateSettings();
        #endregion
    }
}