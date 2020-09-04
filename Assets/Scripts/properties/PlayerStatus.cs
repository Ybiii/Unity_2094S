using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/status")]
public class PlayerStatus : ScriptableObject
{
    public bool isAiming;
    public bool isSprint;
    public bool isGround;
}
