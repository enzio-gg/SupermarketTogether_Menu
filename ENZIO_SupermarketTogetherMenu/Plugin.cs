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

namespace ENZIO;

[BepInPlugin(Settings.GUID, Settings.Author, Settings.Version)]
[BepInProcess("Supermarket Together.exe")]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogMessage($"Plugin [{Settings.Name}] loaded!");

        Settings.LoadSettings();
        Logger.LogInfo($"[Settings] Loaded!");


        Harmony.CreateAndPatchAll(typeof(PatchPlayerNetwork));
        Logger.LogInfo($"[Patched] Player!");

        Harmony.CreateAndPatchAll(typeof(PatchNPCManager));
        Harmony.CreateAndPatchAll(typeof(PatchNPCInfo));
        Logger.LogInfo($"[Patched] NPC!");
    }
}

[HarmonyPatch(typeof(PlayerNetwork), "Update")]
class PatchPlayerNetwork
{
    [HarmonyPrefix]
    static bool Update(PlayerNetwork __instance, RPlayer ___MainPlayer)
    {
        if (Helpers.IsMainSceneLoaded()) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;
        RPlayer player = ___MainPlayer;
        if (player == null) return true;
        if (!__instance.isLocalPlayer) return true;

        FirstPersonController firstPersonController = Object.FindFirstObjectByType<FirstPersonController>();
        if (firstPersonController)
        {
            firstPersonController.MoveSpeed = Settings.playerMoveSpeed;
            firstPersonController.SprintSpeed = Settings.playerSprintSpeed;
        }

        if (player.GetButtonDown("Drop Item") && __instance.equippedItem > 0 && Settings.duping)
        {
            Vector3 vector = Camera.main.transform.position + Camera.main.transform.forward * 3.5f;

            for (int i = 0; i < Settings.dupingAmount; i++)
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
        if (Helpers.IsMainSceneLoaded()) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;

        __instance.extraEmployeeSpeedFactor = Settings.extraEmployeeSpeedFactor;
        __instance.maxEmployees = Settings.maxEmployees;

        if(Settings.editEmployeeSpeed)
        {
            NavMeshAgent[] employeesNavMeshAgent = __instance.employeeParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (employeesNavMeshAgent == null || employeesNavMeshAgent.Length <= 0) return true;
            foreach (NavMeshAgent employeeNavMeshAgent in employeesNavMeshAgent)
            {
                employeeNavMeshAgent.speed = Settings.npcSpeed;
                employeeNavMeshAgent.acceleration = Settings.npcAcceleration;
                employeeNavMeshAgent.angularSpeed = Settings.npcAngularSpeed;
            }
        }
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

[HarmonyPatch(typeof(NPC_Info), "FixedUpdate")]
class PatchNPCInfo
{
    [HarmonyPrefix]
    static bool FixedUpdate(NPC_Info __instance, Animator ___npcAnimator)
    {
        if (Helpers.IsMainSceneLoaded()) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;

        __instance.productCheckoutWait = Settings.npcProductCheckoutWait;
        __instance.productItemPlaceWait = Settings.npcProductItemPlaceWait;

        if (__instance.isEmployee)
        {
            __instance.employeeItemPlaceWait = Settings.employeeItemPlaceWait;
        }

        return true;
    }
}
