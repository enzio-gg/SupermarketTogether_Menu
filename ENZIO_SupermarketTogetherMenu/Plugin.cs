using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;
using Camera = UnityEngine.Camera;
using Player = Rewired.Player;

namespace ENZIO;

[BepInPlugin(Settings.GUID, Settings.Name, Settings.Version)]
[BepInProcess("Supermarket Together.exe")]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin ${Settings.GUID} is loaded!");

        Harmony.CreateAndPatchAll(typeof(PatchPlayerNetwork));
        Harmony.CreateAndPatchAll(typeof(PatchNPCManager));
        Harmony.CreateAndPatchAll(typeof(PatchNPCInfo));
    }
}

[HarmonyPatch(typeof(PlayerNetwork), "Update")]
class PatchPlayerNetwork
{
    [HarmonyPrefix]
    static bool Update(PlayerNetwork __instance, Player ___MainPlayer)
    {
        if (SceneManager.GetActiveScene().buildIndex <= 0) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;
        Player player = ___MainPlayer;
        if (player == null) return true;
        if (!__instance.isLocalPlayer) return true;

        FirstPersonController firstPersonController = Object.FindFirstObjectByType<FirstPersonController>();
        if (firstPersonController)
        {
            firstPersonController.MoveSpeed = 10f;
            firstPersonController.SprintSpeed = 5f;
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
        if (SceneManager.GetActiveScene().buildIndex <= 0) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;

        __instance.extraEmployeeSpeedFactor = Settings.extraEmployeeSpeedFactor;
        __instance.maxEmployees = Settings.maxEmployees;

        //NavMeshAgent[] employeesNavMeshAgent = __instance.employeeParentOBJ.GetComponentsInChildren<NavMeshAgent>();
        //if (employeesNavMeshAgent == null || employeesNavMeshAgent.Length <= 0) return true;
        //foreach (NavMeshAgent employeeNavMeshAgent in employeesNavMeshAgent)
        //{
        //    employeeNavMeshAgent.speed = Settings.speed;
        //    employeeNavMeshAgent.acceleration = Settings.acceleration;
        //    employeeNavMeshAgent.angularSpeed = Settings.angularSpeed;
        //}

        return true;
    }
}

[HarmonyPatch(typeof(NPC_Info), "FixedUpdate")]
class PatchNPCInfo
{
    [HarmonyPrefix]
    static bool FixedUpdate(NPC_Info __instance, Animator ___npcAnimator)
    {
        if (SceneManager.GetActiveScene().buildIndex <= 0) return true;
        if (!__instance || !__instance.isActiveAndEnabled) return true;

        __instance.productCheckoutWait = Settings.productCheckoutWait;
        __instance.productItemPlaceWait = Settings.productItemPlaceWait;

        if (__instance.isEmployee)
        {
            __instance.employeeItemPlaceWait = Settings.employeeItemPlaceWait;

            //if (___npcAnimator)
            //{
            //    ___npcAnimator.speed = Settings.speed;
            //    ___npcAnimator.SetFloat("MoveFactor", Settings.speed);
            //}
        }

        return true;
    }
}

[HarmonyPatch(typeof(NPC_Speed), "Start")]
class PatchNPCSpeed
{
    [HarmonyPrefix]
    static bool Start(NPC_Speed __instance)
    {
        __instance.velocity = Settings.speed;

        return false;
    }
}
