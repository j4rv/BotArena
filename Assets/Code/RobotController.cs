using System;
using System.Collections.Generic;
using UnityEngine;


namespace BotArena
{
    public class RobotController : MonoBehaviour
    {
        public IRobot robot;
        public GameObject head;
        public Dictionary<Command, ICommand> commands;
        public string dllPath;


        //Unity methods

        private void Start()
        {
            commands = new Dictionary<Command, ICommand>();
            robot = DLLLoader.LoadRobotFromDLL(dllPath);

            //Base commands, all robots can execute them
            RotateCommand rotateCmd = new RotateCommand(this);
            commands.Add(Command.ROTATE, rotateCmd);

            robot.commands = new HashSet<Command>(commands.Keys);
        }

        private void FixedUpdate()
        {
            UpdateRobot();
            robot.Think();
            CheckEnemyAhead();
        }

        // Robot methods

        public void UpdateRobot()
        {
            robot.position = transform.position;
            robot.rotation = transform.rotation.eulerAngles;
            robot.headRotation = head.transform.rotation.eulerAngles;
        }

        public void Execute(Command cmd, object[] args)
        {
            switch (cmd)
            {
                case Command.ROTATE:
                    RotateCommand rotate = (RotateCommand)commands[cmd];
                    float speed = (float)args[0];

                    rotate.SetSpeed(speed);
                    rotate.Execute();

                    break;
            }
        }

        public bool CanExecute(Command cmd, object[] args)
        {
            bool res = false;

            switch (cmd)
            {
                case Command.ROTATE:
                    RotateCommand rotate = (RotateCommand)commands[cmd];
                    float speed = (float)args[0];

                    rotate.SetSpeed(speed);
                    res = rotate.CanExecute();

                    break;
            }

            return res;
        }

        public void CheckEnemyAhead()
        {
            //check if there's an enemy ahead, if there is, execute robot.OnEnemyAhead()
            //robot.OnEnemyAhead();
        }
    }
}