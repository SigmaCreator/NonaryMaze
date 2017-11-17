using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMazeFactory : IMazeFactory
{


    /* Door - Room relation arrays can easily be reassembled into parameters
       for maze generation. We just need more accurate rules base generation on.

        Generates a straight corridor instance of the problem.
        Same as the inspiring problem.

        Maze is a sequence of 5 connected rooms, connected by some door.

     */


    public Maze GenerateMaze()
    {
        int[][] doorRelation = GenerateRelation();
        //Debug.Log(doorRelation[0][0]);


        for(int i = 0; i < doorRelation.Length; i++)
        {
            string s = "";
            foreach (int  j in doorRelation[i])
            {
                s += " " + j;
            }
            Debug.Log(s);
        }

        return SetUpMaze(doorRelation);
    }


    private int[][] GenerateRelation()
    {
        int[][] relation =  new int[4][];

        int roomCount = 0;

        List<int> doorSelection = new List<int>();

        while(roomCount < 4)
        {
            while(DigitalRoot(doorSelection) != 9 || doorSelection.Count < 1 || doorSelection.Count > 3)
            {
                int newDoor = Random.Range(1, 10);
                if (doorSelection.Contains(newDoor))
                {
                    continue;
                }
                doorSelection.Add(newDoor);

                if (doorSelection.Count > 3)
                {
                    doorSelection.Clear();
                }
            }

            relation[roomCount] = new int[doorSelection.Count];

            for(int i = 0; i < doorSelection.Count; i++)
            {
                relation[roomCount][i] = doorSelection[i];
            }

            ++roomCount;
            doorSelection.Clear();
        }
        
        return relation;
    } 

    private int DigitalRoot(List<int> selection)
    {
        int pHash = 0;
        foreach (int p in selection) { pHash += p; }
        pHash %= 9;
        pHash = pHash == 0 ? 9 : pHash;

        return pHash;
    }

    // Requires arrays representing the destination of each door and origin of each door.
    // Each room has to have doors with DR(sum(doorcodes)) = 9
    public Maze SetUpMaze(int[][] doorRelation) {
        System.Type mazeType = typeof(Maze);
        System.Type digitalRuleType = typeof(DigitalRootRule);
        System.Type threeOrFiveRuleType = typeof(ThreeOrFiveRule);


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
        start.name = "StartRoom";
        goal.name = "GoalRoom";

        start.gameObject.transform.position = new Vector3(0f, -4f, 0f);

        maze.Start = start;
        maze.Goal = goal;

        rooms.Add(start);

        for (int i = 1; i < 4; i++)
        {
            Room room = GameObject.Instantiate(roomPrefab).GetComponent<Room>();
            room.name = "Room" + i;
            rooms.Add(room);
        }

        rooms.Add(goal);


        for (int i = 1; i < rooms.Count; i++)
        {
            rooms[i].transform.position = rooms[i - 1].transform.Find("RoomAnchor").position;
        }

        #endregion

        #region Initializes Doors and Room Links
        IDoorOpeningRule doorRule = (IDoorOpeningRule)ScriptableObject.CreateInstance(digitalRuleType);
        IDoorOpeningRule doorRule2 = (IDoorOpeningRule)ScriptableObject.CreateInstance(threeOrFiveRuleType);
        doorRule.CompositeRule = doorRule2;


        for (int i = 0; i < 4; i++)
        {
            List<Door> newDoors = new List<Door>();

            foreach (int j in doorRelation[i])
            {
                Door door = GameObject.Instantiate(doorPrefab).GetComponent<Door>();
                door.name = "Door" + j;
                door.Code = j;
                door.Rule = doorRule;
                door.Destination = rooms[i+1];
                door.Origin = rooms[i];
                door.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Door" + j);
                doors.Add(door);
                newDoors.Add(door);
            }
            rooms[i].SetDoors(newDoors);
        }


        #endregion

        #region Initializes Players

        // Shuffles the player list for fun.
        List<int> playerRandomizer = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = 0; i < playerRandomizer.Count; i++)
        {
            int rand = Random.Range(0, 8);

            if (rand == i) { continue; }
            playerRandomizer[i] += playerRandomizer[rand];
            playerRandomizer[rand] = playerRandomizer[i] - playerRandomizer[rand];
            playerRandomizer[i] -= playerRandomizer[rand];
        }


        for (int i = 1; i < 10; i++)
        {
            Player player = GameObject.Instantiate(playerPrefab).GetComponent<Player>();
            player.Code = i;
            player.name = "Player" + i;

            Sprite[] sprites = new Sprite[2];
            sprites[0] = Resources.Load<Sprite>("Sprites/Player" + playerRandomizer[i - 1]);
            sprites[1] = Resources.Load<Sprite>("Sprites/Player" + playerRandomizer[i - 1] + "Idle");

            player.Sprites = sprites;

            players.Add(player);

        }

        start.AddPlayers(players);
        #endregion

        maze.InitMaze(players, doors, rooms);

        return maze;
    }
}
