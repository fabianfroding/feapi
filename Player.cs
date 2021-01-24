using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Material greyscaleMat;
    private Material defaultMat;

    public Vector2 gridPosition = Vector2.zero;

    public Vector3 moveDestination;
    public float moveSpeed = 10.0f;

    public bool moving = false;
    public bool attacking = false;

    public string name = "Charles";
    public int hp = 20;
    public float accuracy = 0.75f;
    public float defenseReduction = 0.15f;
    public int damageBase = 5;
    public int attackRange = 2;
    public int moveRange = 4;

    public bool actionTaken = false;

    private void Awake()
    {
        moveDestination = transform.position;
    }

    private void Start()
    {
        defaultMat = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetToGreyScale(bool flag)
    {
        if (flag)
        {
            GetComponent<SpriteRenderer>().material = greyscaleMat;
        }
        else
        {
            GetComponent<SpriteRenderer>().material = defaultMat;
        }
    }

    public virtual void TurnUpdate()
    {
        if (GameManager.instance.players[GameManager.instance.currentPlayerIndex] == this)
        {

        }
        
    }

    private void OnMouseDown()
    {
        Debug.Log("I am unit");
    }

    public virtual void TurnOnGUI()
    {

    }
}
