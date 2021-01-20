using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 moveDestination;
    public float moveSpeed = 10.0f;

    private void Awake()
    {
        moveDestination = transform.position;
    }

    public virtual void TurnUpdate()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("I am unit");
    }
}
