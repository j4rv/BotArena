using UnityEngine;

namespace BotArena
{
    class RandomUtil
    {
        static readonly float MAP_DIMENSIONS = 13f;

        protected RandomUtil(){}

        public static Vector3 RandomPositionInsideMap() {
            return new Vector3(Random.Range(-MAP_DIMENSIONS, MAP_DIMENSIONS), 0.01f, Random.Range(-MAP_DIMENSIONS, MAP_DIMENSIONS));
        }

        public static Vector3 RandomRotationHorizontal() {
            return new Vector3(0, Random.Range(0, 360), 0);
        }
    }
}
