using System;
using System.Collections.Generic;
using System.Globalization;
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
        internal static string[] gridStrings = { "General", "Player", "Employee", "Customer" };


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
            windowRef = GUILayout.Window(window.id, windowRef, window.func, window.name, []);
        }
        internal static void ShowWindow(string ident, ref Rect windowRef)
        {
            Window window = windows.Get(ident);
            windowRef = GUILayout.Window(window.id, windowRef, window.func, window.name, []);
        }


        // Windows
        internal static void MainWindow(int id)
        {
            GUILayout.BeginArea(new Rect(10f, 25f, 330f, 340f));
            gridSelected = GUILayout.SelectionGrid(gridSelected, gridStrings, 4);
            GUILayout.Space(4f);
            switch (gridSelected)
            {
                case 0:
                    MainWindowGeneral();
                    break;
                case 1:
                    MainWindowPlayer();
                    break;
                case 2:
                    MainWindowEmployee();
                    break;
                case 3:
                    MainWindowCustomer();
                    break;
                default:
                    MainWindowGeneral();
                    break;
            }
            GUILayout.EndArea();

            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical([]);
            if (GUILayout.Button("Save Settings")) Settings.Save();
            GUILayout.EndVertical();
            GUIStyle guistyle = new GUIStyle(GUI.skin.label);
            guistyle.fontSize = 14;
            guistyle.fontStyle = FontStyle.Bold;
            guistyle.normal.textColor = new Color(1.0f, 0.64f, 0.0f);
            GUILayout.Label("Powered by ENZIO", guistyle, [GUILayout.ExpandWidth(true)]);

            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }
        internal static void MainWindowGeneral()
        {
            GUILayout.BeginVertical([]);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Activate Duping:", [GUILayout.Width(130f)]);
            if (GUILayout.Toggle(Settings.duping, "") != Settings.duping) Settings.duping = !Settings.duping;
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Duping Amount:", [GUILayout.Width(130f)]);
            Settings.dupingAmount = Int32.Parse(GUILayout.TextArea(Settings.dupingAmount.ToString(), 24, [GUILayout.Width(60f)]));
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Edit Decoration:", [GUILayout.Width(130f)]);
            if (GUILayout.Toggle(Settings.editDecorationPlacement, "") != Settings.editDecorationPlacement) Settings.editDecorationPlacement = !Settings.editDecorationPlacement;
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.EndVertical();
        }
        internal static void MainWindowPlayer()
        {
            GUILayout.BeginVertical([]);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Movement Speed:", [GUILayout.Width(130f)]);
            Settings.playerMoveSpeed = float.Parse(GUILayout.TextArea(Settings.playerMoveSpeed.ToString("G7", CultureInfo.GetCultureInfo("en-US")), 24, [GUILayout.Width(60f)]), CultureInfo.GetCultureInfo("en-US"));
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Sprinting Speed:", [GUILayout.Width(130f)]);
            Settings.playerSprintSpeed = float.Parse(GUILayout.TextArea(Settings.playerSprintSpeed.ToString("G7", CultureInfo.GetCultureInfo("en-US")), 24, [GUILayout.Width(60f)]), CultureInfo.GetCultureInfo("en-US"));
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.EndVertical();
        }
        internal static void MainWindowEmployee()
        {
            GUILayout.BeginVertical([]);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Activate Employee changes:", [GUILayout.Width(130f)]);
            if (GUILayout.Toggle(Settings.editEmployees, "") != Settings.editEmployees) Settings.editEmployees = !Settings.editEmployees;
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Max Employees:", [GUILayout.Width(130f)]);
            Settings.maxEmployees = Int32.Parse(GUILayout.TextArea(Settings.maxEmployees.ToString(), 24, [GUILayout.Width(60f)]));
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Item Place Wait Time:", [GUILayout.Width(130f)]);
            Settings.employeeItemPlaceWait = float.Parse(GUILayout.TextArea(Settings.employeeItemPlaceWait.ToString("G7", CultureInfo.GetCultureInfo("en-US")), 24, [GUILayout.Width(60f)]), CultureInfo.GetCultureInfo("en-US"));
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginHorizontal([]);
            GUILayout.Label("Speed Factor:", [GUILayout.Width(130f)]);
            Settings.extraEmployeeSpeedFactor = float.Parse(GUILayout.TextArea(Settings.extraEmployeeSpeedFactor.ToString("G7", CultureInfo.GetCultureInfo("en-US")), 24, [GUILayout.Width(60f)]), CultureInfo.GetCultureInfo("en-US"));
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUILayout.BeginVertical([]);
            if (GUILayout.Button("Apply Changes")) Helpers.UpdateEmployeeStats();
            GUILayout.EndVertical();
            GUILayout.Space(4f);

            GUILayout.EndVertical();
        }
        internal static void MainWindowCustomer()
        {
        }
        internal static void MainWindowNpc()
        {
        }
    }
}
