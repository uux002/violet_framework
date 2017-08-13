using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TaskTest : MonoBehaviour {

    public Transform trans = null;

    // Use this for initialization
    void Start() {
        StartTest(); 
    }
	
	// Update is called once per frame
	void Update () {
        trans.eulerAngles += new Vector3(0, 0, 100 * Time.deltaTime);	
	}

    private async void StartTest() {
        await ToTest();
        Debug.Log("Test Ended And OK");
    }


    async Task ToTest() {
        Task t1 = new Task(MyMethod);
        t1.Start();
        await t1; 
        Debug.Log("test End");
    }

    void MyMethod() {
        for(int i = 0; i < 5; ++i) {
            Debug.Log("======> " + System.DateTime.Now);
            System.Threading.Thread.Sleep(2000);
        }
    }

}
