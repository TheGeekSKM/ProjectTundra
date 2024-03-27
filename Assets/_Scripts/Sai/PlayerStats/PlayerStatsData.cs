using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "PlayerStats/PlayerStatsData")]
public class PlayerStatsData : ScriptableObject
{
    public int TotalActionPoints;
    //public int ActionPointCost;
    public int TotalHealth;
    public int Damage;
    public int AOERange;
}
