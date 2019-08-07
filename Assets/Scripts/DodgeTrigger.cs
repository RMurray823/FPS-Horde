using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeTrigger : MonoBehaviour {

    //public GameObject Obj1;
    //public GameObject Obj2;
    public float Distance_;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Distance_ = Vector3.Distance(Obj1.transform.position, Obj2.transform.position);
    }

    //1
    //Trigger an object
    /*    private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name + " has entered");
        }*/

    //2
    //Collide with an object
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Boss") //If the boss hits the object, do the if statment.
        {
            //Debug.Log(collision.transform.name + " BOSS has collided");
            collision.gameObject.SendMessage("DodgeRight");
            //if distance between boss and object doesn't change, dodge other way

        }
    }

    /*private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.transform.name + " has exited");
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.transform.name + " has stayed");
    }*/




}
