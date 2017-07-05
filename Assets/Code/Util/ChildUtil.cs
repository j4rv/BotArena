﻿using UnityEngine;

namespace BotArena
{
    public static class ChildUtil
    {
        public static Transform FindChildWithTag(this GameObject parent, string tag) {
            Transform t = parent.transform;
            Transform res = null;

            foreach (Transform transform in t) {
                if (transform.tag == tag) {
                    res = transform;
                    break;
                }
            }

            return res;
        }
    }
}
