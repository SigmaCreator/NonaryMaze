using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {

    // Set up a state machine for character states.
    // Selected/unselected

    [SerializeField]
    int playerCode = 0;

 
    public int Code
    {
        get { return playerCode; }
        set { playerCode = playerCode == 0 ? value : playerCode; }
    }

    public Sprite[] Sprites
    {
        get
        {
            return _sprites;
        }

        set
        {
            _sprites = value;
        }
    }

    GameController gc;

    [SerializeField]
    Sprite[] _sprites;

    SpriteRenderer _chracterSprite;

    SpriteRenderer _codeSprite;

    Collider2D _collider;

    Color[] colors;
    Vector3[] scales;
    bool _selected = false;

    float _selectionAnimator = Mathf.PI/2f;

    void Start()
    {
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
        if (_selected) {

            _selectionAnimator += Time.deltaTime;

            //TODO ineffective, cache this cleanly
            _chracterSprite.gameObject.transform.Rotate(new Vector3(0f, 0f, Mathf.Sin(_selectionAnimator)));
            
        }
    }

    public void OnTouch() {
        
        if (_selected)
        {
            _selected = !_selected;
            //_chracterSprite.color = colors[0];
            _chracterSprite.sprite = Sprites[0];
            transform.localScale = scales[0];

            _selectionAnimator = Mathf.PI / 2f;
            _chracterSprite.transform.rotation = Quaternion.identity;
        }
        else
        {
            _selected = !_selected;
            //_chracterSprite.color = colors[1];
            _chracterSprite.sprite = Sprites[1];
            transform.localScale = scales[1];
        }

    }

}
