using System.Collections.Generic;
using System.Linq;

public class TilePath
{
	public List<Tile> tiles = new List<Tile>();
	public int costOfPath = 0;
	public Tile lastTile;

	public TilePath() { }

	public TilePath(TilePath tp)
	{
		tiles = tp.tiles.ToList();
		costOfPath = tp.costOfPath;
		lastTile = tp.lastTile;
	}

	public void AddTile(Tile t)
	{
		costOfPath += t.movementCost;
		tiles.Add(t);
		lastTile = t;
	}

	public void AddStaticTile(Tile t)
	{
		costOfPath += 1;
		tiles.Add(t);
		lastTile = t;
	}
}