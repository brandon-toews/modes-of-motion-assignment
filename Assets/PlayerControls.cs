using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using static UnityEngine.UIElements.UxmlAttributeDescription;

//Camera movement script
public class PlayerControls : MonoBehaviour
{
    //Speed of camera controls
    private const float speed = 8f;

    //Create var to store Raycast hit objects
    public static GameObject CurrentSelectedObject;

    //Lerp float variable for storing cam lerp values
    public float camLerpFloat;

    //Lerp boolean for preventing user from starting Cam lerp coroutine while a lerp is in progress
    public bool lerping = false;

    //Public function to call the Cam lerp from a button
    public void StartCamLerp()
    {
        //Only starts coroutine if there isn't a lerp in progress
        if (!lerping)
        {
            //Coroutine is fed Cam ease choices the Cam dropdowns in UIManager script
            StartCoroutine(CamLerp(UIManager.camEaseDropPick));
        }
    }

    //Function to engage Cam lerping
    private IEnumerator CamLerp(int ease)
    {
        //Set lerping to true to prevent user from starting another lerp while this on is in progress
        lerping = true;

        //Set time to zero
        float time = 0f;

        //Start creating Cam lerpFloat values for one sec duration
        while (time < 1f)
        {
            //If Cam shake option is picked in dropdown then first part of statement executes sine wave on x Axis
            if (UIManager.camEffectDropPick == 0) {

                //Generates sine wave values by taking amount set by slider & current time,
                //the slider controls the size of the sine wave and therefore the amount of shake produced
                camLerpFloat = UIManager.camEffectAmount * Mathf.Sin(time * 30f);

                //Increment time
                time += Time.deltaTime;

                //IEnumerator must return but we don't want to return anything so we return null
                yield return null;
            }
            //If Cam rotate or zoom option is picked in dropdown then else if executes to
            else if (UIManager.camEffectDropPick == 1 || UIManager.camEffectDropPick == 2) {

                //Generates Cam lerpFloat values by taking amount set by slider, ease chosen, & current time
                camLerpFloat = LerpFunctions.Lerping(0, UIManager.camEffectAmount, LerpFunctions.LerpPerc(ease, time, false));

                //Increment time
                time += Time.deltaTime;

                //IEnumerator must return but we don't want to return anything so we return null
                yield return null;
            }
        }
        //Set lerping to false to allow user to start another cam lerp since this one has ended
        lerping = false;
    }

    //Camera movement function
    void MovePlayer()
    {
        //Get horizontal keyboard input
        float LeftRight = Input.GetAxis("Horizontal");
        //Create and store movement in Vector3 & multiply by speed and time
        Vector3 moveX = new Vector3(LeftRight, 0, 0) * Time.deltaTime * speed;
        //Translates horizontal keyboard input into sidetoside cam movement
        transform.Translate(moveX, Space.Self);


        //Get vertical keyboard input
        float ForwardBack = Input.GetAxis("Vertical");
        //Create and store movement in Vector3 & multiply by speed and time
        Vector3 moveZ = new Vector3(0, 0, ForwardBack) * Time.deltaTime * speed;
        //Translates vertical keyboard input into forward cam movement
        transform.Translate(moveZ, Space.Self);


        //Create variable to store Up & Down keyboard input
        float UpDown = 0;
        //If Q key pressed then camera moves down
        if (Input.GetKey(KeyCode.Q)) UpDown = -1;
        //If E key pressed then camera moves up
        if (Input.GetKey(KeyCode.E)) UpDown = 1;
        //Create and store movement in Vector3 & multiply by speed and time
        Vector3 moveY = new Vector3(0, UpDown, 0) * Time.deltaTime * speed;
        //Translates Q & E keyboard input into UP/DOWN cam movement
        transform.Translate(moveY, Space.Self);

        //Create variable to store Rotate left/right keyboard input
        float rotateLeftRight = 0;
        //If Z key pressed then camera looks left
        if (Input.GetKey(KeyCode.Z)) rotateLeftRight = -3;
        //If X key pressed then camera looks right
        if (Input.GetKey(KeyCode.X)) rotateLeftRight = 3;
        //Create and store movement in Vector3 & multiply by speed and time
        Vector3 moveRotY = new Vector3(0, rotateLeftRight, 0) * Time.deltaTime * speed;
        //Translates Z & X keyboard input into looking left/right cam movement
        transform.Rotate(moveRotY, Space.Self);

        //Create variable to store Rotate up/down keyboard input
        float rotateUpDown = 0;
        //If R key pressed then camera looks up
        if (Input.GetKey(KeyCode.R)) rotateUpDown = -3;
        //If F key pressed then camera looks down
        if (Input.GetKey(KeyCode.F)) rotateUpDown = 3;
        //Create and store movement in Vector3 & multiply by speed and time
        Vector3 moveRotX = new Vector3(rotateUpDown, 0, 0) * Time.deltaTime * speed;
        //Translates Z & X keyboard input into looking up/down cam movement
        transform.Rotate(moveRotX, Space.Self);

        //When cam lerp is engaged this if statement executes to move the cam according to generated lerp
        if (lerping) {

            //Switch to translate cam lerp float into movement according to dropdown choice
            switch (UIManager.camEffectDropPick)
            {
                //If cam shake is chosen do this
                case 0:
                    //Create variable to store cam shake values
                    float shake;
                    //store movement & multiply by speed and time
                    shake = camLerpFloat * Time.deltaTime * speed;
                    //Translates lerp float values into movement
                    transform.Rotate(new Vector3(0, shake, 0), Space.Self);
                    break;

                //If cam rotate is chosen do this
                case 1:
                    //Create variable to store cam rotate values
                    float rotate;
                    //store movement & multiply by speed and time
                    rotate = camLerpFloat * Time.deltaTime * speed;
                    //If left option is chosen then make values negative to rotate left instead of right
                    if (UIManager.camRotDirDropPick == 0) rotate = -rotate;
                    //Translates lerp float values into movement
                    transform.Rotate(new Vector3(0, 0, rotate), Space.Self);
                    break;

                //If cam zoom is chosen do this
                case 2:
                    //Create variable to store cam zoom values
                    float zoom;
                    //store movement & multiply by speed and time
                    zoom = camLerpFloat * Time.deltaTime * speed;
                    //If zoom out option is chosen then make values negative to zoom out instead of in
                    if (UIManager.camZoomDropPick == 1) zoom = -zoom;
                    //Translates lerp float values into movement
                    transform.Translate(new Vector3(0, 0, zoom), Space.Self);
                    break;

                default:
                    break;
            }
        }
    }

