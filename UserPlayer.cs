using UnityEngine;

public class UserPlayer : Player
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
        }

        base.TurnUpdate();
    }
}
