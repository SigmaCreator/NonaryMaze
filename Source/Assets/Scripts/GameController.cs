using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameController : MonoBehaviour {

    public IMazeFactory factory;

    [SerializeField]
    public Maze maze;

    List<Player> _selectedPlayers = new List<Player>();

	// Use this for initialization
	void Start () {
        factory = new DummyMazeFactory();
        maze = factory.GenerateMaze();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {

            Debug.Log("Click Event");

            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(click);

            Debug.Log("world point: " + click);
            Debug.Log("screen point: " + Input.mousePosition);

            if (hit != null) {
                Debug.Log(hit.name);
            }
                
            if (hit!= null && hit.tag == "Player")
            {

                Debug.Log("Player Touched Event");

                //TODO Abstract player selection

                // Player selected
                Player player = hit.GetComponent<Player>();
                player.OnTouch();

                if (_selectedPlayers.Contains(player))
                {
                    _selectedPlayers.Remove(player);
                }
                else
                {
                    _selectedPlayers.Add(player);
                }


            }
            else if (hit != null && hit.tag == "Door") {

                // Door activated
                Debug.Log("Door Touched Event");
                Door door = hit.GetComponent<Door>();
                door.OnTouch();

                bool doorUnlock = door.UnlockDoor(_selectedPlayers);
                if (doorUnlock) {

                    door.Origin.RemovePlayers(_selectedPlayers);
                    door.Destination.AddPlayers(_selectedPlayers);

                    foreach (Player p in _selectedPlayers)
                    {
                        p.OnTouch();
                    }
                    _selectedPlayers.Clear();
                }

                //Unselects players

                

            }
        }
		
	}


}
