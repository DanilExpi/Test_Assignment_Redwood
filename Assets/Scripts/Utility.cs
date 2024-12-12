using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UtilityBase
{
    public static class Utility
    {
        public static int RandomRange(this Vector2Int vector2Int)
        {
            return Random.Range(vector2Int.x, vector2Int.y + 1);
        }

        public static float RandomRange(this Vector2 vector2)
        {
            var min = vector2.x;
            var max = vector2.y;
            var timeRatio = Random.value;
            return Mathf.Lerp(min, max, timeRatio);
        }

        public static T RandomRange<T>(this T[] tts)
        {
            var t = tts[Random.Range(0, tts.Length)];
            return t;
        }

        public static T RandomRange<T>(this List<T> tts)
        {
            var t = tts[Random.Range(0, tts.Count)];
            return t;
        }
    }
}
