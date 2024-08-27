using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
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

            if (Settings.maxEmployees < Vars.npcManager.maxEmployees) DespawnEmployees();
            Vars.npcManager.maxEmployees = Settings.maxEmployees;
            Vars.npcManager.UpdateEmployeesNumberInBlackboard();
            Vars.npcManager.extraEmployeeSpeedFactor = Settings.extraEmployeeSpeedFactor;

            NavMeshAgent[] employeesNavMeshAgent = Vars.npcManager.employeeParentOBJ.GetComponentsInChildren<NavMeshAgent>();
            if (employeesNavMeshAgent != null && employeesNavMeshAgent.Length > 0)
            {

                foreach (NavMeshAgent employeeNavMeshAgent in employeesNavMeshAgent)
                {
                    employeeNavMeshAgent.speed = Settings.employeeSpeed;
                    employeeNavMeshAgent.acceleration = Settings.employeeAcceleration;
                    employeeNavMeshAgent.angularSpeed = Settings.employeeAngularSpeed;
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
        internal static void DespawnEmployees()
        {
            if (Vars.npcManager == null) return;

            for (int i = Vars.npcManager.employeeParentOBJ.transform.childCount-1;i >= Settings.maxEmployees; i--)
            {
                Object.Destroy(Vars.npcManager.employeeParentOBJ.transform.GetChild(i).gameObject);
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
    public static class EditorGUILayout
    {
        private static int activeFloatField = -1;
        private static float activeFloatFieldLastValue = 0;
        private static string activeFloatFieldString = "";

        public static float FloatField(float value)
        {
            Rect pos = GUILayoutUtility.GetRect(new GUIContent(value.ToString()), GUI.skin.label, new GUILayoutOption[] { GUILayout.ExpandWidth(false), GUILayout.MinWidth(40) });
            int floatFieldID = GUIUtility.GetControlID("FloatField".GetHashCode(), FocusType.Keyboard, pos) + 1;
            if (floatFieldID == 0)
                return value;

            bool recorded = activeFloatField == floatFieldID;
            bool active = floatFieldID == GUIUtility.keyboardControl;

            if (active && recorded && activeFloatFieldLastValue != value)
            {
                activeFloatFieldLastValue = value;
                activeFloatFieldString = value.ToString();
            }

            string str = recorded ? activeFloatFieldString : value.ToString();

            string strValue = GUI.TextField(pos, str);

            if (recorded)
                activeFloatFieldString = strValue;

            bool parsed = true;
            if (strValue != value.ToString())
            {
                float newValue;
                parsed = float.TryParse(strValue, out newValue);
                if (parsed)
                    value = activeFloatFieldLastValue = newValue;
            }

            if (active && !recorded)
            {
                activeFloatField = floatFieldID;
                activeFloatFieldString = strValue;
                activeFloatFieldLastValue = value;
            }
            else if (!active && recorded)
            {
                activeFloatField = -1;
                if (!parsed)
                    value = strValue.ForceParse();
            }

            return value;
        }

        public static float FloatField(GUIContent label, float value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, label != GUIContent.none ? GUILayout.ExpandWidth(true) : GUILayout.ExpandWidth(false));
            value = FloatField(value);
            GUILayout.EndHorizontal();
            return value;
        }

        public static float ForceParse(this string str)
        {
            float value;
            if (float.TryParse(str, out value))
                return value;

            bool recordedDecimalPoint = false;
            List<char> strVal = new List<char>(str);
            for (int cnt = 0; cnt < strVal.Count; cnt++)
            {
                UnicodeCategory type = CharUnicodeInfo.GetUnicodeCategory(str[cnt]);
                if (type != UnicodeCategory.DecimalDigitNumber)
                {
                    strVal.RemoveRange(cnt, strVal.Count - cnt);
                    break;
                }
                else if (str[cnt] == '.')
                {
                    if (recordedDecimalPoint)
                    {
                        strVal.RemoveRange(cnt, strVal.Count - cnt);
                        break;
                    }
                    recordedDecimalPoint = true;
                }
            }

            if (strVal.Count == 0)
                return 0;
            str = new string(strVal.ToArray());
            if (!float.TryParse(str, out value))
                Plugin.Logger.LogError("Could not parse " + str);
            return value;
        }
    }
}
