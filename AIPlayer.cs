using UnityEngine;

public class AIPlayer : Player
{
    public override void TurnUpdate()
    {
        if (Vector3.Distance(moveDestination, transform.position) > 0.1f)
        {
            transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(moveDestination, transform.position) <= 0.1f)
            {
                transform.position = moveDestination;
                GameManager.instance.nextTurn();
            }
        } else
        {
            moveDestination = new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize/2), -0 + Mathf.Floor(GameManager.instance.mapSize / 2), 0);
        }

        base.TurnUpdate();
    }

    public override void TurnOnGUI()
    {
        base.TurnOnGUI();
    }
}
