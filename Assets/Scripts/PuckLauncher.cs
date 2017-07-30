using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckLauncher : MonoBehaviour {

    [Header("BoardObjects")]
    public BoardSetupScript boardSetup;
    public TwitchParser parser;
    public GameObject puckPrefab;

    bool stopMultiDrop;

    // Use this for initialization
    void Start () {
        if (parser)
        {
            parser.recievedParseEvent.AddListener(OnMessage);
        }
	}

    void OnMessage(UserMessage cmd)
    {
        if (cmd.command.Equals("!drop") || cmd.command.Equals("!Drop"))
        {
            Launch(makePuck(cmd));
        }
    }

    GameObject makePuck(UserMessage cmd)
    {
        Color newColor = ColorPicker.pickColor("random");
        GameObject puck = GameObject.Instantiate(puckPrefab);
        if (cmd.args != null)
        {
            newColor = ColorPicker.pickColor(cmd.args[0]);
        }
        puck.GetComponent<Renderer>().material.color = newColor;
        puck.GetComponent<PuckScript>().userName = cmd.user;
        return puck;
    }

    GameObject makePuck(Color newColor)
    {
        GameObject puck = GameObject.Instantiate(puckPrefab);
        puck.GetComponent<Renderer>().material.color = newColor;

        return puck;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("RandomDrop") > 0)
        {
            if (!stopMultiDrop)
            {
                stopMultiDrop = true;
                Launch(makePuck(ColorPicker.pickColor("random")));
            }
            return;
        }
        stopMultiDrop = false;
    }

    public void Launch(GameObject puck)
    {
        if(puck != null && boardSetup != null)
        {
            puck.transform.position = this.transform.position;
            float Displace = Random.value * (boardSetup.BoardSize.y - puck.transform.localScale.y);
            Displace = Displace - (boardSetup.BoardSize.y - puck.transform.localScale.y) / 2.0f;
            Vector3 DisplacePos = new Vector3(0, 0, Displace);
            puck.transform.position += DisplacePos;
        }
    }

}
