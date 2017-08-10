using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTEst : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vlog.disableError = true;
        Vlog.Info("ccc", "Test");
        Vlog.Error("aaa", 89, "ccc", "27");
        Vlog.Error(this, "aaa", "Fred", 99);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


