using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TilePrefab;
    public GameObject unitPrefab;
    public GameObject aiUnitPrefab;

    public int mapSize = 11;
    Transform mapTransform;

    public List<List<Tile>> map = new List<List<Tile>>();
    public List<Unit> players = new List<Unit>();
    public int currentPlayerIndex = 0;

    public List<Player> _players = new List<Player>();
    private Player userPlayer = new Player { playerType = Player.PlayerType.USER };
    private Player enemyPlayer = new Player { playerType = Player.PlayerType.ENEMY };
    private Player neutralPlayer = new Player { playerType = Player.PlayerType.NEUTRAL };

    private void Awake()
    {
        instance = this;
        mapTransform = transform.Find("Map");
    }

    void Start()
    {
        _players.Add(userPlayer);
        _players.Add(enemyPlayer);
        GenerateMap();
        GeneratePlayerUnits();
    }

    void FixedUpdate()
    {
        if (players[currentPlayerIndex] != null && players[currentPlayerIndex].hp > 0) players[currentPlayerIndex].TurnUpdate();
        else NextTurn();
    }

    private void OnGUI()
    {
        if (players[currentPlayerIndex].hp > 0) players[currentPlayerIndex].TurnOnGUI();
    }

    public void NextTurn()
    {
        if (currentPlayerIndex + 1 < players.Count) currentPlayerIndex++;
        else currentPlayerIndex = 0;
        // Reset greyscale.
        players[currentPlayerIndex].SetToGreyScale(false);
        players[currentPlayerIndex].actionTaken = false;
    }

    public void HighlightTilesAt(Vector2 originLocation, Color highlightColor, int range)
    {
        HighlightTilesAt(originLocation, highlightColor, range, true);
    }

    public void HighlightTilesAt(Vector2 originLocation, Color highlightColor, int range, bool ignorePlayers)
    {
        highlightColor.a = 0.5f;
        List<Tile> highlightedTiles = new List<Tile>();
        if (ignorePlayers) highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], range); // highlightColor == Color.red
        else highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], range, players.Where(x => x.gridPosition != originLocation).Select(x => x.gridPosition).ToArray()); // highlightColor == Color.red
        foreach (Tile t in highlightedTiles)
        {
            t.GetComponent<SpriteRenderer>().material.color = highlightColor;
            t.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }

    public void RemoveTileHighlights()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++) // TODO: What if dimensions are different?
            {
                if (!map[i][j].impassable) {
                    map[i][j].GetComponent<SpriteRenderer>().material.color = Color.white;
                    map[i][j].GetComponent<SpriteRenderer>().sortingOrder = 0;
                }
            }
        }
    }

    public void MoveCurrentPlayer(Tile destTile)
    {
        if (destTile.GetComponent<SpriteRenderer>().material.color != Color.white && !destTile.impassable && players[currentPlayerIndex].positionQueue.Count == 0)
        {
            RemoveTileHighlights();
            players[currentPlayerIndex].moving = false;
            foreach (Tile t in TilePathFinder.FindPath(map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != players[currentPlayerIndex].gridPosition).Select(x => x.gridPosition).ToArray()))
            {
                players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position);
            }
            players[currentPlayerIndex].gridPosition = destTile.gridPosition;
        }
        else Debug.Log("Destination unreachable");
    }

    public void AttackWithCurrentPlayer(Tile targetTile)
    {
            if (targetTile.GetComponent<SpriteRenderer>().material.color != Color.white && !targetTile.impassable)
            {
                Unit target = null;
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
                    players[currentPlayerIndex].actionTaken = true;
                    RemoveTileHighlights();
                    players[currentPlayerIndex].attacking = false; //??? // PART 5: 17:30
                    bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].accuracy;
                    if (hit && target.hp > 0)
                    {
                        float inflictedDmg = players[currentPlayerIndex].damageBase * (1f - target.defenseReduction);
                        target.hp -= (int)inflictedDmg;
                        Debug.Log(players[currentPlayerIndex].unitName + " hit " + target.unitName + " for " + inflictedDmg);
                    }
                    else Debug.Log(players[currentPlayerIndex].unitName + " missed " + target.unitName);
                    players[currentPlayerIndex].actionTaken = true;
                    players[currentPlayerIndex].SetToGreyScale(true);
                }
                else Debug.Log("Invalid target");
            }
    }

    void GenerateMap()
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

    void GeneratePlayerUnits()
    {
        UserUnit unit = ((GameObject)Instantiate(unitPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 0 + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<UserUnit>();
        unit.gridPosition = new Vector2(0, 0);
        unit.unitName = "Unit 1";
        unit.owningPlayer = userPlayer;
        players.Add(unit);

        UserUnit unit2 = ((GameObject)Instantiate(unitPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), -(mapSize - 1) + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<UserUnit>();
        unit2.gridPosition = new Vector2(mapSize - 1, mapSize - 1);
        unit2.unitName = "Unit 2";
        unit.owningPlayer = userPlayer;
        players.Add(unit2);

        UserUnit unit3 = ((GameObject)Instantiate(unitPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), -1 + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<UserUnit>();
        unit3.gridPosition = new Vector2(mapSize - 1, 1);
        unit3.unitName = "Unit 3";
        unit.owningPlayer = userPlayer;
        players.Add(unit3);

        AIUnit aiUnit = ((GameObject)Instantiate(aiUnitPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), -(mapSize - 1) + Mathf.Floor(mapSize / 2), 0), Quaternion.Euler(new Vector3()))).GetComponent<AIUnit>();
        aiUnit.gridPosition = new Vector2(0, mapSize - 1);
        unit.owningPlayer = enemyPlayer;
        aiUnit.unitName = "AI Unit 1";

        players.Add(aiUnit);
    }
}
