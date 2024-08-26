using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ENZIO
{
    public struct Window
    {
        public bool active;
        public int id;
        public string ident;
        public string name;
        public GUI.WindowFunction func;
    }
    public class Windows : Dictionary<int, Window>
    {
        public void Add(int id, string ident, string name, GUI.WindowFunction func, bool active = true)
        {
            if (this.Any(t => t.Value.ident.Contains(ident)))
            {
                Plugin.Logger.LogError($"Window with ident {ident} already exist!");
                return;
            }
            if (this.Any(t => t.Key == id))
            {
                Plugin.Logger.LogError($"Window with id {id} already exist!");
                return;
            }

            Window window;
            window.active = active;
            window.id = id;
            window.ident = ident;
            window.name = name;
            window.func = func;
            this.Add(id, window);
        }
        public Window Get(int id)
        {
            return this.ElementAt(id).Value;
        }
        public Window Get(string ident)
        {
            return this.FirstOrDefault(t => t.Value.ident == ident).Value;
        }
        public new void Remove(int id)
        {
            base.Remove(id);
        }
        public void Remove(string ident)
        {
            base.Remove(this.FirstOrDefault(t => t.Value.ident == ident).Key);
        }
    }

    internal static class IGUI
    {
        internal static bool showGui = false;
        internal static bool showGuiLastState = false;
        internal static CursorLockMode originalLockState = CursorLockMode.None;
        internal static bool originalVisible = true;

        internal static Windows windows = new Windows();
        internal static bool windows_initialized = false;
        internal static Rect mainWindow = new Rect(Screen.width - 360f, 10f, 350f, 350f);

        internal static int gridSelected = 0;
        internal static string[] gridStrings = { "General", "Player", "NPC" };


        internal static void SetupWindows()
        {
            AddWindow("main", Settings.name, new GUI.WindowFunction(IGUI.MainWindow));
            if (!windows_initialized) windows_initialized = true;
        }
        internal static void AddWindow(string ident, string name, GUI.WindowFunction func)
        {
            windows.Add(windows.Count, ident, name, func);
        }
        internal static void DeleteWindow(int id, bool all = false)
        {
            if (!all)
                windows.Remove(id);
            else
            {
                windows.Clear();
                SetupWindows();
            }
        }
        internal static void ShowWindow(int id, ref Rect windowRef)
        {
            Window window = windows.Get(id);
            windowRef = GUILayout.Window(window.id, windowRef, window.func, window.name, Array.Empty<GUILayoutOption>());
        }
        internal static void ShowWindow(string ident, ref Rect windowRef)
        {
            Window window = windows.Get(ident);
            windowRef = GUILayout.Window(window.id, windowRef, window.func, window.name, Array.Empty<GUILayoutOption>());
        }


        // Windows
        internal static void MainWindow(int id)
        {
            GUILayout.BeginArea(new Rect(10f, 25f, 330f, 330f));
            gridSelected = GUILayout.SelectionGrid(gridSelected, gridStrings, 3);
            GUILayout.Space(4f);
            switch (gridSelected)
            {
                case 0:
                    MainWindowGeneral();
                    break;
                case 1:
                    GUILayout.Label("Player selected");
                    break;
                case 2:
                    GUILayout.Label("NPC selected");
                    break;
                default:
                    MainWindowGeneral();
                    break;
            }
            GUILayout.EndArea();

            GUILayout.FlexibleSpace();
            GUIStyle guistyle = new GUIStyle(GUI.skin.label);
            guistyle.fontSize = 14;
            guistyle.fontStyle = FontStyle.Bold;
            guistyle.normal.textColor = new Color(1.0f, 0.64f, 0.0f);
            GUILayout.Label("Powered by ENZIO", guistyle, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });

            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }
        internal static void MainWindowGeneral()
        {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());

            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Activate Duping:", new GUILayoutOption[] { GUILayout.Width(120f) });
            if (GUILayout.Toggle(Settings.duping, "") != Settings.duping)
                Settings.duping = !Settings.duping;
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Duping Amount:", new GUILayoutOption[] { GUILayout.Width(120f) });
            Settings.dupingAmount = GUILayout.TextArea(Settings.dupingAmount, 3, new GUILayoutOption[] { GUILayout.Width(40f) });
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.EndVertical();
        }
    }
}
