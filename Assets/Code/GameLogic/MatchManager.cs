using System.Collections.Generic;
using UnityEngine;

namespace BotArena
{
    public class MatchManager : MonoBehaviour
    {
        private List<IRobot> robots;

        private static MatchManager instance;

        private static MatchManager Get()
        {
            if (instance == null)
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
            return instance;
        }

        private void Start()
        {
            robots = new List<IRobot>();

            //robots.Add()
        }

        

        void FixedUpdate()
        {
            
        }

        private void InstantiateRobotFromDll(string dllPath)
        {
            IRobot robot = DllUtil.LoadRobotFromDll(dllPath);
            string name = robot.GetName();

        }

    }
}