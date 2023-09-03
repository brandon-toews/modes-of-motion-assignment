using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.VisualScripting;
using System;

//UI Management Script
public class UIManager : MonoBehaviour
{
    //Cube and Sphere prefabs, arrays, & array amounts
    [SerializeField] private GameObject CubePrefab;
    [SerializeField] private GameObject SpherePrefab;
    public GameObject[] Cube;
    public GameObject[] Sphere;
    int maxCubes = 10;
    int maxSpheres = 10;

    //Canvas'
    [SerializeField] private GameObject CamEffectCanvas;
    [SerializeField] private GameObject ObjectLerpCanvas;

    //Object Lerp Canvas panels
    [SerializeField] private GameObject LerpPanel;
    [SerializeField] private GameObject BouncePanel;
    [SerializeField] private GameObject LaunchPanel;

    //UI Text fields to display slider amounts and info
    [SerializeField] private TextMeshProUGUI CamScalarText;
    [SerializeField] private TextMeshProUGUI CamEffectAmountText;
    [SerializeField] private TextMeshProUGUI ObjectNameText;
    [SerializeField] private TextMeshProUGUI ObjectShapeText;
    [SerializeField] private TextMeshProUGUI ObjectXLocText;
    [SerializeField] private TextMeshProUGUI ObjectYLocText;
    [SerializeField] private TextMeshProUGUI ObjectZLocText;
    [SerializeField] private TextMeshProUGUI ObjectXDistAmountText;
    [SerializeField] private TextMeshProUGUI ObjectYDistAmountText;
    [SerializeField] private TextMeshProUGUI ObjectZDistAmountText;
    [SerializeField] private TextMeshProUGUI BounceHgtAmountText;
    [SerializeField] private TextMeshProUGUI BounceSpeedAmountText;
    [SerializeField] private TextMeshProUGUI VelAmountText;
    [SerializeField] private TextMeshProUGUI AngAmountText;

    //Cam Dropdown menus
    [SerializeField] private TMP_Dropdown CamEffectDropdown;
    [SerializeField] private TMP_Dropdown CamEaseDropdown;
    [SerializeField] private TMP_Dropdown CamZoomDropdown;
    [SerializeField] private TMP_Dropdown CamRotDirDropdown;

    //Object Movement dropdown menu and start movement button
    [SerializeField] private TMP_Dropdown ActionDropdown;
    [SerializeField] private UnityEngine.UI.Button ActionButton;

    //Lerp axis ease dropdowns
    [SerializeField] private TMP_Dropdown ObjectXEaseDropdown;
    [SerializeField] private TMP_Dropdown ObjectYEaseDropdown;
    [SerializeField] private TMP_Dropdown ObjectZEaseDropdown;

    //Lerp axis pingpong toggles & continuous toggles for bounce and lerp movements
    [SerializeField] private UnityEngine.UI.Toggle PingPongXToggle;
    [SerializeField] private UnityEngine.UI.Toggle PingPongYToggle;
    [SerializeField] private UnityEngine.UI.Toggle PingPongZToggle;
    [SerializeField] private UnityEngine.UI.Toggle LerpContinuousToggle;
    [SerializeField] private UnityEngine.UI.Toggle BounceContinuousToggle;

    //Cam, lerp axis distance, bounce speed & height, launch velocity and angle sliders
    [SerializeField] private UnityEngine.UI.Slider CamEffectSlider;
    [SerializeField] private UnityEngine.UI.Slider XDistSlider;
    [SerializeField] private UnityEngine.UI.Slider YDistSlider;
    [SerializeField] private UnityEngine.UI.Slider ZDistSlider;
    [SerializeField] private UnityEngine.UI.Slider BounceHeightSlider;
    [SerializeField] private UnityEngine.UI.Slider BounceSpeedSlider;
    [SerializeField] private UnityEngine.UI.Slider VelSlider;
    [SerializeField] private UnityEngine.UI.Slider AngSlider;


    //Slider variables ammounts
    public static float camEffectAmount = 0f;
    public static float xDistAmount = 0f;
    public static float yDistAmount = 0f;
    public static float zDistAmount = 0f;
    public static float bounceHgtAmount = 10f;
    public static float bounceSpeedAmount = 0f;
    public static float velAmount = 0f;
    public static float angAmount = 0f;

    //Object start location variable
    public static Vector3 objLoc = Vector3.zero;

