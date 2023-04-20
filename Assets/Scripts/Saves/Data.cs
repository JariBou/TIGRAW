using System;
using System.Collections.Generic;
using PlayerBundle;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class Data
    {

        public int roomNumber;

        public int playerHealth;
        public int playerGold; // Might replace with souls to make more sense
        public int playerCrystals; 


    }
}