using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BotArena
{
    internal class RobotStatsGUIController : MonoBehaviour
    {
        [SerializeField]
        Image healthSprite;
        [SerializeField]
        Image energySprite;
        RobotController robotController;

        private static readonly int MAX = 50;

        void Start() {
            robotController = GetComponentInParent<RobotController>();
        }

        void FixedUpdate() {
            RectTransform hpRectTransform = healthSprite.rectTransform;
            RectTransform enRectTransform = energySprite.rectTransform;
            hpRectTransform.offsetMax = new Vector2(1, GetRobotHealthValue());
            enRectTransform.offsetMax = new Vector2(1, GetRobotEnergyValue());
        }

        float GetRobotHealthValue() {
            return -MAX + (robotController.health * MAX / robotController.robot.maxHealth);
        }

        float GetRobotEnergyValue() {
            return -MAX + (robotController.energy * MAX / robotController.robot.maxEnergy);
        }
    }
}