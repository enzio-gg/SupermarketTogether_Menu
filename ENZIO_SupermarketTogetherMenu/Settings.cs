using BepInEx;
using BepInEx.Configuration;
using System.IO;

namespace ENZIO
{
    internal static class Settings
    {
        internal const string Guid = "org.bepinex.plugins.enziosupermarkettogether";
        internal const string author = "ENZIO";
        internal const string name = "ENZIO - Supermarket Together Menu";
        internal const string version = "0.0.1";

        // General
        internal static bool duping = false;
        internal static int dupingAmount = 2;
        internal static bool editDecorationPlacement = false;

        // Player
        internal static float playerMoveSpeed = 10f;
        internal static float playerSprintSpeed = 15f;

        // Employee
        internal static bool editEmployees = false;
        internal static int maxEmployees = 10;
        internal static float employeeItemPlaceWait = 0.01f;
        internal static float extraEmployeeSpeedFactor = 0.5f;
        internal static float employeeSpeed = 10f;
        internal static float employeeAcceleration = 50f;
        internal static float employeeAngularSpeed = 1000f;

        // NPC
        internal static bool editCustomers = false;
        internal static bool editNpcs = false;
        internal static float npcProductCheckoutWait = 0.01f;
        internal static float npcProductItemPlaceWait = 0.01f;
        internal static float npcSpeed = 10f;
        internal static float npcAcceleration = 50f;
        internal static float npcAngularSpeed = 1000f;

        internal static void Load()
        {
            var settings = new ConfigFile(Path.Combine(Paths.ConfigPath, "ENZIO.cfg"), true);

            // General
            var duping = settings.Bind("General", "ActivateDuping", false);
            Settings.duping = duping.Value;

            var dupingAmount = settings.Bind("General", "DupingAmount", 2);
            Settings.dupingAmount = dupingAmount.Value;

            var editDecorationPlacement = settings.Bind("General", "EditDecorationPlacement", false);
            Settings.editDecorationPlacement = editDecorationPlacement.Value;


            // Player
            var playerMoveSpeed = settings.Bind("Player", "MoveSpeed", 5f);
            Settings.playerMoveSpeed = playerMoveSpeed.Value;

            var playerSprintSpeed = settings.Bind("Player", "SprintSpeed", 10f);
            Settings.playerSprintSpeed = playerSprintSpeed.Value;


            // Employee
            var editEmployees = settings.Bind("Employee", "EditEmployees", false);
            Settings.editEmployees = editEmployees.Value;

            var maxEmployees = settings.Bind("Employee", "MaxEmployees", 10);
            Settings.maxEmployees = maxEmployees.Value;

            var employeeItemPlaceWait = settings.Bind("Employee", "ItemPlaceWait", 0.01f);
            Settings.employeeItemPlaceWait = employeeItemPlaceWait.Value;

            var extraEmployeeSpeedFactor = settings.Bind("Employee", "SpeedFactor", 0.5f);
            Settings.extraEmployeeSpeedFactor = extraEmployeeSpeedFactor.Value;

            var employeeSpeed = settings.Bind("Employee", "Speed", 5f);
            Settings.employeeSpeed = employeeSpeed.Value;

            var employeeAcceleration = settings.Bind("Employee", "Acceleration", 50f);
            Settings.employeeAcceleration = employeeAcceleration.Value;

            var employeeAngularSpeed = settings.Bind("Employee", "AngularSpeed", 1000f);
            Settings.employeeAngularSpeed = employeeAngularSpeed.Value;


            // NPC
            var editCustomers = settings.Bind("NPC", "EditCustomers", false);
            Settings.editCustomers = editCustomers.Value;

            var editNpcs = settings.Bind("NPC", "EditNpcs", false);
            Settings.editNpcs = editNpcs.Value;

            var npcProductCheckoutWait = settings.Bind("NPC", "ProductCheckoutWait", 0.01f);
            Settings.npcProductCheckoutWait = npcProductCheckoutWait.Value;

            var npcProductItemPlaceWait = settings.Bind("NPC", "ProductItemPlaceWait", 0.01f);
            Settings.npcProductItemPlaceWait = npcProductItemPlaceWait.Value;

            var npcSpeed = settings.Bind("NPC", "Speed", 5f);
            Settings.npcSpeed = npcSpeed.Value;

            var npcAcceleration = settings.Bind("NPC", "Acceleration", 50f);
            Settings.npcAcceleration = npcAcceleration.Value;

            var npcAngularSpeed = settings.Bind("NPC", "AngularSpeed", 1000f);
            Settings.npcAngularSpeed = npcAngularSpeed.Value;
        }

