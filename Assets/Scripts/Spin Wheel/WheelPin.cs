using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPin : MonoBehaviour
{
    public static WheelPin _instanceSpinWheel;

    public int spinvalue;


    void Awake()
    {
        _instanceSpinWheel = this;
    }

    public void OnTriggerEnter(Collider Col)
    {
        print("Name-- " + Col.gameObject.name);

        spinvalue = int.Parse(Col.gameObject.name);

        print("spinvalue-- " + spinvalue);

    }
}