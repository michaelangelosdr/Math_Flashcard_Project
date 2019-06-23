
//Im going to add comments here to let you understand whats going on with each line of code.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//^ Common namespaces used by unity C# scripts.

using UnityEngine.UI; //this adds the capability of calling in Canvas related components such as text/sprites/etc.,

public class MathManager : MonoBehaviour {


	[SerializeField] //This makes whatever variable appear in unity editor so you can assign it thru there.
	Text Upper_Number_Text; //<- making a text variable named Upper_Number

	[SerializeField]
	Text Lower_Number_Text; //<- Making a text variable name lower number

	[SerializeField]
	Text Sign; //<- Making a text variable named "sign" 

	[SerializeField]
	Text Answer;

	private int a;
	private int b;
	private int CorrectAnswer;
	public int Player_answer;
	//bool OnesHasvalue = false;

	private Math_function _func;

	// Use this for initialization
	public void Initialize (Math_function function, Difficulty dif) {
		int floor= 0, ceiling = 1;

		if (dif == Difficulty.EASY) {
			floor = 0;
			ceiling = 10;
		} else if (dif == Difficulty.NORMAL) {
			floor = 5;
			ceiling = 20;
		} else if (dif == Difficulty.HARD) {
			floor = 20;
			ceiling = 60;
		} else if (dif == Difficulty.EXPERT) {
			floor = 10;
			ceiling = 99;
		}

		a = AssignNumberToText (Upper_Number_Text,floor,ceiling);
		b = AssignNumberToText (Lower_Number_Text,floor,ceiling);
		Player_answer = 0;
		ApplySign (Math_function.ADDITION);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.A)) {
			Calculate ();
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Initialize (Math_function.ADDITION, Difficulty.HARD);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			Initialize (Math_function.ADDITION, Difficulty.EASY);
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			Initialize (Math_function.ADDITION, Difficulty.NORMAL);
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			Initialize (Math_function.ADDITION, Difficulty.EXPERT);
		}

	}



	public void Calculate()
	{
		if (_func == Math_function.ADDITION) {
			CorrectAnswer = a + b;
		} else if (_func == Math_function.SUBTRACTION) {
			CorrectAnswer = a - b;
		} else if (_func == Math_function.MULTIPLICATION) {
			CorrectAnswer = a * b;
		} else if (_func == Math_function.DIVISION) {
			CorrectAnswer = a / b;
		} else {
			Debug.Log ("Invalid function");
		}

		Debug.Log (CorrectAnswer);

		if (Player_answer == CorrectAnswer) {
			Debug.Log ("CORRECT");
		}
	}

	public void InputHandle(int val)
	{
		if (Player_answer == 0) {
			Player_answer += val;
		} else if (Player_answer > 0 && Player_answer <10) {
			Player_answer *= 10 + val;
		}
		

		Answer.text = Player_answer.ToString ();
	}


	public void ApplySign (Math_function _function)
	{
		if (_function == Math_function.ADDITION) {
			Sign.text = "+";
		} else if (_function == Math_function.SUBTRACTION) {
			Sign.text = "-";
		} else if (_function == Math_function.MULTIPLICATION) {
			Sign.text = "x";
		} else if (_function == Math_function.DIVISION) {
			Sign.text = "/";
		} else {
			Debug.Log ("Invalid function");
		}
		_func = _function;
	}

	public int AssignNumberToText(Text _text,int rangefloor, int rangeceiling,int val = 0)
	{
		if (val == 0) {
			val = Random.Range (rangefloor, rangeceiling);
		}
		_text.text = val.ToString ();
		return val;
	}
}




public enum Math_function
{
	ADDITION,
	SUBTRACTION,
	DIVISION,
	MULTIPLICATION
}
