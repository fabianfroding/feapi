using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileHighlight
{
    public static List<Tile> FindHighlight(Tile originTile, int movementPoints)
    {
        return FindHighlight(originTile, movementPoints, new Vector2[0]);
    }

    public static List<Tile> FindHighlight(Tile originTile, int movementPoints, Vector2[] occupied)
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
                if (t.impassable || occupied.Contains(t.gridPosition))
                {
                    continue;
                }
                TilePath newTilePath = new TilePath(current);
                newTilePath.AddTile(t);
                open.Add(newTilePath);
            }
        }

        closed.Remove(originTile);
        return closed;
    }
}
