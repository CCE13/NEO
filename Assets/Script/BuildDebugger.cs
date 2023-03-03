
using UnityEngine;

namespace DebugStuff
{
    public class BuildDebugger : MonoBehaviour
    {
        //#if !UNITY_EDITOR
        static string S_myLog = "";
        private string _output;
        private string _stack;

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            _output = logString;
            _stack = stackTrace;
            S_myLog = _output + "\n" + S_myLog;
            if (S_myLog.Length > 5000)
            {
                S_myLog = S_myLog.Substring(0, 4000);
            }
        }

        void OnGUI()
        {
            if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                S_myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 800, Screen.height - 800), S_myLog);
                
            }
        }
        //#endif
    }
}
