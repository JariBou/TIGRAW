using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic
{
    public class RoomPathGenerator
    {
        private int _currRoomIndex = -1; // -1 = Lobby
        private List<String> _listOfRooms = new();
        private GameManager _gm;
        private int _roomAmount;
        private int _patternRep;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gm">Game Manager</param>
        /// <param name="roomAmount">Amount of rooms before Boss</param>
        /// <param name="patternRep">Number of times to repeat pattern (number of bosses)</param>
        /// <param name="infinite"></param>
        public RoomPathGenerator(GameManager gm, int roomAmount, int patternRep, bool infinite = false) // will see about infinite later
        {
            _gm = gm;
            _roomAmount = roomAmount;
            _patternRep = patternRep;
            
            Regenerate();
        }

        public void Regenerate()
        {
            _currRoomIndex = -1;

            for (int i = 1; i < _patternRep+1; i++)
            {
                for (int j = 0; j < _roomAmount; j++)
                {
                    _listOfRooms.Add($"Arenas/Level{Random.Range(1, _gm.numberOfArenas+1)}");
                }
                _listOfRooms.Add($"Bosses/Boss{Random.Range(1, _gm.numberOfBosses+1)}_lvl{i}");
            }
            
            
            _listOfRooms.Add("WinScreen"); // Change to victory scene later
        }

        public String Next()
        {
            _currRoomIndex++;
            if (_currRoomIndex > _listOfRooms.Count)
            {
                throw new IndexOutOfRangeException();
            }
            Debug.Log($"CurrRoomIndex:{_currRoomIndex} || Value: {_listOfRooms[_currRoomIndex]}");
            return _listOfRooms[_currRoomIndex];
        }
    }
}