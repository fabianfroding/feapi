using UnityEngine;

public class UserPlayer : Player
{

    public override void TurnUpdate()
    {
        //highlight





        if (Vector3.Distance(moveDestination, transform.position) > 0.1f)
        {
            transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(moveDestination, transform.position) <= 0.1f)
            {
                transform.position = moveDestination;
                // TODO: Prompt user to action (Attack, Item, Wait etc)
                actionTaken = true;
                SetToGreyScale(true);
            }
        }

        base.TurnUpdate();
    }

    public override void TurnOnGUI()
    {
        float btnHeight = 50f;
        float btnWidth = 100f;

        // Move BTN
        Rect buttonRect = new Rect(0, Screen.height - btnHeight * 3, btnWidth, btnHeight);
        if (GUI.Button(buttonRect, "Move"))
        {
            if (!moving)
            {
                moving = true;
                attacking = false;
            }
            else
            {
                moving = false;
                attacking = false;
            }
        }

        // Attack BTN
        buttonRect = new Rect(0, Screen.height - btnHeight * 2, btnWidth, btnHeight);
        if (GUI.Button(buttonRect, "Attack"))
        {
            if (!attacking)
            {
                moving = false;
                attacking = true;
            }
            else
            {
                moving = false;
                attacking = false;
            }
        }

        // End BTN
        buttonRect = new Rect(0, Screen.height - btnHeight * 1, btnWidth, btnHeight);
        if (GUI.Button(buttonRect, "End"))
        {
            actionTaken = false;
            moving = false;
            attacking = false;
            GameManager.instance.nextTurn();
        }


        base.TurnOnGUI();
    }
}
