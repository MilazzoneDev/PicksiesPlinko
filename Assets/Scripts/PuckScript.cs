using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour {

    [Header("Kill If")]
    public float killIfBelow;
    public float killAfterSeconds;

    [Header("In Game Properties")]
    public string userName;

    float timeAlive;

	// Use this for initialization
	void Start () {
        timeAlive = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timeAlive += Time.deltaTime;
		if(this.transform.position.y < killIfBelow || timeAlive > killAfterSeconds)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
