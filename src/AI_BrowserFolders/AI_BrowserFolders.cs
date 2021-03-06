﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;

namespace BrowserFolders
{
    [BepInPlugin(Guid, "Studio Browser Folders", Version)]
    [BepInProcess("StudioNEOV2")]
    [BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
    public class AI_BrowserFolders : BaseUnityPlugin
    {
        public const string Guid = "marco.FolderBrowser";
        public const string Version = "1.5";

        internal static new ManualLogSource Logger { get; private set; }

        private IFolderBrowser _sceneFolders;
        private IFolderBrowser _studioCharaFolders;

        public static ConfigEntry<bool> EnableStudio { get; private set; }
        public static ConfigEntry<bool> EnableStudioChara { get; private set; }
        public static ConfigEntry<bool> StudioSaveOverride { get; private set; }

        private void Awake()
        {
            Logger = base.Logger;

            EnableStudio = Config.Bind("Chara Studio", "Enable folder browser in scene browser", true, "Changes take effect on game restart");
            if (EnableStudio.Value) _sceneFolders = new SceneFolders();

            EnableStudioChara = Config.Bind("Chara Studio", "Enable folder browser in character browser", true, "Changes take effect on game restart");
            if (EnableStudioChara.Value) _studioCharaFolders = new StudioCharaFolders();

            StudioSaveOverride = Config.Bind("Chara Studio", "Save scenes to current folder", false, "When you select a custom folder to load a scene from, newly saved scenes will be saved to this folder.\nIf disabled, scenes are always saved to default folder (studio/scene).");
        }

        private void OnGUI()
        {
            _sceneFolders?.OnGui();
            _studioCharaFolders?.OnGui();
        }
    }
}
