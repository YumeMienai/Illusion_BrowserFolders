﻿using System.Diagnostics;
using BrowserFolders.Common;
using HarmonyLib;
using Manager;
using UnityEngine;

namespace BrowserFolders.Hooks.KKP
{
    public class NewGameFolders : IFolderBrowser
    {
        public BrowserType Type => BrowserType.NewGame;

        private static FolderTreeView _folderTreeView;

        private static string _targetScene;

        private static EntryPlayer _newGame;

        public static string CurrentRelativeFolder => _folderTreeView?.CurrentRelativeFolder;

        public NewGameFolders()
        {
            _folderTreeView = new FolderTreeView(Overlord.GetUserDataRootPath(), Overlord.GetDefaultPath(0));
            _folderTreeView.CurrentFolderChanged = OnFolderChanged;

            Overlord.Init();
        }

        public static void Init(EntryPlayer list, int sex)
        {
            if (_newGame != list)
            {
                _folderTreeView.DefaultPath = Overlord.GetDefaultPath(sex);
                _folderTreeView.CurrentFolder = _folderTreeView.DefaultPath;

                _newGame = list;
                _targetScene = Scene.Instance.AddSceneName;
            }
        }

        public void OnGui()
        {
            if (_newGame != null && _targetScene == Scene.Instance.AddSceneName)
            {
                var screenRect = GetFullscreenBrowserRect();
                Utils.DrawSolidWindowBackground(screenRect);
                GUILayout.Window(362, screenRect, TreeWindow, "Select character folder");
            }
        }

        private static Rect GetFullscreenBrowserRect()
        {
            return new Rect((int)(Screen.width * 0.73), (int)(Screen.height * 0.55f), (int)(Screen.width * 0.2), (int)(Screen.height * 0.3));
        }

        private static void OnFolderChanged()
        {
            if (_newGame != null)
                Traverse.Create(_newGame).Method("CreateMaleList").GetValue();
        }

        private static void TreeWindow(int id)
        {
            GUILayout.BeginVertical();
            {
                _folderTreeView.DrawDirectoryTree();

                GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
                {
                    if (Overlord.DrawDefaultCardsToggle())
                        OnFolderChanged();

                    if (GUILayout.Button("Refresh thumbnails"))
                        OnFolderChanged();

                    if (GUILayout.Button("Open current folder in explorer"))
                        Process.Start("explorer.exe", $"\"{_folderTreeView.CurrentFolder}\"");
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
    }
}