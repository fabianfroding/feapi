using System.Collections.Generic;

public class TilePath
{
    public List<Tile> tiles = new List<Tile>();
    public Tile lastTile;
    public int costOfPath = 0;

    public TilePath() { }

    public TilePath(TilePath tp)
    {
        tiles = tp.tiles;
        costOfPath = tp.costOfPath;
        lastTile = tp.lastTile;
    }

    public void AddTile(Tile t)
    {
        costOfPath += t.movementCost;
        tiles.Add(t);
        lastTile = t;
    }
}