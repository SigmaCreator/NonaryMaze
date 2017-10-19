using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : ScriptableObject {

    List<Player> _occupants = new List<Player>();
    List<Door> _doors = new List<Door>();
    List<Room> _rooms = new List<Room>();

    readonly Room Start;
    readonly Room Goal;

}
