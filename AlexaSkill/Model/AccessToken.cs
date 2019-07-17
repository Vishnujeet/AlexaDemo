using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectPlay.AlexaSkill.Model
{
    public class AccessToken
    {
        public string token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }

    }
}
