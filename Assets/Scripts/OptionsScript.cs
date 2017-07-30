using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{

    [Header("Cam Positions")]
    public Vector3 chatPos;
    public Vector3 withoutChatPos;
    public Vector3 viewPos;
    public Vector3 upPos;
    public Vector3 sidePos;

    [Header("Cam Rotations")]
    public Vector3 normalRot;
    public Vector3 upRot;
    public Vector3 sideRot;

    [Header("screens")]
    public GameObject optionsMenu;
    public GameObject chatWindow;
    public GameObject InstructionsStart;
    public GameObject InstructionsPlay;
    public GameObject InstructionsBot;
    public GameObject OtherSettings;

    [Header("Settings")]
    public Toggle chatToggle;
    public Toggle optionsButtonToggle;
    public Image chromaImage;
    public InputField chromaInput;

    [Header("BoardOptionRadioButtons")]
    public Toggle LinesBoardToggle;
    public Toggle CircleBoardToggle;
    public Toggle SpiralBoardToggle;

    [Header("PuckSettingsSliders")]
    public GameObject puckPrefab;
    public PhysicMaterial puckMaterial;
    public Slider puckSizeSlider;
    public Slider puckBounceSlider;
    public Slider puckFrictionSlider;
    public InputField puckSizeText;

    [Header("LinesBoardSliders")]
    public GameObject LinesOptions;
    public Slider lineRowsSlider;
    public Slider lineColsSlider;
    public Toggle lineAltToggle;
    public InputField lineRowsText;
    public InputField lineColsText;

    [Header("SpiralBoardSliders")]
    public GameObject SpiralOptions;
    public Slider spiArmsSlider;
    public Slider spiOffsetSlider;
    public Slider spiDensitySlider;
    public InputField spiArmsText;
    public InputField spiOffsetText;
    public InputField spiDensityText;

    [Header("CircleBoardSliders")]
    public GameObject CircleOptions;
    public Slider cirRingsSlider;
    public Slider cirOffsetSlider;
    public Slider cirDensitySlider;
    public InputField cirRingsText;
    public InputField cirOffsetText;
    public InputField cirDensityText;
    public Toggle cirAltOffsetToggle;

    [Header("objects")]
    public GameObject SetupObject;
    public GameObject optionsButton;
    public GameObject launcher;
    public Camera cam;

    private bool optionsButtonActive;
    private bool chatActive;
    private bool shouldIgnoreOnChanged = false;


    // Use this for initialization
    void Start()
    {
        CloseMenu(); //in case it's already open
        HideOtherOptionsScreens();

        ShowMenu();
        ToggleChat();
        ToggleOptions();
        cam.transform.position = chatPos;
        cam.transform.rotation = Quaternion.Euler(normalRot);
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = Color.black;




        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];
        if (setupscript != null)
        {
            shouldIgnoreOnChanged = true;
            // board type
            if (LinesBoardToggle != null && setupscript.drawAsCircle == false && setupscript.drawAsSprial == false)
            {
                LinesBoardToggle.isOn = setupscript.drawAsCircle == false && setupscript.drawAsSprial == false;
            }
            if (CircleBoardToggle != null && setupscript.drawAsCircle)
            {
                CircleBoardToggle.isOn = setupscript.drawAsCircle;
            }
            if (SpiralBoardToggle != null && setupscript.drawAsSprial)
            {
                CircleBoardToggle.isOn = setupscript.drawAsSprial;
            }

            // lines
            if (lineRowsSlider != null && lineRowsText != null)
            {
                lineRowsSlider.value = setupscript.numRows;
                lineRowsText.text = setupscript.numRows.ToString();
            }
            if (lineColsSlider != null && lineColsText != null)
            {
                lineColsSlider.value = setupscript.numCols;
                lineColsText.text = setupscript.numCols.ToString();
            }
            if (lineAltToggle != null)
            {
                lineAltToggle.isOn = setupscript.alternatePegs;
            }

            // spiral
            if (spiArmsSlider != null && spiArmsText != null)
            {
                spiArmsSlider.value = setupscript.spiArms;
                spiArmsText.text = setupscript.spiArms.ToString();
            }
            if (spiOffsetSlider != null && spiOffsetText != null)
            {
                spiOffsetSlider.value = setupscript.spiOffset;
                spiOffsetText.text = setupscript.spiOffset.ToString();
            }
            if (spiDensitySlider != null && spiDensityText != null)
            {
                spiDensitySlider.value = setupscript.spiSteps;
                spiDensityText.text = setupscript.spiSteps.ToString();
            }

            // circle
            if (cirRingsSlider != null && cirRingsText != null)
            {
                cirRingsSlider.value = setupscript.cirRings;
                cirRingsText.text = setupscript.cirRings.ToString();
            }
            if (cirOffsetSlider != null && cirOffsetText != null)
            {
                cirOffsetSlider.value = setupscript.cirOffset;
                cirOffsetText.text = setupscript.cirOffset.ToString();
            }
            if (cirDensitySlider != null && cirDensityText != null)
            {
                cirDensitySlider.value = setupscript.cirRingDensity;
                cirDensityText.text = setupscript.cirRingDensity.ToString();
            }
            if (cirAltOffsetToggle != null)
            {
                cirAltOffsetToggle.isOn = setupscript.cirAltOffset;
            }

            //puck and pegs
            if (puckSizeSlider != null && puckSizeText != null && puckPrefab != null)
            {
                puckSizeSlider.value = puckPrefab.transform.localScale.x;
                puckSizeText.text = puckSizeSlider.value.ToString();

            }


            shouldIgnoreOnChanged = false;
            ChangeBoardType();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("ChatCam") > 0)
        {
            if (chatActive)
            {
                cam.transform.position = chatPos;
            }
            else
            {
                cam.transform.position = withoutChatPos;
            }

            cam.transform.rotation = Quaternion.Euler(normalRot);
            SetChat(true);
            ViewLauncher(true);
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = Color.black;

        }
        else if (Input.GetAxis("ViewCam") > 0)
        {
            cam.transform.position = viewPos;
            cam.transform.rotation = Quaternion.Euler(normalRot);
            SetChat(false);
            ViewLauncher(true);
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = Color.black;
        }
        /*
        else if (Input.GetAxis("SideCam") > 0)
        {
            cam.transform.position = sidePos;
            cam.transform.rotation = Quaternion.Euler(sideRot);
            SetChat(false);
            ViewLauncher(true);
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = Color.black;
        }
        else if (Input.GetAxis("UpCam") > 0)
        {
            cam.transform.position = upPos;
            cam.transform.rotation = Quaternion.Euler(upRot);
            SetChat(false);
            ViewLauncher(false);
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = Color.black;
        }
        */

        if (Input.GetAxis("OpenMenu") > 0)
        {
            ShowMenu();
        }
    }

    void ShowMenu()
    {
        HideChat();
        HideOptions();
        ShowLauncher(false);

        //do last to make sure options and chat are hidden
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
        }
    }

    void CloseMenu()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }
        SetChat(chatActive);
        SetOptionsButton(optionsButtonActive);
        ShowLauncher(true);
    }

    //
    //variable menu toggles
    //

    void SetOptionsButton(bool setTo)
    {
        if (optionsButton != null && optionsButtonToggle != null)
        {
            if (!optionsMenu.activeSelf)
            {
                optionsButton.SetActive(setTo);
                optionsButtonToggle.isOn = setTo;
            }
            if (optionsMenu.activeSelf)
            {
                optionsButtonToggle.isOn = setTo;
            }
        }
    }

    void ToggleOptions()
    {
        SetOptionsButton(!optionsButtonActive);
    }

    void ChangeOptions()
    {
        if (optionsButton != null)
        {
            optionsButtonActive = optionsButtonToggle.isOn;
        }
    }

    void HideOptions()
    {
        if (optionsButton != null)
        {
            optionsButton.SetActive(false);
        }
    }

    void SetChat(bool setActive)
    {
        if (chatWindow != null && chatToggle != null)
        {
            if (!optionsMenu.activeSelf)
            {
                chatWindow.SetActive(setActive);
                chatToggle.isOn = setActive;
            }
            if (optionsMenu.activeSelf)
            {
                chatToggle.isOn = setActive;
            }
        }
    }

    void ToggleChat()
    {
        SetChat(!chatActive);
    }

    void ChangeChat()
    {
        if (chatWindow != null)
        {
            chatActive = chatToggle.isOn;
        }

        if (cam.transform.position == chatPos)
        {
            if (!chatActive)
            {
                cam.transform.position = withoutChatPos;
            }
        }
        else if (cam.transform.position == withoutChatPos)
        {
            if (chatActive)
            {
                cam.transform.position = chatPos;
            }
        }
    }

    void HideChat()
    {
        if (chatWindow != null)
        {
            chatWindow.SetActive(false);
        }
    }

    void ViewLauncher(bool setAcitve)
    {
        if (launcher != null)
        {
            launcher.GetComponent<MeshRenderer>().enabled = setAcitve;
        }
    }

    void ShowLauncher(bool setActive)
    {
        if (launcher != null)
        {
            launcher.SetActive(setActive);
        }
    }

    //
    //instruction screens
    //

    void HideMenu(bool shouldHide)
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(!shouldHide);
        }
    }

    void ViewStart()
    {
        HideMenu(true);
        if (InstructionsStart != null)
        {
            InstructionsStart.SetActive(true);
        }
    }

    void ViewPlay()
    {
        HideMenu(true);
        if (InstructionsPlay != null)
        {
            InstructionsPlay.SetActive(true);
        }
    }

    void ViewBot()
    {
        HideMenu(true);
        if (InstructionsBot != null)
        {
            InstructionsBot.SetActive(true);
        }
    }

    void ViewOtherSettings()
    {
        HideMenu(true);
        if (OtherSettings != null)
        {
            OtherSettings.SetActive(true);
        }
    }

    void HideInstructionsFromOptions()
    {
        HideOtherOptionsScreens();
        HideMenu(false);
    }

    void HideOtherOptionsScreens()
    {
        if (InstructionsStart != null)
        {
            InstructionsStart.SetActive(false);
        }
        if (InstructionsPlay != null)
        {
            InstructionsPlay.SetActive(false);
        }
        if (InstructionsBot != null)
        {
            InstructionsBot.SetActive(false);
        }
        if (OtherSettings != null)
        {
            OtherSettings.SetActive(false);
        }
    }

    void ChangeChroma()
    {
        Color newColor = Color.clear;
        if (chromaInput.text != "")
        {
            newColor = ColorPicker.pickColor(chromaInput.text);

        }
        if (newColor == Color.clear)
        {
            newColor = Color.black;
        }

        chromaImage.color = newColor;
        cam.backgroundColor = newColor;

    }

    void ChangeBoardType(bool drawAsCircle, bool drawAsSprial)
    {

    }


    // ------------------Spiral change events------------------
    public void ChangeBoardType()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (LinesOptions != null) { LinesOptions.SetActive(false); }
        if (CircleOptions != null) { CircleOptions.SetActive(false); }
        if (SpiralOptions != null) { SpiralOptions.SetActive(false); }

        if (LinesBoardToggle && LinesBoardToggle.isOn)
        {
            setupscript.SetLinesBoard();
            if (LinesOptions != null) { LinesOptions.SetActive(true); }
        }
        if (CircleBoardToggle && CircleBoardToggle.isOn)
        {
            setupscript.SetCircleBoard();
            if (CircleOptions != null) { CircleOptions.SetActive(true); }
        }
        if (SpiralBoardToggle && SpiralBoardToggle.isOn)
        {
            setupscript.SetSpiralBoard();
            if (SpiralOptions != null) { SpiralOptions.SetActive(true); }
        }
        setupscript.initializeBoard();
    }

    // ------------------Lines change events------------------

    public void ChangeBoardLinesViaSliders()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (lineRowsSlider != null && lineRowsText != null)
        {
            setupscript.SetLineRows((int)lineRowsSlider.value);
            lineRowsText.text = lineRowsSlider.value.ToString();
        }
        if (lineColsSlider != null && lineColsText != null)
        {
            setupscript.SetLineCols((int)lineColsSlider.value);
            lineColsText.text = lineColsSlider.value.ToString();
        }
        if (lineAltToggle != null)
        {
            setupscript.SetLineAlt(lineAltToggle.isOn);
        }
        setupscript.initializeBoard();
    }

    public void ChangeBoardLinesViaText()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (lineRowsSlider != null && lineRowsText != null)
        {
            setupscript.SetLineRows(Convert.ToInt32(lineRowsText.text));
            lineRowsSlider.value = Convert.ToInt32(lineRowsText.text);
        }
        if (lineColsSlider != null && lineColsText != null)
        {
            setupscript.SetLineCols(Convert.ToInt32(lineColsText.text));
            lineColsSlider.value = Convert.ToInt32(lineColsText.text);
        }
        setupscript.initializeBoard();
    }

    // ------------------Circle change events------------------

    public void ChangeBoardCircleViaSliders()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (cirRingsSlider != null && cirRingsText != null)
        {
            setupscript.SetCirRings((int)cirRingsSlider.value);
            cirRingsText.text = cirRingsSlider.value.ToString();
        }
        if (cirOffsetSlider != null && cirOffsetText != null)
        {
            setupscript.SetCirOffset(cirOffsetSlider.value);
            cirOffsetText.text = cirOffsetSlider.value.ToString();
        }
        if (cirDensitySlider != null && cirDensityText != null)
        {
            setupscript.SetCirRingDensity((int)cirDensitySlider.value);
            cirDensityText.text = cirDensitySlider.value.ToString();
        }
        if (cirAltOffsetToggle != null)
        {
            setupscript.SetCirAltOffset(cirAltOffsetToggle.isOn);
        }
        setupscript.initializeBoard();
    }

    public void ChangeBoadCircleViaText()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (cirRingsSlider != null && cirRingsText != null)
        {
            setupscript.SetCirRings(Convert.ToInt32(cirRingsText.text));
            cirRingsSlider.value = Convert.ToInt32(cirRingsText.text);
        }
        if (cirOffsetSlider != null && cirOffsetText != null)
        {
            setupscript.SetCirOffset(float.Parse(cirOffsetText.text));
            cirOffsetSlider.value = float.Parse(cirOffsetText.text);
        }
        if (cirDensitySlider != null && cirDensityText != null)
        {
            setupscript.SetCirRingDensity(Convert.ToInt32(cirDensityText.text));
            cirDensitySlider.value = Convert.ToInt32(cirDensityText.text);
        }
        setupscript.initializeBoard();
    }

    // ------------------Spiral change events------------------

    public void ChangeBoardSpiralViaSliders()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (spiArmsSlider != null && spiArmsText != null)
        {
            setupscript.SetSpiArms((int)spiArmsSlider.value);
            spiArmsText.text = spiArmsSlider.value.ToString();
        }
        if (spiOffsetSlider != null && spiOffsetText != null)
        {
            setupscript.SetSpiOffset(spiOffsetSlider.value);
            spiOffsetText.text = spiOffsetSlider.value.ToString();
        }
        if (spiDensitySlider != null && spiDensityText != null)
        {
            setupscript.SetSpiSteps((int)spiDensitySlider.value);
            spiDensityText.text = spiDensitySlider.value.ToString();
        }
        setupscript.initializeBoard();
    }

    public void ChangeBoadSpiralViaText()
    {
        BoardSetupScript setupscript = SetupObject.GetComponents<BoardSetupScript>()[0];

        if (setupscript == null || shouldIgnoreOnChanged) { return; }
        if (spiArmsSlider != null && spiArmsText != null)
        {
            setupscript.SetSpiArms(Convert.ToInt32(spiArmsText.text));
            spiArmsSlider.value = Convert.ToInt32(spiArmsText.text);
        }
        if (spiOffsetSlider != null && spiOffsetText != null)
        {
            setupscript.SetSpiOffset(float.Parse(spiOffsetText.text));
            spiOffsetSlider.value = float.Parse(spiOffsetText.text);
        }
        if (spiDensitySlider != null && spiDensityText != null)
        {
            setupscript.SetSpiSteps(Convert.ToInt32(spiDensityText.text));
            spiDensitySlider.value = Convert.ToInt32(spiDensityText.text);
        }
        setupscript.initializeBoard();
    }

    // ------------------Puck and Pegs change events------------------
    public void ChangePuckViaSliders()
    {

        if (shouldIgnoreOnChanged) { return; }
        if (puckSizeSlider != null && puckSizeText != null && puckPrefab != null)
        {
            puckSizeText.text = puckSizeSlider.value.ToString();
            puckPrefab.transform.localScale = new Vector3(puckSizeSlider.value, puckPrefab.transform.localScale.y, puckSizeSlider.value);
        }
    }

    public void ChangePuckViaText()
    {
        if (shouldIgnoreOnChanged) { return; }
    }

}