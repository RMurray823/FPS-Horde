using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHUD : MonoBehaviour {

    public bool damaged = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        adjustDamageIndicator();
    }

    public void adjustDamageIndicator() {

        GameObject damageIndicator = GameObject.Find("DamageIndicator");
        Image damageSprite = damageIndicator.GetComponent<Image>();

        Color Opaque = new Color(1, 1, 1, 1);
        Color Transparent = new Color(1, 1, 1, 0);

        if (damaged) {

            damageSprite.color = Color.Lerp(damageSprite.color, Opaque, 10 * Time.deltaTime);

            if (damageSprite.color.a >= .75) {
                damaged = false;
            }

        }

        if (!damaged) {
            damageSprite.color = Color.Lerp(damageSprite.color, Transparent, 2 * Time.deltaTime);
        }

    }

}
