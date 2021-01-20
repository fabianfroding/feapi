using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TilePrefab;
    public GameObject unitPrefab;
    public GameObject aiUnitPrefab;

    public int mapSize = 11;

    List<List<Tile>> map = new List<List<Tile>>();
    List<Player> players = new List<Player>();
    int currentPlayerIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        generateMap();
        generatePlayerUnits();
    }

    void FixedUpdate()
    {
        players[currentPlayerIndex].TurnUpdate();
    }

    public void nextTurn()
    {
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }
    }

    public void moveCurrentPlayer(Tile destTile)
    {
        players[currentPlayerIndex].moveDestination = destTile.transform.position;
    } 

    void generateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void generatePlayerUnits()
    {
        UserPlayer player = ((GameObject)Instantiate(unitPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 0 + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        players.Add(player);

        AIPlayer aiPlayer = ((GameObject)Instantiate(aiUnitPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), -(mapSize - 1) + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
        players.Add(aiPlayer);
    }
}
