using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameController : MonoBehaviour {

    public IMazeFactory factory;

    [SerializeField]
    public Maze maze;

	// Use this for initialization
	void Start () {
        factory = new DummyMazeFactory();
        maze = factory.GenerateMaze();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
