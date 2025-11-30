using System;
using System.Collections.Generic;
using UnityEngine;

//public enum RoomType
//{
//    Enemy,
//    Treasure,
//    Empty,
//    Puzzle,
//    RestStop
//}
public class RoomGenerator
{
    public const int roomTypes = 5;
    public static List<RoomType> GenerateRoomSeq(int roomCount = 5, List<RoomType> requiredRooms = null)//generates a sequence of rooms, including those in the required list
    {
        if (requiredRooms != null)
        {
            if (requiredRooms.Count > roomCount)
            {
                throw new ArgumentException("More required rooms than total room count");
            }
        }
        List<RoomType> rooms = new List<RoomType>();
        for (int i = 0; i < roomCount; i++)
        {
            rooms.Add((RoomType)UnityEngine.Random.Range(0,roomTypes));
        }

        //This should work....
        if (requiredRooms != null)
        {
            if (requiredRooms.Count > roomCount)
            {
                throw new ArgumentException("More required rooms than total room count");
            }
            List<int> generatedNums = new List<int>();
            for (int i = 0; i < requiredRooms.Count; i++)
            {
                int rng = UnityEngine.Random.Range(0,roomCount - i);
                for (int j = 0; j < generatedNums.Count; j++)
                {
                    if (generatedNums[j] == rng)
                    {
                        rng = (rng + 1) % roomCount;
                    }
                }
                rooms[rng] = requiredRooms[i];
                generatedNums.Add(rng);
                generatedNums.Sort();
            }
        }
        return rooms;
    }
}
