using UnityEngine;

namespace BotArena
{
    class RandomUtil
    {
        static readonly float MAP_DIMENSIONS = 13f;
        static System.Random random = new System.Random();

        protected RandomUtil(){}

        public static uint RandomSeed(){
            return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
        }

        public static Vector3 RandomPositionInsideMap() {
            return new Vector3(Random.Range(-MAP_DIMENSIONS, MAP_DIMENSIONS), 0.01f, Random.Range(-MAP_DIMENSIONS, MAP_DIMENSIONS));
        }

        public static Vector3 RandomRotationHorizontal() {
            return new Vector3(0, Random.Range(0, 360), 0);
        }
    }
}
