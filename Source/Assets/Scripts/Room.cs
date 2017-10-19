using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : ScriptableObject {

    List<Player> _occupants = new List<Player>();
    List<Door> _doors = new List<Door>();
    
    public IEnumerator<Player> Occupants() {
        foreach (Player p in _occupants) {
            yield return p;
        }
    }

    public IEnumerator<Door> Doors()
    {
        foreach (Door p in _doors)
        {
            yield return p;
        }
    }

    bool RemovePlayers(List<Player> selection) {

        foreach (Player p in selection) {
            if (!_occupants.Contains(p)) {
                return false;
            }
        }

        _occupants.RemoveAll(selection.Contains);
        return true;

    }

    bool AddPlayers(List<Player> selection)
    {
        _occupants.AddRange(selection);
        return true;
    }

    void SetDoors(List<Door> doors) {
        _doors.Clear();  
        _doors.AddRange(doors);
    }

}
