using UnityEngine.SceneManagement;

namespace ENZIO
{
    internal class Helpers
    {
        internal static bool IsMainSceneLoaded()
        {
            return SceneManager.GetActiveScene().buildIndex == 1 && SceneManager.GetActiveScene().name == "B_Main" && !SceneManager.GetActiveScene().isLoaded;
        }
    }
}
