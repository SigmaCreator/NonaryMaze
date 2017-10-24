using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameController : MonoBehaviour {

    public enum GameState {
        MENU,
        IN_GAME,
        GOAL
    };

    GameState _state = GameState.IN_GAME;
    public GameState State
    {
        get
        { return _state; }

        set
        { ContextSwitch(value); }
    }

    public IMazeFactory factory;

    [SerializeField]
    public Maze maze;

    List<Player> _selectedPlayers = new List<Player>();



    // Use this for initialization
    void Start () {
        factory = new DummyMazeFactory();
        maze = factory.GenerateMaze();
	}

    void TogglePlayerSelection(Player player) {

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

    void ClearPlayerSelection()
    {
        foreach (Player p in _selectedPlayers)
        {
            p.OnTouch();
        }
        _selectedPlayers.Clear();
    }


    void InGameUpdate()
    {
        if (Input.GetMouseButtonDown(0)) {
            
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(click);

            if (hit != null) {
                Debug.Log(hit.name);
            }
                
            if (hit!= null && hit.tag == "Player")
            {
               
                // Player selected
                Player player = hit.GetComponent<Player>();

                TogglePlayerSelection(player);

            }
            else if (hit != null && hit.tag == "Door") {

                // Door activated
                Door door = hit.GetComponent<Door>();
                door.OnTouch();

                bool doorUnlock = door.UnlockDoor(_selectedPlayers);
                if (doorUnlock)
                {

                    door.Origin.RemovePlayers(_selectedPlayers);
                    door.Destination.AddPlayers(_selectedPlayers);

                    ClearPlayerSelection();
                }
            }
        }
    }

    void ContextSwitch(GameState newState) {

        //Switches context
        _state = newState;
        //TODO STATE SWITCH CLEAN UP

    }

	// Update is called once per frame
	void Update () {

        switch (State)
        {
            case GameState.IN_GAME:
                InGameUpdate();
                break;
            case GameState.MENU:
                break;
            case GameState.GOAL:
                break;
            default:
                return;


        }
        
		
	}


}
