using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;
using Camera = UnityEngine.Camera;
using RPlayer = Rewired.Player;
using System;
using HighlightPlus;
using Rewired.UI.ControlMapper;

namespace ENZIO;

[BepInPlugin(Settings.Guid, Settings.author, Settings.version)]
[BepInProcess("Supermarket Together.exe")]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private void Awake()
    {
        if (!SteamManager.Initialized) return;

        Logger = base.Logger;
        Logger.LogMessage($"{Settings.name} loaded");

        Settings.LoadSettings();
        Logger.LogInfo($"[Settings] Loaded");

        Harmony.CreateAndPatchAll(typeof(PatchPlayerNetwork));
        Logger.LogInfo($"[Patched] Player");

        Harmony.CreateAndPatchAll(typeof(PatchUpdateEmployeeStats));
        Harmony.CreateAndPatchAll(typeof(PatchSpawnEmployee));
        Logger.LogInfo($"[Patched] Employee");

        Harmony.CreateAndPatchAll(typeof(PatchNPCManager));
        Harmony.CreateAndPatchAll(typeof(PatchNPCInfo));
        Logger.LogInfo($"[Patched] NPC");

        Harmony.CreateAndPatchAll(typeof(PatchBuilderDecoration));
        Logger.LogInfo($"[Patched] Decoration");
    }

    private void Start() { }

    private void Update()
    {
        if (!Helpers.IsMainSceneLoaded()) return;        

        if (Input.GetKeyDown(KeyCode.F1)) IGUI.showGui = !IGUI.showGui;

        if (!IGUI.showGui && !IGUI.showGuiLastState) return;
        if (IGUI.showGui && !IGUI.showGuiLastState)
        {
            IGUI.showGuiLastState = true;

            IGUI.originalLockState = Cursor.lockState;
            IGUI.originalVisible = Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Vars.firstPersonController != null)
                Vars.firstPersonController.enabled = false;
        }
        if (!IGUI.showGui && IGUI.showGuiLastState)
        {
            IGUI.showGuiLastState = false;

            Cursor.lockState = IGUI.originalLockState;
            Cursor.visible = IGUI.originalVisible;
            if (Vars.firstPersonController != null)
                Vars.firstPersonController.enabled = true;
        }
    }

    private void OnGUI()
    {
        if (!IGUI.windows_initialized)
        {
            IGUI.SetupWindows();
            Logger.LogInfo($"[Created] Windows");
        }
        if (!Helpers.IsMainSceneLoaded() || !IGUI.showGui) return;

        IGUI.ShowWindow("main", ref IGUI.mainWindow);
    }
}


