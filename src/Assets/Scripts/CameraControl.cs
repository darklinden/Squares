using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private const string animationKey = "CameraMove";

	public enum CameraVerticalState {
		Top,
		Relative,
		Horizontal
	}

	public enum CameraHorizontalState {
		Left,
		Right,
		Front,
		Back
	}

	public float AnimateDuration = 1.0f;
	public float CameraDepth = 28.0f;
	public float RelativeDepth = 15.0f;
	public CameraVerticalState verticalState = CameraVerticalState.Top;
	public CameraHorizontalState horizontalState = CameraHorizontalState.Front; 
	public GameObject relativeObj;

	public static CameraControl controller;
	
	// Use this for initialization
	void Start () {

		if (controller == null) {
			controller = this;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (animation.GetClip (animationKey)) {
			if (!animation.IsPlaying (animationKey)) {
				animation.RemoveClip (animationKey);

				float fx = transform.localRotation.eulerAngles.x;
				if (fx >= 360)
						fx -= 360;
				if (fx <= -360)
						fx += 360;

				float fy = transform.localRotation.eulerAngles.y;
				if (fy >= 360)
						fy -= 360;
				if (fy <= -360)
						fy += 360;

				float fz = transform.localRotation.eulerAngles.z;
				if (fz >= 360)
						fz -= 360;
				if (fz <= -360)
						fz += 360;

				transform.localRotation = Quaternion.Euler (new Vector3 (fx, fy, fz));
			}
		} 
		else {
			//input control
			if (Input.GetKey (KeyCode.Z)) {
				
				switch (verticalState) {
				case CameraVerticalState.Top:
				{
					if (relativeObj) {
						verticalState = CameraVerticalState.Relative;
					}
					else {
						verticalState = CameraVerticalState.Horizontal;
					}
				}
					break;
				case CameraVerticalState.Horizontal:
					verticalState = CameraVerticalState.Top;
					break;
				case CameraVerticalState.Relative:
					verticalState = CameraVerticalState.Horizontal;
					break;
				}
				
				animateToCamera(verticalState, horizontalState);
			}
			
			if (Input.GetKey (KeyCode.X)) {
				
				switch (horizontalState) {
				case CameraHorizontalState.Front:
					horizontalState = CameraHorizontalState.Left;
					break;
				case CameraHorizontalState.Left:
					horizontalState = CameraHorizontalState.Back;
					break;
				case CameraHorizontalState.Back:
					horizontalState = CameraHorizontalState.Right;
					break;
				case CameraHorizontalState.Right:
					horizontalState = CameraHorizontalState.Front;
					break;
				}
				
				animateToCamera(verticalState, horizontalState);
			}
			
//			if (Input.GetKey (KeyCode.C)) {
//				verticalState = CameraVerticalState.Top;
//				horizontalState = CameraHorizontalState.Front;
//				
//				animateToCamera(verticalState, horizontalState);
//			}
		}
	}

	void animateToCamera (CameraVerticalState _vs, CameraHorizontalState _hs) {

		if (animation.isPlaying) {
			animation.Stop();
		}
		
		if (animation.GetClip (animationKey)) {
			animation.RemoveClip(animationKey);
		}

		Vector3 np = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		Quaternion nr = new Quaternion (transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);

		switch (_vs) {
		case CameraVerticalState.Top:
		{
			np = new Vector3(0, 35, 0);

			switch (_hs) {
			case CameraHorizontalState.Left:
			{
				float fa = 90;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(90, fa, 0));
			}
				break;
			case CameraHorizontalState.Right:
			{
				float fa = 270;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(90, fa, 0));
			}
				break;
			case CameraHorizontalState.Front:
			{
				float fa = 0;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(90, fa, 0));
			}
				break;
			case CameraHorizontalState.Back:
			{
				float fa = 180;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(90, fa, 0));
			}
				break;
			}
		}
			break;

		case CameraVerticalState.Relative:
		{
			switch (_hs) {
			case CameraHorizontalState.Left:
			{
				np = new Vector3(relativeObj.transform.localPosition.x - RelativeDepth, relativeObj.transform.localPosition.y, relativeObj.transform.localPosition.z);

				float fa = 90;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;

				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			case CameraHorizontalState.Right:
			{
				np = new Vector3(relativeObj.transform.localPosition.x + RelativeDepth, relativeObj.transform.localPosition.y, relativeObj.transform.localPosition.z);

				float fa = 270;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;

				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			case CameraHorizontalState.Front:
			{
				np = new Vector3(relativeObj.transform.localPosition.x, relativeObj.transform.localPosition.y, relativeObj.transform.localPosition.z - RelativeDepth);

				float fa = 0;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;

				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			case CameraHorizontalState.Back:
			{
				np = new Vector3(relativeObj.transform.localPosition.x, relativeObj.transform.localPosition.y, relativeObj.transform.localPosition.z + RelativeDepth);

				float fa = 180;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;

				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			}
		}
			break;
		
		case CameraVerticalState.Horizontal:
		{
			switch (_hs) {
			case CameraHorizontalState.Left:
			{
				np = new Vector3(-CameraDepth, 15, 0);
				
				float fa = 90;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			case CameraHorizontalState.Right:
			{
				np = new Vector3(CameraDepth, 15, 0);
				
				float fa = 270;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			case CameraHorizontalState.Front:
			{
				np = new Vector3(0, 15, -CameraDepth);
				
				float fa = 0;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			case CameraHorizontalState.Back:
			{
				np = new Vector3(0, 15, CameraDepth);
				
				float fa = 180;
				if (fa - transform.localRotation.eulerAngles.y >= 180) fa -= 360;
				if (fa - transform.localRotation.eulerAngles.y <= -180) fa += 360;
				
				nr = Quaternion.Euler(new Vector3(0, fa, 0));
			}
				break;
			}
		}
			break;
		}

		AnimationClip clip = new AnimationClip();
		clip.name = animationKey;
		clip.SetCurve("", typeof(Transform), "localPosition.x", AnimationCurve.Linear(0, transform.localPosition.x, AnimateDuration, np.x));
		clip.SetCurve("", typeof(Transform), "localPosition.y", AnimationCurve.Linear(0, transform.localPosition.y, AnimateDuration, np.y));
		clip.SetCurve("", typeof(Transform), "localPosition.z", AnimationCurve.Linear(0, transform.localPosition.z, AnimateDuration, np.z));
		
		clip.SetCurve("", typeof(Transform), "localRotation.x", AnimationCurve.Linear(0, transform.localRotation.x, AnimateDuration, nr.x));
		clip.SetCurve("", typeof(Transform), "localRotation.y", AnimationCurve.Linear(0, transform.localRotation.y, AnimateDuration, nr.y));
		clip.SetCurve("", typeof(Transform), "localRotation.z", AnimationCurve.Linear(0, transform.localRotation.z, AnimateDuration, nr.z));
		clip.SetCurve("", typeof(Transform), "localRotation.w", AnimationCurve.Linear(0, transform.localRotation.w, AnimateDuration, nr.w));
		animation.AddClip(clip, animationKey);
		animation.Play(animationKey);
	}
}
