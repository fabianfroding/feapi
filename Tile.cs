using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //spriteRenderer.material.color = Color.white;
        Debug.Log("Grid: " + gridPosition.x + ", " + gridPosition.y);
        GameManager.instance.moveCurrentPlayer(this);
    }
}
