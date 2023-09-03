using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Lerp Script
public class MoveObject : MonoBehaviour
{
    //Vector3 variables to store Start & End values for each axis and lerpFloats for each axis
    public Vector3 start, end, lerpFloat;

    //Lerp boolean for preventing user from starting lerp coroutine while a lerp is in progress 
    public bool lerping = false;


    private void Awake()
    {
        //Set lerpFloat to current object position so that object doesn't get moved during update frames
        lerpFloat = transform.position;
    }

    //Public function to call the lerp from a button
    public void StartLerp() {

        //Only starts coroutine if there isn't a lerp in progress
        if (!lerping)
        {
            //Coroutine is fed ease and pingpong choices for each axis from the dropdowns in UIManager script
            StartCoroutine(Lerp(UIManager.objXEaseDropPick, UIManager.objYEaseDropPick, UIManager.objZEaseDropPick, UIManager.pingPongX, UIManager.pingPongY, UIManager.pingPongZ));
        }
    }

    //Function to engage lerping
    private IEnumerator Lerp(int x_ease, int y_ease, int z_ease, bool pingPongX, bool pingPongY, bool pingPongZ) {
        //Set lerping to true to prevent user from starting another lerp while this on is in progress
        lerping = true;

        //Set time to zero
        float time = 0f;

        //Set start position to current object position
        start = transform.position;

        //Set end position of each axis individually according to what the sliders in UIManager script are set
        end.x = start.x + UIManager.xDistAmount;
        end.y = start.y + UIManager.yDistAmount;
        end.z = start.z + UIManager.zDistAmount;

        //Start creating lerpFloat values for one sec or continuously if Lerp contiuous toggle is set in UIManager
        while (time < 1f || UIManager.lerpContinuous) {
            //If continuous is on then action speeds up unless reset back to zero after every sec
            if (time > 1f) time = 0f;

            //Generate lerpFloat values by access Lerp function library and feeding in various options chosen
            lerpFloat.x = LerpFunctions.Lerping(start.x, end.x, LerpFunctions.LerpPerc((int) x_ease, time, pingPongX));
            lerpFloat.y = LerpFunctions.Lerping(start.y, end.y, LerpFunctions.LerpPerc((int) y_ease, time, pingPongY));
            lerpFloat.z = LerpFunctions.Lerping(start.z, end.z, LerpFunctions.LerpPerc((int) z_ease, time, pingPongZ));

            //Increment Time
            time += Time.deltaTime;

            //IEnumerator must return but we don't want to return anything so we return null
            yield return null;
        }

        //Set lerping to false to allow user to start another lerp since this one has ended
        lerping = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Translate lerpFloat values into object movement
        transform.position = new Vector3(lerpFloat.x, lerpFloat.y, lerpFloat.z);
    }
}
