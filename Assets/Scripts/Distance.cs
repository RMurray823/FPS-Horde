using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour {
    
    public GameObject Obj1;
    public GameObject Obj2;
    public float Distance_;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Distance_ = Vector3.Distance(Obj1.transform.position, Obj2.transform.position);
/*        if (Distance_ < 3)
        {
            Debug.Log("HELP");
        }*/
    }
}
