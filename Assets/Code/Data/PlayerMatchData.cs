using BotArena;
using System.Collections.Generic;


namespace BotArena { 
    internal class PlayerMatchData {

        private static readonly Dictionary<MatchResult, int> RESULT_POINTS = new Dictionary<MatchResult, int>() {
            { MatchResult.VICTORY, 100 },
            { MatchResult.DRAW, 50 },
            { MatchResult.LOSS, 0 }
        };

        public RobotController controller;
        public IRobot robot;

        private readonly string playerName;
        private long points;
        private List<MatchResult> matchResults;

        public PlayerMatchData(string playerName, IRobot robot, RobotController controller) {
            this.playerName = playerName;
            this.robot = robot;
            this.controller = controller;
            matchResults = new List<MatchResult>();
        }

        public void AddMatch(MatchResult result) {
            matchResults.Add(result);
            points += RESULT_POINTS[result];
        }

        public int VictoriesCount() {
            int res = 0;
            matchResults.ForEach(m => res += (m == MatchResult.VICTORY ? 1 : 0));
            return res;
        }

        public int LossesCount() {
            int res = 0;
            matchResults.ForEach(m => res += (m == MatchResult.LOSS ? 1 : 0));
            return res;
        }

        public long Points() {
            return points;
        }

    }
}