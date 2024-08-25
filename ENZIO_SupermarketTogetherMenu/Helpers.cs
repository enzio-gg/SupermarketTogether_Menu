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
            if (Vars.npcManager == null) return;

            NavMeshAgent[] employeesNavMeshAgent = Vars.npcManager.employeeParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (employeesNavMeshAgent == null || employeesNavMeshAgent.Length <= 0) return;

            foreach (NavMeshAgent employeeNavMeshAgent in employeesNavMeshAgent)
            {
                employeeNavMeshAgent.speed = Settings.npcSpeed;
                employeeNavMeshAgent.acceleration = Settings.npcAcceleration;
                employeeNavMeshAgent.angularSpeed = Settings.npcAngularSpeed;
            }
            return;
        }
    }
}
