using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Unit> units;
    public PlayerType playerType;
    
    public enum PlayerType
    {
        USER,
        ENEMY,
        NEUTRAL
    }
}
