using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using TMPro;

public enum Curve {
    Linear = 0,
    SmoothStep = 1,
    SmootherStep = 2,
    EaseInSine = 3,
    EaseOutSine = 4,
    EaseInExp = 5,
    EaseOutExp = 6
}

/// <summary>
/// Class where common functions are placed
/// </summary>
public class Utils {

    /// <summary>
    /// Shuffles a list using the Fisher-Yates algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(ref List<T> list) {
        
        for(int i = list.Count - 1; i > 0; i--) {

            int randomIndex = Random.Range(0, i);

            T tempItem = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = tempItem;
        }
    }

    public static void Shuffle<T>(ref T[] array) {
        for(int i = array.Length - 1; i > 0; i--) {

            int randomIndex = Random.Range(0, i);

            T tempItem = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = tempItem;
        }
    }

    /// <summary>
    /// Checks if two lists contain the same elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listA"></param>
    /// <param name="listB"></param>
    /// <returns></returns>
    public static bool ListsAreEqual<T>(List<T> listA, List<T> listB) {
        if(listA.Count != listB.Count) {
            return false;
        }

        List<T> listACopy = new List<T>(listA);
        List<T> listBCopy = new List<T>(listB);

        while(listACopy.Count > 0) {
            T element = listACopy[0];

            if(listBCopy.Contains(element)) {
                listBCopy.Remove(element);
                listACopy.RemoveAt(0);
            } else {
                return false;
            }
        }

        return true;
    }

    #region Position

    static float Evaluate(Curve type, float time) {
        switch(type) {
            case Curve.Linear:
                return time;
            case Curve.SmoothStep:
                return (time * time) * (3f - (2f * time));
            case Curve.SmootherStep:
                return (time * time * time) * (time * ((6f * time) - 15f) + 10f);
            case Curve.EaseInSine:
                return 1f - Mathf.Cos(time * Mathf.PI * 0.5f);
            case Curve.EaseInExp:
                return time * time * time;
            case Curve.EaseOutSine:
                return Mathf.Sin(time * Mathf.PI * 0.5f);
            case Curve.EaseOutExp:
                return Mathf.Pow(time, 0.333f);
            default:
                return time;
        }
    }

    /// <summary>
    /// Coroutine that lerps a transform's position from a start position to and end position
    /// </summary>
    /// <param name="objTransform"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpingPosition(Transform objTransform, Vector3 startPos, Vector3 endPos, float duration, Curve curve = Curve.Linear, AnimationCurve offsetCurve = null) {

        float timeElapsed = 0;
        float interpolation = 0;
        float offset = 0;

        Vector3 offsetVector = Vector3.zero;

        objTransform.position = startPos;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;

            if(offsetCurve != null) {
                offset = offsetCurve.Evaluate(interpolation);
                offsetVector = Vector3.one * offset;
            }

            objTransform.position = Vector3.Lerp(startPos, endPos, Evaluate(curve, interpolation)) + offsetVector;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        objTransform.position = endPos;

        yield return null;
    }

    /// <summary>
    /// Coroutine that lerps a transform's position in a specified animation curve
    /// </summary>
    /// <param name="objTransform"></param>
    /// <param name="curve"></param>
    /// <param name="worldSpace"></param>
    /// <returns></returns>
    public static IEnumerator LerpingPosition(Transform objTransform, AnimationCurve curve, bool worldSpace = false) {

        float startTime = curve[0].time;
        float curveTime = curve[curve.length - 1].time;
        float timeElapsed = 0;
        Vector3 originalPosition = objTransform.position;

        objTransform.position = worldSpace ? Vector3.zero : originalPosition + GetCurveVector(curve, 0f);

        while(timeElapsed < curveTime) {

            objTransform.position = worldSpace ? Vector3.zero : originalPosition + GetCurveVector(curve, (startTime + curveTime) * (timeElapsed / curveTime));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        objTransform.position = worldSpace ? Vector3.zero : originalPosition + GetCurveVector(curve, curveTime);

        yield return null;
    }

    static Vector3 GetCurveVector(AnimationCurve curve, float time) {

        return new Vector3(time, curve.Evaluate(time));
    }

    /// <summary>
    /// Coroutine that lerps a transform's position from a start position to and end position
    /// </summary>
    /// <param name="objTransform"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpingLocalPosition(Transform objTransform, Vector3 startPos, Vector3 endPos, float duration, Curve curve = Curve.Linear, AnimationCurve offsetCurve = null) {

        float timeElapsed = 0;
        float interpolation = 0;
        float offset = 0;

        Vector3 offsetVector = Vector3.zero;

        objTransform.localPosition = startPos;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;

            if(offsetCurve != null) {
                offset = offsetCurve.Evaluate(interpolation);
                offsetVector = Vector3.one * offset;
            }

            objTransform.localPosition = Vector3.Lerp(startPos, endPos, Evaluate(curve, interpolation)) + offsetVector;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        objTransform.localPosition = endPos;

        yield return null;
    }

	public static IEnumerator LerpingLocalPosition(RectTransform objTransform, Vector3 startPos, Vector3 endPos, float duration, Curve curve = Curve.Linear, AnimationCurve offsetCurve = null) {

		float timeElapsed = 0;
		float interpolation = 0;
		float offset = 0;

		Vector3 offsetVector = Vector3.zero;

		objTransform.localPosition = startPos;

		while(timeElapsed < duration) {
			interpolation = timeElapsed / duration;

			if(offsetCurve != null) {
				offset = offsetCurve.Evaluate(interpolation);
				offsetVector = Vector3.one * offset;
			}

			objTransform.localPosition = Vector3.Lerp(startPos, endPos, Evaluate(curve, interpolation)) + offsetVector;

			timeElapsed += Time.deltaTime;
			yield return null;
		}

		objTransform.localPosition = endPos;

		yield return null;
	}

    #endregion

    /// <summary>
    /// Coroutine that lerps a transform's local rotation from a start rotation to an end rotation
    /// </summary>
    /// <param name="objTransform"></param>
    /// <param name="startRotation"></param>
    /// <param name="endRotation"></param>
    /// <param name="duration"></param>
    /// <param name="curve"></param>
    /// <returns></returns>
    public static IEnumerator LerpingRotation(Transform objTransform, Vector3 startRotation, Vector3 endRotation, float duration, Curve curve = Curve.Linear) {
        Debug.Log("Lerping local rotation " + objTransform.name);

        float timeElapsed = 0f;
        float interpolation = 0f;

        objTransform.localRotation = Quaternion.Euler(startRotation);

        Vector3 currentRotation = Vector3.zero;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            currentRotation = Vector3.Lerp(startRotation, endRotation, Evaluate(curve, interpolation));
            objTransform.localRotation = Quaternion.Euler(currentRotation);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        objTransform.localRotation = Quaternion.Euler(endRotation);

        yield return null;
    }


    /// <summary>
    /// Coroutine that lerps a transform's scale from a start scale to an end scale
    /// </summary>
    /// <param name="objTransform">Object transform.</param>
    /// <param name="startScale">Start scale.</param>
    /// <param name="endScale">End scale.</param>
    /// <param name="duration">Duration.</param>
    public static IEnumerator LerpingScale(Transform objTransform, Vector3 startScale, Vector3 endScale, float duration, Curve curve = Curve.Linear) {

        float timeElapsed = 0f;
        float interpolation = 0f;

        objTransform.localScale = startScale;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            objTransform.localScale = Vector3.Lerp(startScale, endScale, Evaluate(curve, interpolation));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        objTransform.localScale = endScale;

        yield return null;
    }

    #region Color

    /// <summary>
    /// Coroutine that lerps a UI image's color from a start color to an end color
    /// </summary>
    /// <param name="image"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpingColor(Image image, Color startColor, Color endColor, float duration, Curve curve = Curve.Linear) {

        float timeElapsed = 0;
        float interpolation = 0f;

        image.color = startColor;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            image.color = Color.Lerp(startColor, endColor, Evaluate(curve, interpolation));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        image.color = endColor;

        yield return null;
    }
	public static IEnumerator LerpingColor(Camera c, Color startColor, Color endColor, float duration, Curve curve = Curve.Linear) {

		float timeElapsed = 0;
		float interpolation = 0f;

		c.backgroundColor = startColor;

		while(timeElapsed < duration) {
			interpolation = timeElapsed / duration;
			c.backgroundColor = Color.Lerp(startColor, endColor, Evaluate(curve, interpolation));

			timeElapsed += Time.deltaTime;
			yield return null;
		}

		c.backgroundColor = endColor;

		yield return null;
	}

    /// <summary>
    /// Coroutine that lerps a sprite renderer's color from a start color to an end color
    /// </summary>
    /// <param name="image"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpingColor(SpriteRenderer spriteRenderer, Color startColor, Color endColor, float duration, Curve curve = Curve.Linear) {

        float timeElapsed = 0f;
        float interpolation = 0f;

        spriteRenderer.color = startColor;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, Evaluate(curve, interpolation));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor;

        yield return null;
    }

    /// <summary>
    /// Coroutine that lerps a UI image's color from a start color to an end color
    /// </summary>
    /// <param name="image"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpingColor(LineRenderer line, Color startColor, Color endColor, float duration, Curve curve = Curve.Linear) {

        float timeElapsed = 0f;
        float interpolation = 0f;

        line.startColor = startColor;
        line.endColor = startColor;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            interpolation = Evaluate(curve, interpolation);

            line.startColor = Color.Lerp(startColor, endColor, interpolation);
            line.endColor = Color.Lerp(startColor, endColor, interpolation);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        line.startColor = endColor;
        line.endColor = endColor;

        yield return null;
    }

    /// <summary>
    /// Coroutine that lerps a text's color from a start color to an end color
    /// </summary>
    /// <param name="image"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator LerpingColor(Text text, Color startColor, Color endColor, float duration, Curve curve = Curve.Linear) {

        float timeElapsed = 0f;
        float interpolation = 0f;

        text.color = startColor;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            text.color = Color.Lerp(startColor, endColor, Evaluate(curve, interpolation));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        text.color = endColor;

        yield return null;
    }


    /// <summary>
    /// Coroutine that lerps a text's color from a start color to an end color
    /// </summary>
    /// <param name="image"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
   /* public static IEnumerator LerpingColor(TextMeshProUGUI text, Color startColor, Color endColor, float duration, Curve curve = Curve.Linear) {

        float timeElapsed = 0f;
        float interpolation = 0f;

        text.color = startColor;

        while(timeElapsed < duration) {
            interpolation = timeElapsed / duration;
            text.color = Color.Lerp(startColor, endColor, Evaluate(curve, interpolation));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        text.color = endColor;

        yield return null;
    }
*/
    #endregion

    #region Interaction

    public static bool OnConfirmDown() {

        return Input.GetKeyDown(KeyCode.E);
    }

    public static bool OnConfirm() {

        return Input.GetKey(KeyCode.E);
    }

    public static bool OnConfirmUp() {

        return Input.GetKeyUp(KeyCode.E);
    }

   /* public static bool CanTakeInput() {

        return !DialogueHandler.showingDialogue &&
            !GameManager.Instance.cutscene;
    }*/

    #endregion
}