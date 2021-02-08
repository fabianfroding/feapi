using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : Player
{
	public override void TurnUpdate()
	{
		if (positionQueue.Count > 0)
		{
			transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

			if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
			{
				transform.position = positionQueue[0];
				positionQueue.RemoveAt(0);
				if (positionQueue.Count == 0)
				{
					actionTaken = true; ;
				}
			}

		}
		else
		{
			//priority queue
			List<Tile> attacktilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange, true);
			List<Tile> movementTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], moveRange + 1000);
			//attack if in range and with lowest HP
			if (attacktilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.hp > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
			{
				var opponentsInRange = attacktilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.hp > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.hp : 1000).First();

				GameManager.instance.RemoveTileHighlights();
				moving = false;
				attacking = true;
				GameManager.instance.HighlightTilesAt(gridPosition, Color.red, attackRange);

				GameManager.instance.AttackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
			}
			else if (!moving && movementTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.hp > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
			{
				var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.hp > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.hp : 1000).ThenBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

				GameManager.instance.RemoveTileHighlights();
				moving = true;
				attacking = false;
				GameManager.instance.HighlightTilesAt(gridPosition, Color.blue, moveRange, false);

				List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
				if (path.Count() > 1)
				{
					List<Tile> actualMovement = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], moveRange, GameManager.instance.players.Where(x => x.gridPosition != gridPosition).Select(x => x.gridPosition).ToArray());
					path.Reverse();
					if (path.Where(x => actualMovement.Contains(x)).Count() > 0) GameManager.instance.MoveCurrentPlayer(path.Where(x => actualMovement.Contains(x)).First());
				}
			}
		}

		if (/*actionPoints <= 1*/ actionTaken && (attacking || moving))
		{
			moving = false;
			attacking = false;
		}
		base.TurnUpdate();
	}

	public override void TurnOnGUI()
    {
        base.TurnOnGUI();
    }
}
