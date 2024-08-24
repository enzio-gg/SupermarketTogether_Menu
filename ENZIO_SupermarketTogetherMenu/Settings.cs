namespace ENZIO
{
    internal class Settings
    {
        internal const string GUID = "org.bepinex.plugins.enziosupermarkettogether";
        internal const string Name = "ENZIO - Supermarket Together Menu";
        internal const string Version = "0.0.1";

        internal static bool duping = true;
        internal static int dupingAmount = 10;

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
    }
}
