using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour {

    public static Robber singleton;


	// Use this for initialization
	void Awake () {
        singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
