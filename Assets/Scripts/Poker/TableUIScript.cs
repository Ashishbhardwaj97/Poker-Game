using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableUIScript : MonoBehaviour
{
    public Text tableNumber;
    public Text tableType;
    public Text blind;
    public Text TotalPot;
    public Text RoundPot;

    public Transform localPlayerMenu;
    public Transform tableParent;
    public Transform playerParent;
    public Transform footer;
    public Transform cards;
    public Transform observingPanel;
    public Transform tableStartButton;

    [Space]
    public UIEvents functionsToRun;
}
