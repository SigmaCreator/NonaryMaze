using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door : MonoBehaviour {


    private enum DoorState {
        LOCKED,
        UNLOCKED
    }

    DoorState _state = DoorState.LOCKED;

    GameController _gc;

    [SerializeField]
    IDoorOpeningRule _rule;
    public IDoorOpeningRule Rule
    {
        get { return _rule; }
        set { _rule = _rule ?? value; }
    }

    [SerializeField]
    int _doorCode;
    public int Code
    {
        get { return _doorCode; }
        set { _doorCode = _doorCode == 0 ? value : _doorCode; }
    }
    
    [SerializeField]
    Room _destination;
    public Room Destination
    {
        get { return _destination; }
        set { _destination = _destination ?? value; }
    }

    [SerializeField]
    Room _origin;
    public Room Origin
    {
        get { return _origin; }
        set { _origin = _origin ?? value; }
    }

    Collider2D _collider;
    Vector3 _position;

    // Use this for initialization
    public void Start () {
        _position = gameObject.transform.position;
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

    private void DoorAnimation(DoorState animation_state) {

        //Starts the appropriate door animation coroutine
        switch (animation_state)
        {
            case DoorState.LOCKED:
                break;
            case DoorState.UNLOCKED:
                break;
        }

    }

    public void OnTouch() {
        // Does some animation
        DoorAnimation(_state);
        LockDoor();

    }

    private void LockDoor() {
        _state = DoorState.LOCKED;
    }

    public bool UnlockDoor(List<Player> selection) {
        bool unlock = _rule.VerifyCode(selection, _doorCode);
        if (unlock) { _state = DoorState.UNLOCKED; }
        return unlock;
    }

}
