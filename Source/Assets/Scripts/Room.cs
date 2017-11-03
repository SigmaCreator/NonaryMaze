using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour {

    [SerializeField]
    List<Player> _occupants = new List<Player>();

    [SerializeField]
    List<Door> _doors = new List<Door>();

    // Positions players can occupy in room.
    [SerializeField]
    Vector3[] _playerAnchors = new Vector3[9];

    // Cacheing informations
    Vector3 _roomScale;
    Transform _transform;

    SpriteRenderer _sprite;

    public void Awake()
    {
        _sprite = transform.GetComponent<SpriteRenderer>();

        _transform = gameObject.transform;
        _roomScale = _transform.localScale;

        // initializes the room
        float widthDelta = (_roomScale.x * 0.8f) / 5f;
        float heightDelta = (_roomScale.y * 0.5f) / 2f ;

        // Positions players can occupy in room.
        _playerAnchors[0] = new Vector3(0f, heightDelta, 0f);
        _playerAnchors[1] = new Vector3(-widthDelta, heightDelta, 0f);
        _playerAnchors[2] = new Vector3(widthDelta, heightDelta, 0f);
        _playerAnchors[3] = new Vector3(2f * (-widthDelta), heightDelta, 0f);
        _playerAnchors[4] = new Vector3(2f * widthDelta, heightDelta, 0f);

        _playerAnchors[5] = new Vector3(0.5f * (-widthDelta), -heightDelta, 0f);
        _playerAnchors[6] = new Vector3(0.5f * widthDelta, -heightDelta, 0f);
        _playerAnchors[7] = new Vector3(1.5f * (-widthDelta), -heightDelta, 0f);
        _playerAnchors[8] = new Vector3(1.5f * widthDelta, -heightDelta, 0f);

    }

    public IEnumerable<Player> Occupants() {
        foreach (Player p in _occupants) {
            yield return p;
        }
    }

    public IEnumerable<Door> Doors()
    {
        foreach (Door p in _doors)
        {
            yield return p;
        }
    }
    public bool PlayerInRoom(Player p)
    {
        return _occupants.Contains(p);
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
        SettlePlayers();

        if(this.name == "GoalRoom" && _occupants.Count == 9)
        {
            transform.GetComponent<ParticleSystem>().Play();
        }

        return true;
    }

    public void SetDoors(List<Door> doors) {
        _doors.Clear();  
        _doors.AddRange(doors);

        //TODO Should Rooms be responsible for setting the origin of doors?
        // Problably not. How should the factory do this?
        foreach (Door d in doors) {
            d.Origin = this;
        }

        SettleDoors();
    }

    // Settles players inside the room.
    // Positions them in a way they fit comfortably
    public void SettlePlayers()
    {
        for (int i = 0; i < _occupants.Count; i++)
        {
            _occupants[i].transform.position = _playerAnchors[i] + _transform.position;
        }
    }

    // Settles doors that lead out of the room
    // Poistions them at the corresponding anchors
    public void SettleDoors() {

        Transform[] anchors = new Transform[_doors.Count];

        //Settles the doors in somewhat pleasing configurations
        switch (_doors.Count) {

            case 0: break;
            case 1:
                anchors[0] = transform.Find("DoorAnchor1");
                break;
            case 2:
                anchors[0] = transform.Find("DoorAnchor2");
                anchors[1] = transform.Find("DoorAnchor3");
                break;
            case 3:
                anchors[0] = transform.Find("DoorAnchor1");
                anchors[1] = transform.Find("DoorAnchor2");
                anchors[2] = transform.Find("DoorAnchor3");
                break;
            default:
                Debug.Log("Room exceeds door quantity");
                return;
        }

        int count = 0;

        foreach (Door d in Doors()) {
            d.transform.position = anchors[count].position;
            ++count;
        }

    }


}
