using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door : MonoBehaviour {

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
    Vector3 _transform;

    // Use this for initialization
    public void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

    public void OnTouch() {


    }

    public bool UnlockDoor(List<Player> selection) {

        return _rule.VerifyCode(selection, _doorCode);

    }
    
}
