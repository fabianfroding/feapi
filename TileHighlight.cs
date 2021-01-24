using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePath
{
    public List<Tile> tiles = new List<Tile>();
    public Tile lastTile;
    public int costOfPath = 0;

    public TilePath() {}

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

public class TileHighlight
{
    public TileHighlight() {}

    public static List<Tile> FindHighlight(Tile originTile, int movementPoints)
    {
        List<Tile> closed = new List<Tile>();
        List<TilePath> open = new List<TilePath>();

        TilePath originPath = new TilePath();
        originPath.AddTile(originTile);
        open.Add(originPath);

        while(open.Count > 0)
        {
            TilePath current = open[0];
            open.Remove(open[0]);

            if (closed.Contains(current.lastTile))
            {
                continue;
            }
            if (current.costOfPath > movementPoints + 1)
            {
                continue;
            }

            closed.Add(current.lastTile);

            foreach (Tile t in current.lastTile.neighbors)
            {
                TilePath newTilePath = new TilePath(current);
                newTilePath.AddTile(t);
                open.Add(newTilePath);
            }
        }

        closed.Remove(originTile);
        return closed;
    }

}
