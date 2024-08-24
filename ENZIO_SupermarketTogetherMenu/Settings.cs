using BepInEx;
using BepInEx.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using static UnityEngine.Rendering.RayTracingAccelerationStructure;

namespace ENZIO
{
    internal static class Settings
    {
        internal const string GUID = "org.bepinex.plugins.enziosupermarkettogether";
        internal const string Author = "ENZIO";
        internal const string Name = "ENZIO - Supermarket Together Menu";
        internal const string Version = "0.0.1";

        internal static bool duping;
        internal static int dupingAmount;

        internal static float playerMoveSpeed = 10f;
        internal static float playerSprintSpeed = 15f;

        internal static bool editEmployeeSpeed = true;
        internal static int maxEmployees = 10;
        internal static float employeeItemPlaceWait = 0.01f;
        internal static float extraEmployeeSpeedFactor = 0.5f;

        internal static bool editCustomerSpeed = true;
        internal static bool editNpcSpeed = false;
        internal static float npcProductCheckoutWait = 0.01f;
        internal static float npcProductItemPlaceWait = 0.01f;
        internal static float npcSpeed = 10f;
        internal static float npcAcceleration = 50f;
        internal static float npcAngularSpeed = 1000f;

        internal static void LoadSettings()
        {
            var settings = new ConfigFile(Path.Combine(Paths.ConfigPath, "ENZIO.cfg"), true);
            var duping = settings.Bind("Duping",
                "Activate",
                false);
            Settings.duping = duping.Value;
            var dupingAmount = settings.Bind("Duping",
                "Amount",
                10);
            Settings.dupingAmount = dupingAmount.Value;
        }

        internal static void SaveSettings()
        {
            var settings = new ConfigFile(Path.Combine(Paths.ConfigPath, "ENZIO.cfg"), true);
            var duping = settings.Bind("Duping",
                "Activate",
                false);
            duping.Value = Settings.duping;
            var dupingAmount = settings.Bind("Duping",
            "Amount",
                10);
            dupingAmount.Value = Settings.dupingAmount;
        }
    }
}
