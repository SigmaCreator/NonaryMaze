using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {


    IDoorOpeningRule _rule;
    public readonly int doorCode;
    GameController _gc;
    Room _destination;
    Collider2D _collider;
    Vector3 _position;
    Vector3 _transform;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTouch() {


    }

    bool UnlockDoor(List<Player> selection) {


        return _rule.;
    }
    
}
