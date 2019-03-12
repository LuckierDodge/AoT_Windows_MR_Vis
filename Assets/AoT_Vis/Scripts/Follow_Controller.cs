using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(0f, 0f, .25f);
    }
}
