using System.Collections;
using UnityEngine;

namespace Butils
{
    public static class CoroutineUtils
    {
        public static void StartSafeCourotine(this MonoBehaviour starter, IEnumerator routine)
        {
            if (starter != null && starter.gameObject.activeInHierarchy)
                starter.StartCoroutine(routine);
        }
    }
}