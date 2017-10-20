﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMazeFactory : IMazeFactory
{


    public Maze GenerateMaze()
    {
        System.Type mazeType = typeof(Maze);
        System.Type doorRuleType = typeof(DummyDoorRule);

        #region Prefabs
        GameObject doorPrefab = (GameObject)Resources.Load("Prefabs/Door");
        GameObject playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        GameObject roomPrefab = (GameObject)Resources.Load("Prefabs/Room");
        #endregion

        Maze maze = (Maze)ScriptableObject.CreateInstance(mazeType);

        #region Maze Initializers
        List<Room> rooms = new List<Room>();
        List<Door> doors = new List<Door>();
        List<Player> players = new List<Player>();
        #endregion


        #region Initializes Rooms
        Room start = GameObject.Instantiate(roomPrefab).GetComponent<Room>();
        Room goal = GameObject.Instantiate(roomPrefab).GetComponent<Room>();
        start.name = "RoomStart";
        goal.name = "RoomGoal";

        start.gameObject.transform.position = new Vector3(0f, (-640 / 128) , 0f);
        goal.gameObject.transform.position = new Vector3(0f, (-640 / 128) + 4 * 2.5f, 0f);

        maze.Start = start;
        maze.Goal = goal;

        rooms.Add(start);

        for (int i = 1; i < 4; i++)
        {
            Room room = GameObject.Instantiate(roomPrefab).GetComponent<Room>();
            room.name = "Room" + i;
            room.gameObject.transform.position = new Vector3(0f, (-640 / 128) + (i)*2.5f , 0f);
            rooms.Add(room) ;
        }

        rooms.Add(goal);
        #endregion

        #region Initializes Doors
        IDoorOpeningRule doorRule = (IDoorOpeningRule)ScriptableObject.CreateInstance(doorRuleType);

        //                     1  2  3  4  5  6  7  8  9
        int[] destinations = { 4, 4, 3, 2, 2, 4, 3, 3, 5 };

        for (int i = 1; i < 10; i++)
        {
            Door door = GameObject.Instantiate(doorPrefab).GetComponent<Door>();
            door.name = "Door" + i;
            door.Code = i;
            door.Rule = doorRule;
            door.Destination = rooms[destinations[i-1]-1];
            door.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ph_Door" + i);
            doors.Add(door);
        }

        // which room has which doors.
        // Could recieve this by a parameter.
        int[][] roomDoors = { new int[]{4, 5},      //0
                              new int[]{3, 7, 8},   //1
                              new int[]{2, 6, 1},   //2
                              new int[]{9},         //3
        };
        #endregion

        #region Links Rooms and Doors
        for(int i = 0; i < 4; i++)
        {
            List<Door> newDoors = new List<Door>();

            foreach (int j in roomDoors[i]) { newDoors.Add(doors[j-1]); }

            rooms[i].SetDoors(newDoors);

        }
        #endregion


        #region Initializes Players
        for (int i = 1; i < 10; i++)
        {
            Player player = GameObject.Instantiate(playerPrefab).GetComponent<Player>();
            player.Code = i;
            player.name = "Player" + i;
            player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ph_Player"+i);
            players.Add(player);

        }

        start.AddPlayers(players);
        #endregion

        maze.InitMaze(players, doors, rooms);

        return maze;
    }
}
