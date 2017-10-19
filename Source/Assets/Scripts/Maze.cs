using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : ScriptableObject {

    List<Player> _players = new List<Player>();
    List<Door> _doors = new List<Door>();
    List<Room> _rooms = new List<Room>();

    Room start;
    public Room Start { get { return start; }
                        set { start = start ?? value; } }
    Room goal;
    public Room Goal { get { return goal; }
                       set { goal = goal ?? value; } }

    public IEnumerator<Player> Players()
    {
        foreach (Player p in _players) {  yield return p;  }
    }

    public IEnumerator<Door> Doors()
    {
        foreach (Door d in _doors) { yield return d; }
    }

    public IEnumerator<Room> Rooms()
    {
        foreach (Room r in _rooms) { yield return r; }
    }

    void InitMaze(List<Player> players, List<Door> doors, List<Room> rooms) {
        _players.AddRange(players);
        _doors.AddRange(doors);
        _rooms.AddRange(rooms);



        /* Creates Prefabs 
           Based on graph Recieved
         
         */

    }



}
