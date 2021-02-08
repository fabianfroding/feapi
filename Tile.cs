using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;
    public List<Tile> neighbors = new List<Tile>();
    public int movementCost = 1;
    public bool impassable = false;
    // TODO: Add fields to hold current unit. (If it holds a unit it means it's occupied).

    private void Start()
    {
        GenerateNeighbors();
    }

    public bool isEmpty() // TODO: Impl
    {
        return false;
    }

    private void GenerateNeighbors()
    {
        neighbors = new List<Tile>();

        // Up
        if (gridPosition.y > 0)
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
        // Down
        if (gridPosition.y < GameManager.instance.map.Count - 1)
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }

        // Left
        if (gridPosition.x > 0)
        {
            Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
        // Right
        if (gridPosition.x < GameManager.instance.map.Count - 1)
        {
            Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving)
        {
            GameManager.instance.MoveCurrentPlayer(this);
        }
        else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
        {
            GameManager.instance.AttackWithCurrentPlayer(this);
        }
        else if (Input.GetMouseButton(1))
        {
            // Testing purposes
            if (impassable)
            {
                impassable = false;
                GetComponent<SpriteRenderer>().material.color = Color.white;
            }
            else
            {
                impassable = true;
                GetComponent<SpriteRenderer>().material.color = Color.grey;
            }

        }
        //Debug.Log("Grid: " + gridPosition.x + ", " + gridPosition.y);
    }
}
