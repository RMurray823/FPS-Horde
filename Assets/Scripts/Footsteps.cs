using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    private AudioSource m_Audio;
    private CharacterController charcontrol;
    
    public CharacterController _CharController;
    //private Rigidbody _RigBod;

    // Use this for initialization
    void Start () {

        charcontrol = GetComponent<CharacterController>();
        m_Audio = GetComponent<AudioSource>();		
	}
	
	// Update is called once per frame
	void Update () {

        bool stepping = charcontrol.isGrounded == true && charcontrol.velocity.magnitude > 2f && m_Audio.isPlaying == false;
        if (stepping)
        {
            m_Audio.Play();
        }
		
	}
}
