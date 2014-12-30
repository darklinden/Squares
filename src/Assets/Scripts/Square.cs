using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

	public int size = 4;
	public string[] page1 = {"0000", "1111", "0000", "0000"};
	public string[] page2 = {"0000", "0000", "0000", "0000"};
	public string[] page3 = {"0000", "0000", "0000", "0000"};
	public string[] page4 = {"0000", "0000", "0000", "0000"};
	private bool[,,] squareMatrix;

	private float halfSize;
	private float halfSizeFloat;
	private float childSize;
	private int xPosition;
	private int yPosition;
	private int zPosition;
	private float fallSpeed;
	private bool drop = false;

	enum SquareRotate : int {
		SquareRotateX,
		SquareRotateY,
		SquareRotateZ,
		SquareRotate4DX
	}
	
	// Use this for initialization
	void Start () {
	
		Dbg.Assert (size >= 2, "Square must have at least two pages");

		Dbg.Assert(size > SquaresManager.manager.maxBlockSize, "Blocks must not be larger than " + SquaresManager.manager.maxBlockSize);
		
		halfSize = (size + 1) * .5f;
		childSize = (size - 1) * .5f;
		halfSizeFloat = size * .5f;
		
		squareMatrix = new bool[size, size, size];
		for (int z = 0; z < size; z++) {
			for (int y = 0; y < size; y++) {
				for (int x = 0; x < size; x++) {
					switch (z) {
					case 0:
					{
						if (page1 [y] [x] == '1') {
							
							squareMatrix [z, y, x] = true;
							var cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.parent = transform;
							
						}
					}
						break;
					case 1:
					{
						if (page2 [y] [x] == '1') {
							
							squareMatrix [z, y, x] = true;
							var cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.parent = transform;
							
						}
					}
						break;
					case 2:
					{
						if (page3 [y] [x] == '1') {
							
							squareMatrix [z, y, x] = true;
							var cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.parent = transform;
							
						}
					}
						break;
					case 3:
					{
						if (page4 [y] [x] == '1') {
							
							squareMatrix [z, y, x] = true;
							var cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.parent = transform;
							
						}
					}
						break;
					}
				}
			}
		}

		yPosition = SquaresManager.manager.GetFieldHeight() - 1;
		transform.position = new Vector3((SquaresManager.manager.GetFieldWidth() - size) / 2.0f, 
		                                 yPosition - childSize, 
		                                 -(SquaresManager.manager.GetFieldWidth() - size) / 2.0f);
		xPosition = (int)(transform.position.x - childSize);
		fallSpeed = SquaresManager.manager.blockNormalFallSpeed;
		
//		if (SquaresManager.manager.CheckBlock(squareMatrix, 0, xPosition, yPosition)){
//			SquaresManager.manager.GameOver();
//			return;
//		}
		
		StartCoroutine(CheckInput());
		StartCoroutine(Delay((1 / SquaresManager.manager.blockNormalFallSpeed) * 2));
		StartCoroutine(Fall());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator Delay(float time){
		var t = 0f;
		while (t <= time && !drop){
			t += Time.deltaTime;
			yield return null;
		}
	}
	
	IEnumerator Fall(){
		while(true){

			if (yPosition > 0) {
				yPosition--;
			}
			else {
				yield break;
			}

//			if (SquaresManager.manager.CheckBlock(squareMatrix, 0, xPosition, yPosition)){
//				SquaresManager.manager.SetBlock(squareMatrix, 0, xPosition, yPosition + 1);
//				Destroy(gameObject);
//				break;
//			}
			
			for (float i = yPosition + 1;i > yPosition;i -= Time.deltaTime * fallSpeed){
				transform.position = new Vector3(transform.position.x, i - childSize, transform.position.z);
				/*foreach(Transform child in transform){
					print(child.transform.position);
				}*/
				yield return null;
			}
			
		}
	}
	
	IEnumerator MoveSquare(int distanceX, int distanceZ){
		
//		if (!SquaresManager.manager.CheckBlock(squareMatrix, xPosition + distance, yPosition)){
			transform.position = new Vector3(transform.position.x + distanceX, 
		                                 transform.position.y,
		                                 transform.position.z + distanceZ);
			xPosition += distanceX;
		zPosition += distanceZ;
			yield return new WaitForSeconds(.1f);
//		}
	}
	
	void RotateSquare(SquareRotate rotate){

		switch (rotate) {
		case SquareRotate.SquareRotateX:
		{
			transform.Rotate(90, 0, 0);
		}
			break;
		case SquareRotate.SquareRotateY:
		{
			transform.Rotate(0, 90, 0);
		}
			break;
		case SquareRotate.SquareRotateZ:
		{
			transform.Rotate(0, 0, 90);
		}
			break;
		case SquareRotate.SquareRotate4DX:
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
			break;
		}
		
		var tempMatrix = new bool[size, size, size];
		/*
		for (int y = 0;y < size;y++){
			for (int x = 0;x < size;x++){
				tempMatrix[y, x] = blockMatrix[x, y];
				print(tempMatrix[y, x] + " ");
			}
		    print("\n");
		}*/
		
//	    for (int y = 0; y < size; y++) {
//		     for (int x = 0; x < size; x++) {
//		          tempMatrix[0, y, x] = squareMatrix[0, x, (size-1)-y];
//	         }
//		}
//		
//		if (!SquaresManager.manager.CheckBlock(tempMatrix, xPosition, yPosition)){
//			System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
//		}
	}
	
	IEnumerator CheckInput(){
		
		while(true){
			if (Input.GetKeyDown(KeyCode.UpArrow)){
				switch (CameraControl.controller.horizontalState) {
				case CameraControl.CameraHorizontalState.Left:
					yield return StartCoroutine(MoveSquare(1, 0));
					break;
				case CameraControl.CameraHorizontalState.Right:
					yield return StartCoroutine(MoveSquare(-1, 0));
					break;
				case CameraControl.CameraHorizontalState.Front:
					yield return StartCoroutine(MoveSquare(0, 1));
					break;
				case CameraControl.CameraHorizontalState.Back:
					yield return StartCoroutine(MoveSquare(0, -1));
					break;
				}

			}

			if (Input.GetKeyDown(KeyCode.DownArrow)){
				switch (CameraControl.controller.horizontalState) {
				case CameraControl.CameraHorizontalState.Left:
					yield return StartCoroutine(MoveSquare(-1, 0));
					break;
				case CameraControl.CameraHorizontalState.Right:
					yield return StartCoroutine(MoveSquare(1, 0));
					break;
				case CameraControl.CameraHorizontalState.Front:
					yield return StartCoroutine(MoveSquare(0, -1));
					break;
				case CameraControl.CameraHorizontalState.Back:
					yield return StartCoroutine(MoveSquare(0, 1));
					break;
				}
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow)){
				switch (CameraControl.controller.horizontalState) {
				case CameraControl.CameraHorizontalState.Left:
					yield return StartCoroutine(MoveSquare(0, 1));
					break;
				case CameraControl.CameraHorizontalState.Right:
					yield return StartCoroutine(MoveSquare(0, -1));
					break;
				case CameraControl.CameraHorizontalState.Front:
					yield return StartCoroutine(MoveSquare(-1, 0));
					break;
				case CameraControl.CameraHorizontalState.Back:
					yield return StartCoroutine(MoveSquare(1, 0));
					break;
				}
			}

			if (Input.GetKeyDown(KeyCode.RightArrow)){
				switch (CameraControl.controller.horizontalState) {
				case CameraControl.CameraHorizontalState.Left:
					yield return StartCoroutine(MoveSquare(0, -1));
					break;
				case CameraControl.CameraHorizontalState.Right:
					yield return StartCoroutine(MoveSquare(0, 1));
					break;
				case CameraControl.CameraHorizontalState.Front:
					yield return StartCoroutine(MoveSquare(1, 0));
					break;
				case CameraControl.CameraHorizontalState.Back:
					yield return StartCoroutine(MoveSquare(-1, 0));
					break;
				}
			}

			if (Input.GetKeyDown(KeyCode.W)){
				RotateSquare(SquareRotate.SquareRotate4DX);
			}

			if (Input.GetKeyDown(KeyCode.S)){
				RotateSquare(SquareRotate.SquareRotateY);
			}

			if (Input.GetKeyDown(KeyCode.A)){
				RotateSquare(SquareRotate.SquareRotateX);
			}

			if (Input.GetKeyDown(KeyCode.D)){
				RotateSquare(SquareRotate.SquareRotateZ);
			}

//			var input = Input.GetAxisRaw("Horizontal");
//			if (input < 0){
//				yield return StartCoroutine(MoveSquare(-1));
//			}
//			
//			if (input > 0){
//				yield return StartCoroutine(MoveSquare(1));
//			}
//			
//			if (Input.GetKeyDown(KeyCode.UpArrow)){
//				RotateBlock();
//			}
//			
//			if (Input.GetKeyDown(KeyCode.DownArrow)){
//				fallSpeed = SquaresManager.manager.blockDropSpeed;
//				drop = true;
//				//break;
//			}
			
//			if (Input.GetKeyUp("space")){
//				RotateBlock();
//				fallSpeed = SquaresManager.manager.blockNormalFallSpeed;
//				drop = false;
				//break;
//			}
			
			yield return null;
		}
		
	}
	
}