    //Vars for storing cam dropdown values
    public static int camEffectDropPick = 0;
    public static int camEaseDropPick = 0;
    public static int camZoomDropPick = 0;
    public static int camRotDirDropPick = 0;

    //Var for storing object movement choice from movement dropdown
    public static int actionDropPick = 0;

    //Vars for storing lerp axis ease values from dropdown choices
    public static int objXEaseDropPick = 0;
    public static int objYEaseDropPick = 0;
    public static int objZEaseDropPick = 0;

    //Vars for storing lerp axis pingpong values from pingpong toggle choices
    public static bool pingPongX = true;
    public static bool pingPongY = true;
    public static bool pingPongZ = true;

    //Vars for storing movement continuous value choices
    public static bool lerpContinuous = true;
    public static bool bounceContinuous = true;

    //Vars that can be set by PlayerControls script to turn off and on canvas'
    public static bool camCanvas;
    public static bool objectCanvas;

    //Vars for storing current selected objects name and shape
    public static string objectNameText = "None";
    public static string objectShapeText = "None";

    //Created instance to be able to change values in this script from other scripts
    public static UIManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        //Store this instance to use later
        Instance = this;

        //Set begining UI interface
        camCanvas = true;
        objectCanvas = false;
        CamEffectCanvas.SetActive(camCanvas);
        ObjectLerpCanvas.SetActive(objectCanvas);
        CamScalarText.text = "Shake Severity: ";

        CamEffectAmountText.text = camEffectAmount.ToString();
        ObjectXDistAmountText.text = xDistAmount.ToString();
        ObjectYDistAmountText.text = yDistAmount.ToString();
        ObjectZDistAmountText.text = zDistAmount.ToString();

        CamEaseDropdown.gameObject.SetActive(false);
        CamZoomDropdown.gameObject.SetActive(false);
        CamRotDirDropdown.gameObject.SetActive(false);

