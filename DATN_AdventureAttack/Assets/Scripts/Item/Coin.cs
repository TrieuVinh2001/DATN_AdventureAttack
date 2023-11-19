using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int minCoin;
    public int maxCoin;
    public int coin;

    // Start is called before the first frame update
    void Start()
    {
        coin = Random.Range(minCoin,maxCoin);
    }
}
