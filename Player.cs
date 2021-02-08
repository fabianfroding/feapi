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

    public List<Vector3> positionQueue = new List<Vector3>();

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
        if (hp <= 0) Destroy(gameObject);
    }

    public void SetToGreyScale(bool flag)
    {
        GetComponent<SpriteRenderer>().material = flag ? greyscaleMat : defaultMat;
    }

    public virtual void TurnUpdate()
    {
        if (actionTaken)
        {
            moving = false;
            attacking = false;
            GameManager.instance.nextTurn();
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
