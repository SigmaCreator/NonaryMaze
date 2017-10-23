using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {

    [SerializeField]
    int playerCode = 0;

 
    public int Code
    {
        get { return playerCode; }
        set { playerCode = playerCode == 0 ? value : playerCode; }
    } 

    GameController gc;
    SpriteRenderer _sprite;
    Collider2D _collider;

    Color[] colors;
    Vector3[] scales;
    bool _selected = false;



    void Start()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<Collider2D>();
        colors = new Color[2];
        colors[0] = _sprite.color;
        colors[1] = Color.yellow;
        scales = new Vector3[2];
        scales[0] = transform.localScale;
        scales[1] = scales[0] * 1.2f;

    }

    public void OnTouch() {

        Debug.Log(name + " Touched");
        if (_selected)
        {
            _selected = !_selected;
            _sprite.color = colors[0];
            transform.localScale = scales[0];
        }
        else
        {
            _selected = !_selected;
            _sprite.color = colors[1];
            transform.localScale = scales[1];
        }

    }

}
