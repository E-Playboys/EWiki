﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiApp.DataAccess.Models
{
    public class Move
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public float Power { get; set; }

        public float Cooldown { get; set; }

        public int Energy { get; set; }

        public string Category { get; set; }

        public float DPS { get; set; }

        public float WithSTAB { get; set; }
    }
}