    //Raycasting Function
    void Raycasting()
    {
        //Don't do raycast if mouse is overtop of UI element
        if (!EventSystem.current.IsPointerOverGameObject()) {

            //When mouse left click do raycast
            if (Input.GetMouseButtonDown(0)) {
                //Create ray from main cam to mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Whatever is hit will be stores in "hit' var
                RaycastHit hit;

                //If there something was hit then execute
                if (Physics.Raycast(ray, out hit)) {

                    //If raycast hits plane or ceiling enable cam canvas and disable object lerp canvas
                    //and refresh canvas'
                    if (hit.collider.tag == "Terrain") {
                        UIManager.camCanvas = true;
                        UIManager.objectCanvas = false;
                        UIManager.UpdateCanvas();
                    }

                    //If raycast hits object disable cam canvas and enable object lerp canvas
                    //and refresh canvas'
                    else {
                        UIManager.camCanvas = false;
                        UIManager.objectCanvas = true;

                        //Show object info in object lerp canvas UI
                        UIManager.objectNameText = hit.transform.name;
                        UIManager.objectShapeText = hit.collider.tag;
                        UIManager.objLoc = hit.transform.position;

                        //Refresh canvas'
                        UIManager.UpdateCanvas();

                        //if previous object hit was playing launch script then the launch script must
                        //be disabled or it will affect the new object selected
                        if (CurrentSelectedObject && hit.collider.gameObject != CurrentSelectedObject) CurrentSelectedObject.GetComponent<Launcher>().enabled = false;

                        //Store hit object into variable
                        CurrentSelectedObject = hit.collider.gameObject;

                        //if selected object doesn't have MoveObject script then add it
                        if (!CurrentSelectedObject.GetComponent<MoveObject>()) CurrentSelectedObject.AddComponent<MoveObject>();

                        //if selected object doesn't have Bounce script then add it
                        if (!CurrentSelectedObject.GetComponent<Bounce>()) CurrentSelectedObject.AddComponent<Bounce>();

                        //if selected object doesn't have Launcher script then add it
                        if (!CurrentSelectedObject.GetComponent<Launcher>()) CurrentSelectedObject.AddComponent<Launcher>();

                        //Update Action dropdown so that proper UI for the specific choice appears
                        UIManager.UpdateActionDropdown();
                    }
                }

                //Nothing was hit so enable cam canvas and disable object canvas and refresh
                else {
                    UIManager.camCanvas = true;
                    UIManager.objectCanvas = false;
                    UIManager.UpdateCanvas();
                    //Disable launcher script from old selected object so that it doesn't keep running
                    if (CurrentSelectedObject) CurrentSelectedObject.GetComponent<Launcher>().enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Calls function that takes in user input to move camera
        MovePlayer();

        //Calls function to handle racasting from camera
        Raycasting();

        // If user presses escape program ends
        if (Input.GetKey("escape")) Application.Quit();//UnityEditor.EditorApplication.isPlaying = false;

    }

}
