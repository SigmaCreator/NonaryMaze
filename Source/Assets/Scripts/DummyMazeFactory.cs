using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMazeFactory : IMazeFactory
{
    System.Type roomType = typeof(Room);
    System.Type mazeType = typeof(Maze);
    System.Type doorRuleType = typeof(DummyDoorRule);

    GameObject doorPrefab = (GameObject)Resources.Load("Prefabs/Door.prefab");
    GameObject playerPrefab = (GameObject)Resources.Load("Prefabs/Player.prefab");


    public Maze GenerateMaze()
    {
        Maze maze = (Maze)ScriptableObject.CreateInstance(mazeType);

        List<Room> rooms = new List<Room>();

        IDoorOpeningRule doorRule = (IDoorOpeningRule)ScriptableObject.CreateInstance(doorRuleType);
        Room start = (Room)ScriptableObject.CreateInstance(roomType);
        Room goal = (Room)ScriptableObject.CreateInstance(roomType);

        maze.Start = start;
        maze.Goal = goal;

        rooms.Add(start);

        for (int i = 1; i < 5; i++) {
            rooms.Add((Room)ScriptableObject.CreateInstance(roomType)) ;
        }

        rooms.Add(goal);

        List<Door> doors = new List<Door>();
        //                               1  2  3  4  5  6  7  8  9
        int[] destinations = new int[] { 4, 4, 3, 2, 2, 4, 3, 3, 5 };

        for (int i = 1; i < 10; i++) {
            Door door = GameObject.Instantiate(doorPrefab).GetComponent<Door>();
            door.Code = i;
            door.Rule = doorRule;
            door.Destination = rooms[destinations[i]];
            doors.Add(door);
            
        }

        return maze;
    }
}
