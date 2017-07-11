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

        void Start() {
            robotController = GetComponentInParent<RobotController>();
        }

        void FixedUpdate() {
            RectTransform hpRectTransform = healthSprite.rectTransform;
            //hpRectTransform.offsetMax = new Vector2( robotController.health * 0.95f / robotController.robot.maxHealth, 0);
            hpRectTransform.offsetMax = new Vector2(0.5f, 0);
        }
    }
}