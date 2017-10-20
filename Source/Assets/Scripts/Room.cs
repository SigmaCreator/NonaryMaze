using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour {

    [SerializeField]
    List<Player> _occupants = new List<Player>();

    [SerializeField]
    List<Door> _doors = new List<Door>();

    public void Awake()
    {
        // initializes the room
    }

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

    public bool RemovePlayers(List<Player> selection) {

        foreach (Player p in selection) {
            if (!_occupants.Contains(p)) {
                return false;
            }
        }

        _occupants.RemoveAll(selection.Contains);
        SettlePlayers();
        return true;

    }

    public bool AddPlayers(List<Player> selection)
    {
        _occupants.AddRange(selection);
        return true;
    }

    public void SetDoors(List<Door> doors) {
        _doors.Clear();  
        _doors.AddRange(doors);
        SettlePlayers();
    }

    // Settles players inside the room.
    // Positions them in a way they fit comfortably
    public void SettlePlayers() { }

}
