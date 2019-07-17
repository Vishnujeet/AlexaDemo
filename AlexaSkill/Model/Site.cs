﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectPlay.AlexaSkill.Model
{
    public class Site
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
    }
}
