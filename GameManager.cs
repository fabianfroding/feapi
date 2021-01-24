using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TilePrefab;
    public GameObject unitPrefab;
    public GameObject aiUnitPrefab;

    public int mapSize = 11;

    public List<List<Tile>> map = new List<List<Tile>>();
    public List<Player> players = new List<Player>();
    public int currentPlayerIndex = 0;

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
        if (players[currentPlayerIndex].hp > 0)
        {
            players[currentPlayerIndex].TurnUpdate();
        }
        else
        {
            nextTurn();
        }
    }

    public void HighlightTilesAt(Vector2 originLocation, Color highlightColor, int range)
    {
        Debug.Log("BRange: " + range);
        List<Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], range); // TODO: Put Unit movespeed instead of 4
        foreach (Tile t in highlightedTiles)
        {
            t.GetComponent<SpriteRenderer>().material.color = highlightColor;
        }
    }

    public void RemoveTileHighlights()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++) // TODO: What if dimensions are different?
            {
                if (!map[i][j].impassable)
                {
                    map[i][j].GetComponent<SpriteRenderer>().material.color = Color.white;
                }
            }
        }
    }

    private void OnGUI()
    {
        players[currentPlayerIndex].TurnOnGUI();
    }

    public void nextTurn()
    {
        // Reset greyscale.
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetToGreyScale(false);
        }
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
        if (!players[currentPlayerIndex].actionTaken)
        {
            if (destTile.GetComponent<SpriteRenderer>().material.color != Color.white && !destTile.impassable)
            {
                foreach (Tile t in TilePathFinder.FindPath(map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y], destTile))
                {
                    players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position);
                }
                players[currentPlayerIndex].gridPosition = destTile.gridPosition;
            }
            else
            {
                Debug.Log("Destination unreachable");
            }
        }
    }

    public void attackWithCurrentPlayer(Tile targetTile)
    {
        if (!players[currentPlayerIndex].actionTaken)
        {
            if (targetTile.GetComponent<SpriteRenderer>().material.color != Color.white)
            {
                Player target = null;
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].gridPosition == targetTile.gridPosition)
                    {
                        target = players[i];
                        break;
                    }
                }

                if (target != null)
                {
                    if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - 1 &&
                        players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + 1 &&
                        players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - 1 &&
                        players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + 1)
                    {
                        RemoveTileHighlights();
                        bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].accuracy;
                        if (hit)
                        {
                            if (target.hp > 0)
                            {
                                float inflictedDmg = players[currentPlayerIndex].damageBase * (1f - target.defenseReduction);
                                target.hp -= (int)inflictedDmg;
                                Debug.Log(players[currentPlayerIndex].name + " hit " + target.name + " for " + inflictedDmg);
                            }
                        }
                        else
                        {
                            Debug.Log(players[currentPlayerIndex].name + " missed " + target.name);
                        }
                        players[currentPlayerIndex].actionTaken = true;
                        players[currentPlayerIndex].SetToGreyScale(true);
                    }
                    else
                    {
                        Debug.Log("Target out of range");
                    }
                }
            }
        }
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
        player.gridPosition = new Vector2(0, 0);
        player.name = "Player 1";
        players.Add(player);

        UserPlayer player2 = ((GameObject)Instantiate(unitPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), -(mapSize - 1) + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player2.gridPosition = new Vector2(mapSize - 1, mapSize - 1);
        player2.name = "Player 2";
        players.Add(player2);

        //AIPlayer aiPlayer = ((GameObject)Instantiate(aiUnitPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), -(mapSize - 1) + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
        //players.Add(aiPlayer);
    }
}