        //Create cube and sphere arrays for generating objects later
        Cube = new GameObject[maxCubes];
        Sphere = new GameObject[maxSpheres];
    }

    //Function for setting canvas' & some UI elements 
    public static void UpdateCanvas()
    {
        //Displays selected objects stats while turning on and off the proper canvas'
        UIManager.Instance.ObjectNameText.text = objectNameText;
        UIManager.Instance.ObjectShapeText.text = objectShapeText;
        UIManager.Instance.ObjectXLocText.text = objLoc.x.ToString("F1");
        UIManager.Instance.ObjectYLocText.text = objLoc.y.ToString("F1");
        UIManager.Instance.ObjectZLocText.text = objLoc.z.ToString("F1");
        UIManager.Instance.CamEffectCanvas.SetActive(camCanvas);
        UIManager.Instance.ObjectLerpCanvas.SetActive(objectCanvas);
    }

    //Function that is called everytime Cam dropdowns are changed
    public void UpdateCamDropdownPicks()
    {
        //Store Cam dropdown movement pick in var
        camEffectDropPick = CamEffectDropdown.value;

        //Switch case updates cam canvas according to choice selected from cam effect dropdown
        switch (camEffectDropPick)
        {
            //If shake effect selected turn of dropdowns for other effects, change slider max, & show slider amount
            case 0:
                CamEffectSlider.maxValue = 50;
                CamScalarText.text = "Shake Severity: ";
                CamEffectAmountText.text = camEffectAmount.ToString();
                CamEaseDropdown.gameObject.SetActive(false);
                CamZoomDropdown.gameObject.SetActive(false);
                CamRotDirDropdown.gameObject.SetActive(false);
                break;

            //If rotate effect selected turn of dropdowns for other effects, change slider max, & show slider amount
            case 1:
                CamEffectSlider.maxValue = 500;
                CamScalarText.text = "Rotate Amount: ";
                CamEffectAmountText.text = camEffectAmount.ToString();
                CamEaseDropdown.gameObject.SetActive(true);
                CamZoomDropdown.gameObject.SetActive(false);
                CamRotDirDropdown.gameObject.SetActive(true);
                break;

            //If zoom effect selected turn of dropdowns for other effects, change slider max, & show slider amount
            case 2:
                CamEffectSlider.maxValue = 5;
                CamScalarText.text = "Zoom Amount: ";
                CamEffectAmountText.text = camEffectAmount.ToString();
                CamEaseDropdown.gameObject.SetActive(true);
                CamZoomDropdown.gameObject.SetActive(true);
                CamRotDirDropdown.gameObject.SetActive(false);
                break;

            default: break;
        }

        //store other cam drop pick values
        camEaseDropPick = CamEaseDropdown.value;
        camZoomDropPick = CamZoomDropdown.value;
        camRotDirDropPick = CamRotDirDropdown.value;
    }

    //Function called everytime Object movement dropdown is changed
    public static void UpdateActionDropdown()
    {
        //Store drop pick choice in var
        actionDropPick = UIManager.Instance.ActionDropdown.value;
        //Call function to update slider vars with slider values
        UpdateSliders();
        //Call function to change label for movement start button to reflect the movement chosen
        UpdateActionButton();
    }

    //Function to enabled & disable object movement panels & change label for movement start button to reflect
    //the movement chosen, called everytime object movement dropdown is changed
    public static void UpdateActionButton()
    {

        //Switch case updates movement panels & button according to choice selected from Object movement dropdown
        switch (actionDropPick)
        {
            //If lerp option is chosen then turn on appropriate panel & script while turning off others,
            //also change movement start button label to match action chosen
            case 0:
                UIManager.Instance.LerpPanel.SetActive(true);
                UIManager.Instance.BouncePanel.SetActive(false);
                UIManager.Instance.LaunchPanel.SetActive(false);
                PlayerControls.CurrentSelectedObject.GetComponent<MoveObject>().enabled = true;
                PlayerControls.CurrentSelectedObject.GetComponent<Bounce>().enabled = false;
                PlayerControls.CurrentSelectedObject.GetComponent<Launcher>().enabled = false;
                UIManager.Instance.ActionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Lerp";
                break;

            //If bounce option is chosen then turn on appropriate panel & script while turning off others,
            //also change movement start button label to match action chosen
            case 1:
                UIManager.Instance.BouncePanel.SetActive(true);
                UIManager.Instance.LerpPanel.SetActive(false);
                UIManager.Instance.LaunchPanel.SetActive(false);
                PlayerControls.CurrentSelectedObject.GetComponent<Bounce>().enabled = true;
                PlayerControls.CurrentSelectedObject.GetComponent<MoveObject>().enabled = false;
                PlayerControls.CurrentSelectedObject.GetComponent<Launcher>().enabled = false;
                UIManager.Instance.ActionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Bounce";
                break;

            //If launch option is chosen then turn on appropriate panel & script while turning off others,
            //also change movement start button label to match action chosen
            case 2:
                UIManager.Instance.LaunchPanel.SetActive(true);
                UIManager.Instance.LerpPanel.SetActive(false);
                UIManager.Instance.BouncePanel.SetActive(false);
                PlayerControls.CurrentSelectedObject.GetComponent<Launcher>().enabled = true;
                PlayerControls.CurrentSelectedObject.GetComponent<MoveObject>().enabled = false;
                PlayerControls.CurrentSelectedObject.GetComponent<Bounce>().enabled = false;
                UIManager.Instance.ActionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Launch";
                break;
        }
    }

    //Function to create cubes called everytime player presses "Create Cube" button
    public void CreateCube()
    {
        //Start for loop that will only go as high as the max set for cubes
        for (int i = 0; i < maxCubes; i++)
        {
            //if current cube in loop has not been created then create it
            if (!Cube[i])
            {
                //create cube & place it in front of where the camera is looking
                Cube[i] = Instantiate(CubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10, Camera.main.transform.rotation) as GameObject;
                //set i to the cube max so that only one cube is created
                i = maxCubes;
            }

        }
    }

    //Function to create Spheres called everytime player presses "Create Sphere" button
    public void CreateSphere()
    {
        //Start for loop that will only go as high as the max set for cubes
        for (int i = 0; i < maxSpheres; i++)
        {
            //if current sphere in loop has not been created then create it
            if (!Sphere[i])
            {
                //create sphere & place it in front of where the camera is looking
                Sphere[i] = Instantiate(SpherePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10, Camera.main.transform.rotation) as GameObject;
                //set i to the sphere max so that only one sphere is created
                i = maxSpheres;
            }

        }
    }

    //Function to store lerp axis ease picks in vars, called every time lerp ease dropdown changes
    public void UpdateEaseDropdown()
    {
        //store ease drop pick values
        objXEaseDropPick = ObjectXEaseDropdown.value;
        objYEaseDropPick = ObjectYEaseDropdown.value;
        objZEaseDropPick = ObjectZEaseDropdown.value;
    }

    //Function called everytime a slider changes that store slider values and updates UI for user feedback
    public static void UpdateSliders()
    {

        //If cam canvas is on then only the cam slider needs to be updated with corresponding UI element
        if (UIManager.Instance.CamEffectCanvas.activeSelf == true)
        {
            camEffectAmount = UIManager.Instance.CamEffectSlider.value;
            UIManager.Instance.CamEffectAmountText.text = camEffectAmount.ToString();

        }
        //Object canvas is enabled so only object canvas sliders need to be updated with corresponding UI elements
        else
        {
            //Switch case checks which movement option is selected so that only the sliders and UI that
            //are on get updated
            switch (actionDropPick)
            {
                //if lerp option is selected update lerp sliders & UI
                case 0:
                    xDistAmount = UIManager.Instance.XDistSlider.value;
                    UIManager.Instance.ObjectXDistAmountText.text = xDistAmount.ToString();

                    yDistAmount = UIManager.Instance.YDistSlider.value;
                    UIManager.Instance.ObjectYDistAmountText.text = yDistAmount.ToString();

                    zDistAmount = UIManager.Instance.ZDistSlider.value;
                    UIManager.Instance.ObjectZDistAmountText.text = zDistAmount.ToString();
                    break;
                //if bounce option is selected update bounce sliders & UI
                case 1:
                    bounceHgtAmount = UIManager.Instance.BounceHeightSlider.value;
                    UIManager.Instance.BounceHgtAmountText.text = bounceHgtAmount.ToString();

                    bounceSpeedAmount = UIManager.Instance.BounceSpeedSlider.value;
                    UIManager.Instance.BounceSpeedAmountText.text = bounceSpeedAmount.ToString();

                    Bounce.bounceHeight = bounceHgtAmount;
                    Bounce.bounceSpeed = bounceSpeedAmount;
                    Bounce.detectTopCollision = false;
                    break;
                //if Launch option is selected update launch sliders & UI
                case 2:
                    velAmount = UIManager.Instance.VelSlider.value;
                    UIManager.Instance.VelAmountText.text = velAmount.ToString();

                    angAmount = UIManager.Instance.AngSlider.value;
                    UIManager.Instance.AngAmountText.text = angAmount.ToString();

                    Launcher.launchVelocity = velAmount;
                    Launcher.launchAngle = angAmount;
                    break;

                default:
                    break;
            }
        }
    }

    //Function stores toggle values in var dependign on which ones are enable everytime a toggle changes
    public void UpdateToggles()
    {
        //Switch case checks which movement option is selected so that only the toggles that are on get updated
        switch (actionDropPick)
        {
            //if lerp option is selected update lerp toggle vars
            case 0:
                pingPongX = PingPongXToggle.isOn;
                pingPongY = PingPongYToggle.isOn;
                pingPongZ = PingPongZToggle.isOn;
                lerpContinuous = LerpContinuousToggle.isOn;
                break;
            //if bounce option is selected update lerp toggle vars
            case 1:
                bounceContinuous = BounceContinuousToggle.isOn;
                break;
            //no toggles for launch option
            case 2: break;

            default: break;
        }
    }

    //Function that accesses the correct script and runs the script according to choice selected,
    //called everytime the start action button is pressed
    public void StartActionButton()
    {
        //Switch case checks which movement option is selected so that the proper script & functions are initiated
        switch (actionDropPick)
        {
            //if lerp option is selected then the StartLerp function is started on the MoveObject script from the
            //selected object
            case 0:
                PlayerControls.CurrentSelectedObject.TryGetComponent<MoveObject>(out MoveObject move);
                move.StartLerp();
                break;
            //if bounce option is selected then the StartBounce function is started on the Bounce script from the
            //selected object
            case 1:
                PlayerControls.CurrentSelectedObject.TryGetComponent<Bounce>(out Bounce bounce);
                bounce.StartBounce();
                break;
            //if launch option is selected then the launch var is changed to true on the Launcher script from the
            //selected object, which launches the object
            case 2:
                PlayerControls.CurrentSelectedObject.TryGetComponent<Launcher>(out Launcher launch);
                launch.launch = true;
                break;

            default: break;
        }
    }

    //Function that resets the object to original starting postion after launch script was run,
    //Is called everytime the "Reset" button is pressed
    public void ResetButton()
    {
        //accesses the Launcher script from selected object and stops launch and runs ResetLaunch function
        //in script
        PlayerControls.CurrentSelectedObject.TryGetComponent<Launcher>(out Launcher launch);
        launch.launch = false;
        launch.ResetLaunch();
    }

}