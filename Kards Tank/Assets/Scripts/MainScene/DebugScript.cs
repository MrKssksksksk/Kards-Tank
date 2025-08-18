using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public bool debug;
    public GameObject p1, p2;
    public int debugPlayer; // 0 or 1
    TankLogicScript tankLogicScript;
    TankDataScript tankDataScript;

    private void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player1");
        p2 = GameObject.FindGameObjectWithTag("Player2");
    }

    void Update() // scripts only for debug
    {
        if (debug) 
        {
            tankLogicScript = (debugPlayer == 0 ? p1 : p2).GetComponent<TankLogicScript>();
            tankDataScript = (debugPlayer == 0 ? p1 : p2).GetComponent<TankDataScript>();


            if (Input.GetKeyUp(KeyCode.LeftBracket)) debugPlayer = 0;
            if (Input.GetKeyUp(KeyCode.RightBracket)) debugPlayer = 1;

            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad1)) tankLogicScript.pushBullet(1); // snow
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad2)) tankLogicScript.pushBullet(2); // 85mm
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad3)) tankLogicScript.pushBullet(3); // ib
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad4)) tankLogicScript.pushBullet(4); // li

            if (Input.GetKeyUp(KeyCode.P) && Input.GetKey(KeyCode.Keypad0)) tankLogicScript.giveItem(0);
            if (Input.GetKeyUp(KeyCode.P) && Input.GetKey(KeyCode.Keypad1)) tankLogicScript.giveItem(1);
            if (Input.GetKeyUp(KeyCode.P) && Input.GetKey(KeyCode.Keypad2)) tankLogicScript.giveItem(2);
            if (Input.GetKeyUp(KeyCode.P) && Input.GetKey(KeyCode.Keypad3)) tankLogicScript.giveItem(3);
            if (Input.GetKeyUp(KeyCode.P) && Input.GetKey(KeyCode.Keypad4)) tankLogicScript.giveItem(4);

            if ((Input.GetKeyUp(KeyCode.L))) tankDataScript.cSupply = 500;
        }
    }
}
