using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePath
{
    public List<Tile> tiles = new List<Tile>();
    Tile lastTile;
    int costOfPath = 0;

    public TilePath() {}

    public TilePath(List<Tile> startingPath)
    {
        tiles = startingPath;
    }

    public Tile GetLastTile()
    {
        if (tiles.Count > 0)
        {
            return tiles[tiles.Count - 1];
        }
        return null;
    }

    public void AddTile(Tile t)
    {
        costOfPath += t.movementCost;
        tiles.Add(t);
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
        originPath.addTile(originTile);
        open.Add(originPath);

        while(open.Count > 0)
        {
            TilePath current = new TilePath(open[0]);
            open.Remove(open[0]);

            if (closed.Contains(current.GetLastTile()))
            {
                continue;
            }

            closed.AddRange(current.tiles);

            for (int i = 0; i < current.GetLastTile().neighbors.Count; i++)
            {

            }
        }

        return closed;
    }

}
