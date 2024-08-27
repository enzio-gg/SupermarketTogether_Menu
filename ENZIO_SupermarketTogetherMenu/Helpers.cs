using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace ENZIO
{
    internal class Helpers
    {
        internal static bool IsMainSceneLoaded()
        {
            return SceneManager.GetActiveScene().buildIndex == 1 && SceneManager.GetActiveScene().name == "B_Main" && SceneManager.GetActiveScene().isLoaded;
        }
        internal static bool inLobby()
        {
            SteamLobby steamLobby = SteamLobby.Instance;
            return steamLobby != null && steamLobby.CurrentLobbyID > 0UL;
        }

        internal static void UpdateEmployeeStats()
        {
            if (Vars.npcManager == null || !Settings.editEmployees) return;

            Vars.npcManager.extraEmployeeSpeedFactor = Settings.extraEmployeeSpeedFactor;
            // if (Settings.maxEmployees < Vars.npcManager.maxEmployees) DespawnEmployees();
            Vars.npcManager.maxEmployees = Settings.maxEmployees;
            Vars.npcManager.UpdateEmployeesNumberInBlackboard();

            NavMeshAgent[] employeesNavMeshAgent = Vars.npcManager.employeeParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (employeesNavMeshAgent != null && employeesNavMeshAgent.Length > 0)
            {

                foreach (NavMeshAgent employeeNavMeshAgent in employeesNavMeshAgent)
                {
                    employeeNavMeshAgent.speed = Settings.npcSpeed;
                    employeeNavMeshAgent.acceleration = Settings.npcAcceleration;
                    employeeNavMeshAgent.angularSpeed = Settings.npcAngularSpeed;
                }
            }

            NPC_Info[] employeesInfo = Vars.npcManager.employeeParentOBJ.GetComponentsInChildren<NPC_Info>();
            if (employeesInfo != null && employeesInfo.Length > 0)
            {

                foreach (NPC_Info employeeInfo in employeesInfo)
                {
                    employeeInfo.employeeItemPlaceWait = Settings.employeeItemPlaceWait;
                }
            }

            return;
        }
        // Doesn't work currently
        internal static void DespawnEmployees()
        {
            if (Vars.npcManager == null) return;
            while (Vars.npcManager.employeeParentOBJ.transform.childCount > Settings.maxEmployees)
            {
                Object.Destroy(Vars.npcManager.employeeParentOBJ.transform.GetChild(Vars.npcManager.employeeParentOBJ.transform.childCount - 1).gameObject);
            }
            return;
        }
        internal static void UpdateCustomerStats()
        {
            if (Vars.npcManager == null || !Settings.editCustomers) return;

            NavMeshAgent[] customersNavMeshAgent = Vars.npcManager.customersnpcParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (customersNavMeshAgent != null && customersNavMeshAgent.Length > 0)
            {

                foreach (NavMeshAgent customerNavMeshAgent in customersNavMeshAgent)
                {
                    customerNavMeshAgent.speed = Settings.npcSpeed;
                    customerNavMeshAgent.acceleration = Settings.npcAcceleration;
                    customerNavMeshAgent.angularSpeed = Settings.npcAngularSpeed;
                }
            }

            NPC_Info[] customersInfo = Vars.npcManager.customersnpcParentOBJ.GetComponentsInChildren<NPC_Info>();
            if (customersInfo != null && customersInfo.Length > 0)
            {
                foreach (NPC_Info customerInfo in customersInfo)
                {
                    customerInfo.productCheckoutWait = Settings.npcProductCheckoutWait;
                    customerInfo.productItemPlaceWait = Settings.npcProductItemPlaceWait;
                }
            }
            return;
        }
    }
}
