using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTableId : MonoBehaviour
{
    public void TableId(int id)
    {
        print("Clicked..........................................................take a seat");
        PokerNetworkManager.instance.JoinTable(id);
    }
}
