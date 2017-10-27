using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {

    // Set up a state machine for character states.
    // Selected/unselected

    public enum PlayerState{
    UNSELECTED,
    SELECTED

    };

    [SerializeField]
    int playerCode = 0;
    public int Code
    {
        get { return playerCode; }
        set { playerCode = playerCode == 0 ? value : playerCode; }
    }

    public Sprite[] Sprites
    {
        get { return _sprites; }
        set { _sprites = value; }
    }

    [SerializeField]
    PlayerState _state = PlayerState.UNSELECTED;
    public PlayerState State {
        get { return _state; }
    }

    // cached reference to the gamecontroller. useful?
    GameController gc;

    [SerializeField]
    Sprite[] _sprites;

    // cached references to the sprite components. Controls the sprite character and the numbered badge.
    SpriteRenderer _chracterSprite;
    SpriteRenderer _codeSprite;

    Collider2D _collider;

    Color[] colors;
    Vector3[] scales;
    
    void Start()
    {
        // Sets up all sprites and states.
        // Caches necessary component references.
        // Initializes the procedural components.

        _chracterSprite = transform.Find("CharacterSprite").gameObject.AddComponent<SpriteRenderer>();
        _codeSprite = transform.Find("CodeSprite").gameObject.AddComponent<SpriteRenderer>();

        //Asset Recycling! Saving the World!
        _codeSprite.sprite = Resources.Load<Sprite>("Sprites/ph_Player" + Code);
        _codeSprite.sortingOrder = 1;

        _collider = gameObject.GetComponent<Collider2D>();
        colors = new Color[2];
        colors[0] = _chracterSprite.color;
        colors[1] = Color.yellow;
        scales = new Vector3[2];
        scales[0] = transform.localScale;
        scales[1] = scales[0] * 1.2f;

        _chracterSprite.sprite = Sprites[0];

    }

    public void Update()
    {
        switch (_state)
        {
            case (PlayerState.SELECTED):
                //TODO ineffective, cache this cleanly
                _chracterSprite.gameObject.transform.Rotate(new Vector3(0f, 0f, (Mathf.Sin(Time.time * 4) / 2)));
                //_chracterSprite.gameObject.transform.Translate(new Vector3(Mathf.Cos(Time.time * 4) / 100, Mathf.Sin(Time.time * 4) / 100, 0f));
                break;
            default:
                break;
                
        }

    }

    public void ChangeState(Player.PlayerState state) {
        //Switches into new PlayerState state
    
        _state = state;
        switch (_state)
        {
            case (PlayerState.SELECTED):
                _chracterSprite.sprite = Sprites[1];
                transform.localScale = scales[1];
                break;

            case (PlayerState.UNSELECTED):
                _chracterSprite.transform.rotation = Quaternion.identity;
                //_chracterSprite.color = colors[0];
                _chracterSprite.sprite = Sprites[0];
                    transform.localScale = scales[0];
                break;
            default:
                break;
                
        }
        
    }

    public void OnTouch() {

        switch (_state)
        {
            case (PlayerState.SELECTED):
                ChangeState(PlayerState.UNSELECTED);
                break;
            case (PlayerState.UNSELECTED):
                ChangeState(PlayerState.SELECTED);
                break;
            default:
                break;
        }

    }

}
