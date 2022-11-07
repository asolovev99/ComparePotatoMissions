using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparePotatoMissions.Classes
{
    public class Waveprogress
    {
        public string mission { get; set; }
        public string missionNiceName { get; set; }
        public string map { get; set; }
        public string mapNiceName { get; set; }
        public string imageUrl { get; set; }
        public int difficulty { get; set; }
        public int waveCount { get; set; }
        // List of completed waves
        public bool[] waveProgress { get; set; }
        public int wavesCompletedBitmask { get; set; }
    }

    class PlayerInfo
    {
        public int completedMissions { get; set; }
        //     List of missions
        public Waveprogress[] waveProgress { get; set; }
        public long playerId;
    }
}