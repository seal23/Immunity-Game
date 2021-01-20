using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class LevelManager
    {
        public static string VillageSceneName = "Village v0.2";
        static Dictionary<int, string> LevelScenesName = new Dictionary<int, string>();
        static LevelManager()
        {
            LevelScenesName.Add(1, "Level 01");
            LevelScenesName.Add(2, "Level 02");
            LevelScenesName.Add(3, "Level 02");
        }

        public static string getSceneNameByLevel(int level)
        {
            return LevelScenesName[level];
        }
    }
}