        internal static void Save()
        {
            var settings = new ConfigFile(Path.Combine(Paths.ConfigPath, "ENZIO.cfg"), true);

            // General
            var duping = settings.Bind("General", "ActivateDuping", false);
            duping.Value = Settings.duping;

            var dupingAmount = settings.Bind("General", "DupingAmount", 2);
            dupingAmount.Value = Settings.dupingAmount;

            var editDecorationPlacement = settings.Bind("General", "EditDecorationPlacement", false);
            editDecorationPlacement.Value = Settings.editDecorationPlacement;


            // Player
            var playerMoveSpeed = settings.Bind("Player", "MoveSpeed", 5f);
            playerMoveSpeed.Value = Settings.playerMoveSpeed;

            var playerSprintSpeed = settings.Bind("Player", "SprintSpeed", 10f);
            playerSprintSpeed.Value = Settings.playerSprintSpeed;


            // Employee
            var editEmployees = settings.Bind("Employee", "EditEmployees", false);
            editEmployees.Value = Settings.editEmployees;

            var maxEmployees = settings.Bind("Employee", "MaxEmployees", 10);
            maxEmployees.Value = Settings.maxEmployees;

            var employeeItemPlaceWait = settings.Bind("Employee", "ItemPlaceWait", 0.01f);
            employeeItemPlaceWait.Value = Settings.employeeItemPlaceWait;

            var extraEmployeeSpeedFactor = settings.Bind("Employee", "SpeedFactor", 0.5f);
            extraEmployeeSpeedFactor.Value = Settings.extraEmployeeSpeedFactor;

            var employeeSpeed = settings.Bind("Employee", "Speed", 5f);
            employeeSpeed.Value = Settings.employeeSpeed;

            var employeeAcceleration = settings.Bind("Employee", "Acceleration", 50f);
            employeeAcceleration.Value = Settings.employeeAcceleration;

            var employeeAngularSpeed = settings.Bind("Employee", "AngularSpeed", 1000f);
            employeeAngularSpeed.Value = Settings.employeeAngularSpeed;


            // NPC
            var editCustomers = settings.Bind("NPC", "EditCustomers", false);
            editCustomers.Value = Settings.editCustomers;

            var editNpcs = settings.Bind("NPC", "EditNpcs", false);
            editNpcs.Value = Settings.editNpcs;

            var npcProductCheckoutWait = settings.Bind("NPC", "ProductCheckoutWait", 0.01f);
            npcProductCheckoutWait.Value = Settings.npcProductCheckoutWait;

            var npcProductItemPlaceWait = settings.Bind("NPC", "ProductItemPlaceWait", 0.01f);
            npcProductItemPlaceWait.Value = Settings.npcProductItemPlaceWait;

            var npcSpeed = settings.Bind("NPC", "Speed", 5f);
            npcSpeed.Value = Settings.npcSpeed;

            var npcAcceleration = settings.Bind("NPC", "Acceleration", 50f);
            npcAcceleration.Value = Settings.npcAcceleration;

            var npcAngularSpeed = settings.Bind("NPC", "AngularSpeed", 1000f);
            npcAngularSpeed.Value = Settings.npcAngularSpeed;
        }
        internal static void PrintAll()
        {
            Plugin.Logger.LogWarning($"Duping: {duping}");
            Plugin.Logger.LogWarning($"Duping Amount: {dupingAmount}");
            Plugin.Logger.LogWarning($"Decoration Placement: {editDecorationPlacement}");
            Plugin.Logger.LogWarning($"Player Move Speed: {playerMoveSpeed}");
            Plugin.Logger.LogWarning($"Player Sprint Speed: {playerSprintSpeed}");
            Plugin.Logger.LogWarning($"EditEmployees: {editEmployees}");
            Plugin.Logger.LogWarning($"Max Employees: {maxEmployees}");
            Plugin.Logger.LogWarning($"Eployee Item Place Wait: {employeeItemPlaceWait}");
            Plugin.Logger.LogWarning($"Employee Speed Factor: {extraEmployeeSpeedFactor}");
        }
    }
}
