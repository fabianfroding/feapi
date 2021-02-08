using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilePathFinder
{
    public static List<Tile> FindPath(Tile originTile, Tile destinationTile) {
        return FindPath(originTile, destinationTile, new Vector2[0]);
    }

    public static List<Tile> FindPath(Tile originTile, Tile destinationTile, Vector2[] occupied)
    {
        List<Tile> closed = new List<Tile>();
        List<TilePath> open = new List<TilePath>();

        TilePath originPath = new TilePath();
        originPath.AddTile(originTile);
        open.Add(originPath);

        while (open.Count > 0)
        {
            TilePath current = open[0];
            open.Remove(open[0]);

            if (closed.Contains(current.lastTile))
            {
                continue;
            }
            if (current.lastTile == destinationTile)
            {
                current.tiles.Remove(originTile);
                return current.tiles;
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

        return null;
    }
}
