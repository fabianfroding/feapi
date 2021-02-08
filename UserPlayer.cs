using UnityEngine;

public class UserPlayer : Player
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
                    actionTaken = true;
                    SetToGreyScale(true);
                }
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
                GameManager.instance.RemoveTileHighlights();
                moving = true;
                attacking = false;
                Debug.Log("MRange: " + moveRange);
                GameManager.instance.HighlightTilesAt(gridPosition, Color.blue, moveRange, false);
            }
            else
            {
                moving = false;
                attacking = false;
                Debug.Log("ARange: " + attackRange);
                GameManager.instance.RemoveTileHighlights();
            }
        }

        // Attack BTN
        buttonRect = new Rect(0, Screen.height - btnHeight * 2, btnWidth, btnHeight);
        if (GUI.Button(buttonRect, "Attack"))
        {
            if (!attacking)
            {
                GameManager.instance.RemoveTileHighlights();
                moving = false;
                attacking = true;
                GameManager.instance.HighlightTilesAt(gridPosition, Color.red, attackRange);
            }
            else
            {
                moving = false;
                attacking = false;
                GameManager.instance.RemoveTileHighlights();
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
