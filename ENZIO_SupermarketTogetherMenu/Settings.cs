namespace ENZIO
{
    internal class Settings
    {
        public const string GUID = "org.bepinex.plugins.enziosupermarkettogether";
        public const string Name = "ENZIO - Supermarket Together Menu";
        public const string Version = "0.0.1";

        public static bool duping = true;
        public static int dupingAmount = 10;

        public static float playerMoveSpeed = 10f;
        public static float playerSprintSpeed = 15f;

        public static bool editEmployeeSpeed = true;
        public static int maxEmployees = 10;
        public static float employeeItemPlaceWait = 0.01f;
        public static float extraEmployeeSpeedFactor = 0.5f;

        public static bool editCustomerSpeed = true;
        public static bool editNpcSpeed = false;
        public static float npcProductCheckoutWait = 0.01f;
        public static float npcProductItemPlaceWait = 0.01f;
        public static float npcSpeed = 10f;
        public static float npcAcceleration = 50f;
        public static float npcAngularSpeed = 1000f;
    }
}
