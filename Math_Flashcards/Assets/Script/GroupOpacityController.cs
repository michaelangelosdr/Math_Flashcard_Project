using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupOpacityController : MonoBehaviour {
	
	public IEnumerator FadeImages(GameObject g,float dir)
	{
		Image[] images= g.GetComponentsInChildren<Image>();
		Text[] text =g.GetComponentsInChildren<Text>();

		if (dir == 0) {
			foreach (Image i in images) {
				StartCoroutine (Utils.LerpingColor (i, i.color, new Color (i.color.r, i.color.g, i.color.b, dir), 0.2f));
			}
			foreach (Text i in text) {
				StartCoroutine (Utils.LerpingColor (i, i.color, new Color (i.color.r, i.color.g, i.color.b, dir), 0.2f));
			}

		} else {
			foreach (Image i in images) {
				StartCoroutine (Utils.LerpingColor (i, new Color(i.color.r,i.color.g,i.color.b,0), new Color (255f, 255f, 255f, dir), 0.2f));
			}
			foreach (Text i in text) {
				StartCoroutine (Utils.LerpingColor (i, new Color(i.color.r,i.color.g,i.color.b,0), new Color (i.color.r, i.color.g, i.color.b, dir), 0.2f));
			}


		}
		yield return new WaitForSeconds (0);
	}



}
