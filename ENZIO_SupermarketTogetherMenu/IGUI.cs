using System;
using UnityEngine;

namespace ENZIO
{
    internal class IGUI
    {
        internal static bool showGui = false;
        internal static bool showGuiLastState = false;
        internal static CursorLockMode originalLockState = CursorLockMode.None;
        internal static bool originalVisible = true;




        internal static Rect mainWindow = new Rect(Screen.width - 360f, 10f, 350f, 350f);
        internal static void MainWindow(int windowID)
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
            Settings.dupingAmount = GUILayout.TextArea(Settings.dupingAmount, 3, new GUILayoutOption[] { GUILayout.Width(15f) });
            GUILayout.EndHorizontal();
            GUILayout.Space(4f);

            GUIStyle guistyle = new GUIStyle(GUI.skin.label);
            guistyle.fontSize = 14;
            guistyle.fontStyle = FontStyle.Bold;
            guistyle.normal.textColor = new Color(1.0f, 0.64f, 0.0f);
            GUILayout.Label("Powered by ENZIO", guistyle, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });

            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }
    }
}
