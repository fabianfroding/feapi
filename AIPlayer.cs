using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : Player
{
    public override void TurnUpdate()
    {
        /*
        if (positionQueue.Count > 0)
        {
            GameManager.instance.RemoveTileHighlights();
            if (Vector3.Distance(positionQueue[0], transform.position) > 0.1f)
            {
                transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
                {
                    transform.position = positionQueue[0];
                    positionQueue.RemoveAt(0);
                    if (positionQueue.Count == 0)
                    {
                        actionTaken = true;
                        SetToGreyScale(true);
                    }
                }
            }
        }
        else
        {
            // Priority queue
            // Attack if in range and with lowest hp
            List<Tile> tilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange);
            if (tilesInRange.Where(x => GameManager.instance.players.Where(y => y.hp > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                List<Player> opponentsInRange = tilesInRange.Select(x => GameManager.instance.players.Where(y => y.hp > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null as Player).ToList();
                var opponent = opponentsInRange.OrderBy(x => x != null ? -x.hp : 1000).First();
                GameManager.instance.HighlightTilesAt(gridPosition, Color.red, attackRange);
                GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
            }
            // Move to nearest attack range of opponent
            // Move towards nearest opponent

            else
            {
                moving = false;
                attacking = false;
                GameManager.instance.nextTurn();
            }
        }*/

        base.TurnUpdate();
    }

    public override void TurnOnGUI()
    {
        base.TurnOnGUI();
    }
}
