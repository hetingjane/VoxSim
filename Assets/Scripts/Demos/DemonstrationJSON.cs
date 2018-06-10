using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demonstration
{
    [Serializable]
    public class DemonstrationObject
    {
        public Demonstration[] Away { get; set; }
        public Demonstration[] Closer { get; set; }
        public Demonstration[] Past { get; set; }
        public Demonstration[] Around { get; set; }
        public Demonstration[] Next { get; set; }
    }

    [Serializable]
    public class Demonstration
    {
        public float[][] action_storage;
        public float[][] start_config;

        override public string ToString()
        {
            return "action_storage: " + getFirstStart().x + " , " + getFirstStart().y + " ; start_config: " + getSecondStart().x + " , " + getSecondStart().y;
        }

        public Vector2 getFirstStart()
        {
            return new Vector2(start_config[0][0], start_config[0][1]);
        }

        public Vector2 getSecondStart()
        {
            return new Vector2(start_config[1][0], start_config[1][1]);
        }

        public int getNoOfActions ()
        {
            return action_storage.Length;
        }

        public Vector2 getAction ( int step )
        {
            if (step < 0 || step >= getNoOfActions() ) throw new ArgumentOutOfRangeException();
            return new Vector2(action_storage[step][0], action_storage[step][1]); 
        }
    }
}