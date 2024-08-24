namespace ENZIO
{
    internal class Settings
    {
        public const string GUID = "org.bepinex.plugins.enziosupermarkettogether";
        public const string Name = "ENZIO - Supermarket Together Menu";
        public const string Version = "0.0.1";

        public static bool duping = true;
        public static int dupingAmount = 10;
        public static float employeeItemPlaceWait = 0.01f;
        public static float productCheckoutWait = 0.01f;
        public static float productItemPlaceWait = 0.01f;
        public static float speed = 5f;
        public static float acceleration = 50f;
        public static float angularSpeed = 1000f;
        public static float extraEmployeeSpeedFactor = 0.5f;
        public static int maxEmployees = 10;
        public static float moveSpeed = 10f;
        public static float sprintSpeed = 15f;
    }
}
