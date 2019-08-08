using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHUD:MonoBehaviour {

	public bool damaged = false;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		adjustDamageIndicator();
	}

	public void adjustDamageIndicator() {

		//  Find Damage Indicator and get Sprite
		GameObject damageIndicator = GameObject.Find("DamageIndicator");
		Image damageSprite = damageIndicator.GetComponent<Image>();

		//  Define colors that only affect the alpha of the image 
		Color FullyOpaque = new Color(1, 1, 1, 1);
		Color FullyTransparent = new Color(1, 1, 1, 0);

		//  If player takes damage, begin changing alpha of the image to Opaque until .75 alpha
		if (damaged) {

			damageSprite.color = Color.Lerp(damageSprite.color, FullyOpaque, 10 * Time.deltaTime);

			//  Once at .75 alpha, begin changing back alpha to transparent
			if (damageSprite.color.a >= .75) {
				damaged = false;
			}

		}

		//  Change back alpha to 0 unless it's close enough already
		if (!damaged && (System.Math.Abs(damageSprite.color.a) > 0.001)) {
			damageSprite.color = Color.Lerp(damageSprite.color, FullyTransparent, 2 * Time.deltaTime);
		}

	}

}
