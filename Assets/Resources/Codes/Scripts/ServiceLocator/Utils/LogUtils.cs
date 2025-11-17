#if UNITY_EDITOR

using UnityEngine;

namespace ServiceButcator {
    public static class LogUtils
    {
        public static bool LogError(string log)
        {
            Debug.LogError(log);
            return false;
        }

        public static bool LogSuccess(string log)
        {
            Debug.Log(log);
            return true;
        }
    }
}

#endif