using System.Collections.Generic;

public class TilePathFinder
{
    public static List<Tile> FindPath(Tile originTile, Tile destinationTile)
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
                if (t.impassable)
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
