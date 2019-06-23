using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	EASY,
	NORMAL,
	HARD,
	EXPERT
}

public class GameManager : MonoBehaviour {

	[SerializeField] GameObject FunctionSelections_UI;
	[SerializeField] GameObject DifficultySelections_UI;
	[SerializeField] GameObject MainGame;
	[SerializeField] GroupOpacityController EffectsController;
	[SerializeField] MathManager mathmanager;

	[SerializeField]
	private Difficulty dif;

	[SerializeField]
	private Math_function mode;

	// Use this for initialization
	void Start () {
		MainGame.SetActive (false);
		StartCoroutine(EffectsController.FadeImages(FunctionSelections_UI,1));
		DifficultySelections_UI.SetActive (false);
	}


	public void OperatorIs(int OperatorIndex)
	{
		DifficultySelections_UI.SetActive (true);
		if (OperatorIndex == 0) {
			mode = Math_function.ADDITION;
		} else if (OperatorIndex == 1) {
			mode = Math_function.SUBTRACTION;
		} else if (OperatorIndex == 2) {
			mode = Math_function.MULTIPLICATION;
		} else if (OperatorIndex == 3) {
			mode = Math_function.DIVISION;
		}

		Debug.Log ("Click operatorchoice");
		StartCoroutine(EffectsController.FadeImages(FunctionSelections_UI,0));
		StartCoroutine(EffectsController.FadeImages(DifficultySelections_UI,1));
	
	}

	public void DifficultyIs(int DifficultyIndex)
	{
		Debug.Log ("Click difficultychoice");
		if (DifficultyIndex == 0) {
			dif = Difficulty.EASY;
		}
		else if (DifficultyIndex == 1) {
			dif = Difficulty.NORMAL;
		} 
		else if (DifficultyIndex == 2) {
			dif = Difficulty.HARD;
		}
		else if (DifficultyIndex == 3) {
			dif = Difficulty.EXPERT;
		}
		StartCoroutine (InitializeGame ());
	}

	IEnumerator InitializeGame()
	{
		mathmanager.Initialize (mode, this.dif);
		yield return EffectsController.FadeImages (DifficultySelections_UI, 0);
		yield return Utils.LerpingColor (this.GetComponent<Camera> (), this.GetComponent<Camera> ().backgroundColor, Color.blue, 0.2f);
		yield return EffectsController.FadeImages (MainGame, 1);
			
		FunctionSelections_UI.SetActive (false);
		DifficultySelections_UI.SetActive (false);
		MainGame.SetActive(true);
	}
	public void ExitApp()
	{
	Application.Quit();
	}
			
}
