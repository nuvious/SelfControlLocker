using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SelfControl
{
    [Serializable]
    class SelfControlConfig : Dictionary<int, DailyConfig>
    {
        public SelfControlConfig() : base()
        {
            for(int i = 0; i < 7; i++)
            {
                Add(i, new DailyConfig());
            }
        }
    }

    [Serializable]
    class DailyConfig
    {
        [JsonIgnore]
        public const int DEFAULT_DELAY = 30000;

        [JsonProperty("delay")]
        private int delay = DEFAULT_DELAY;

        /// <summary>
        /// Start hour in the range of [0,23]
        /// </summary>
        [JsonProperty("start")]
        public int Start { get; set; }

        /// <summary>
        /// End hour in the range of [0,23]
        /// </summary>
        [JsonProperty("end")]
        public int End { get; set; }

        /// <summary>
        /// Delay in milliseconds betweeen attempts to lock the screen.
        /// Default is 30k milleseconds (30 seconds).
        /// </summary>
        [JsonIgnore]
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;
            }
        }
    }
}