// All Patches 
[HarmonyPatch(typeof(PlayerNetwork), "Update")]
class PatchPlayerNetwork
{
    [HarmonyPrefix]
    static bool Update(PlayerNetwork __instance, RPlayer ___MainPlayer)
    {
        if (!Helpers.IsMainSceneLoaded()) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;
        RPlayer player = ___MainPlayer;
        if (player == null) return true;
        if (!__instance.isLocalPlayer) return true;

        if(Vars.firstPersonController == null) Vars.firstPersonController = Object.FindFirstObjectByType<FirstPersonController>();
        if (Vars.firstPersonController)
        {
            Vars.firstPersonController.MoveSpeed = Settings.playerMoveSpeed;
            Vars.firstPersonController.SprintSpeed = Settings.playerSprintSpeed;
        }

        if (player.GetButtonDown("Drop Item") && __instance.equippedItem > 0 && Settings.duping)
        {
            Vector3 vector = Camera.main.transform.position + Camera.main.transform.forward * 3.5f;

            for (int i = 0; i < Int32.Parse(Settings.dupingAmount); i++)
                switch (__instance.equippedItem)
                {
                    case 0:
                        break;
                    case 1:
                        __instance.CmdChangeEquippedItem(0);
                        GameData.Instance.GetComponent<ManagerBlackboard>().CmdSpawnBoxFromPlayer(vector, __instance.extraParameter1, __instance.extraParameter2, __instance.transform.rotation.eulerAngles.y);
                        break;
                    case 2:
                        GameData.Instance.GetComponent<NetworkSpawner>().CmdSpawnProp(2, vector, new Vector3(0f, 0f, 90f));
                        break;
                    case 3:
                        GameData.Instance.GetComponent<NetworkSpawner>().CmdSpawnProp(3, vector, new Vector3(270f, 0f, 0f));
                        break;
                    case 4:
                        GameData.Instance.GetComponent<NetworkSpawner>().CmdSpawnProp(4, vector, new Vector3(270f, 0f, 0f));
                        break;
                    case 5:
                        GameData.Instance.GetComponent<NetworkSpawner>().CmdSpawnProp(5, vector, new Vector3(270f, 0f, 0f));
                        break;
                    default:
                        Plugin.Logger.LogInfo("Equipped item error");
                        break;
                }
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(NPC_Manager), "FixedUpdate")]
class PatchNPCManager
{
    [HarmonyPrefix]
    static bool FixedUpdate(NPC_Manager __instance)
    {
        if (!Helpers.IsMainSceneLoaded()) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;

        if(Vars.npcManager == null) Vars.npcManager = __instance;

        __instance.extraEmployeeSpeedFactor = Settings.extraEmployeeSpeedFactor;
        __instance.maxEmployees = Settings.maxEmployees;
        __instance.UpdateEmployeesNumberInBlackboard();

        if (Settings.editCustomerSpeed)
        {
            NavMeshAgent[] customersNavMeshAgent = __instance.customersnpcParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (customersNavMeshAgent == null || customersNavMeshAgent.Length <= 0) return true;
            foreach (NavMeshAgent customerNavMeshAgent in customersNavMeshAgent)
            {
                customerNavMeshAgent.speed = Settings.npcSpeed;
                customerNavMeshAgent.acceleration = Settings.npcAcceleration;
                customerNavMeshAgent.angularSpeed = Settings.npcAngularSpeed;
            }
        }
        if (Settings.editNpcSpeed)
        {
            NavMeshAgent[] npcsNavMeshAgent = __instance.dummynpcParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (npcsNavMeshAgent == null || npcsNavMeshAgent.Length <= 0) return true;
            foreach (NavMeshAgent npcNavMeshAgent in npcsNavMeshAgent)
            {
                npcNavMeshAgent.speed = Settings.npcSpeed;
                npcNavMeshAgent.acceleration = Settings.npcAcceleration;
                npcNavMeshAgent.angularSpeed = Settings.npcAngularSpeed;
            }
        }

        return true;
    }
}

[HarmonyPatch(typeof(NPC_Manager), "UpdateEmployeeStats")]
class PatchUpdateEmployeeStats
{
    [HarmonyPrefix]
    static bool UpdateEmployeeStats()
    {
        if (!Helpers.IsMainSceneLoaded() || !Settings.editEmployeeSpeed) return true;

        Helpers.UpdateEmployeeStats();
        return false;
    }
}

[HarmonyPatch(typeof(NPC_Manager), "SpawnEmployee")]
class PatchSpawnEmployee
{
    [HarmonyPostfix]
    static void SpawnEmployee()
    {
        if (!Helpers.IsMainSceneLoaded() || !Settings.editEmployeeSpeed) return;

        Helpers.UpdateEmployeeStats();
    }
}

[HarmonyPatch(typeof(NPC_Info), "FixedUpdate")]
class PatchNPCInfo
{
    [HarmonyPrefix]
    static bool FixedUpdate(NPC_Info __instance, Animator ___npcAnimator)
    {
        if (!Helpers.IsMainSceneLoaded()) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;

        __instance.employeeItemPlaceWait = Settings.employeeItemPlaceWait;
        __instance.productCheckoutWait = Settings.npcProductCheckoutWait;
        __instance.productItemPlaceWait = Settings.npcProductItemPlaceWait;

        return true;
    }
}

[HarmonyPatch(typeof(Builder_Decoration), "Update")]
class PatchBuilderDecoration
{
    [HarmonyPostfix]
    static void Update(Builder_Decoration __instance, GameObject ___dummyOBJ, RPlayer ___MainPlayer, int ___currentIndex, LayerMask ___lMask)
    {
        if (!Helpers.IsMainSceneLoaded()) return;
        if (!__instance || !__instance.isActiveAndEnabled) return;

        if (___dummyOBJ)
        {
            ___dummyOBJ.GetComponent<HighlightEffect>().glowHQColor = Color.green;
            if (___MainPlayer.GetButtonDown("Build"))
            {
                GameData.Instance.GetComponent<NetworkSpawner>().CmdSpawnDecoration(___currentIndex, ___dummyOBJ.transform.position, ___dummyOBJ.transform.rotation.eulerAngles);
            }
            if (___MainPlayer.GetButtonDown("Main Action"))
            {
                ___dummyOBJ.transform.rotation = Quaternion.Euler(___dummyOBJ.transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f));
            }
            if (___MainPlayer.GetButtonDown("Secondary Action"))
            {
                ___dummyOBJ.transform.rotation = Quaternion.Euler(___dummyOBJ.transform.rotation.eulerAngles - new Vector3(0f, 90f, 0f));
            }
            RaycastHit raycastHit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 4f, ___lMask))
            {
                ___dummyOBJ.transform.position = raycastHit.point;
                return;
            }
            ___dummyOBJ.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 4f;
        }
    }
}
