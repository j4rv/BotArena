using UnityEngine;

namespace BotArena
{
    public class RobotInfo
    {
        public float health         { get; internal set;}
        public float energy         { get; internal set;}
        public float agility        { get; internal set;}
        public Vector3 position     { get; internal set;}
        public Vector3 rotation     { get; internal set;}
        public Vector3 gunRotation  { get; internal set;}
    }
}