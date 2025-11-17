using System;
using UnityEngine;

namespace ColorSorting3D.Util
{
    public static class TransfromUtils
    {
        public static Transform GetParent(this Transform transform, int parentIndex)
        {
            if (parentIndex == 0)
                return transform;

            return transform.parent.GetParent(parentIndex - 1);
        }

        public static void SetActiveParent(this Transform transform, int parentIndex, bool enable)
        {
            if (parentIndex == 0)
            {
                transform.gameObject.SetActive(enable);
                return;
            }

            transform.parent.SetActiveParent(parentIndex - 1, enable);
        }

        public static Vector3 GetTopRightPos(this RectTransform rect)
        {
            Vector3 offsetPos = new(rect.rect.width / 2, rect.rect.height / 2, 0);
            return rect.position + offsetPos;
        }
    }
}