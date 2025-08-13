using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    public static List<TankDataScript> playerDatas = new List<TankDataScript>();
    public static int p1Score = 0, p2Score = 0;
    public static int turn = 1;
}
