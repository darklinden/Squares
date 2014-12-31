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
	private float xPosition;
	private float yPosition;
	private float zPosition;
	private float fallSpeed;
	private Color cubeColor;

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

		Color [] colorpool = {Color.black, Color.blue, Color.cyan, Color.gray,
			Color.green, Color.grey, Color.magenta, Color.red, Color.white, Color.yellow };
		cubeColor = colorpool [Random.Range (0, colorpool.Length)];

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
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
						}
					}
						break;
					case 1:
					{
						if (page2 [y] [x] == '1') {
							squareMatrix [z, y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
						}
					}
						break;
					case 2:
					{
						if (page3 [y] [x] == '1') {
							squareMatrix [z, y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
						}
					}
						break;
					case 3:
					{
						if (page4 [y] [x] == '1') {
							squareMatrix [z, y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
						}
					}
						break;
					}
				}
			}
		}

		transform.position = new Vector3((SquaresManager.manager.GetFieldWidth() - size) / 2.0f, 
		                                 SquaresManager.manager.GetFieldHeight() - childSize, 
		                                 -(SquaresManager.manager.GetFieldWidth() - size) / 2.0f);
		xPosition = transform.position.x;
		yPosition = transform.position.y;
		zPosition = transform.position.z;
		fallSpeed = SquaresManager.manager.blockNormalFallSpeed;
		
		if (SquaresManager.manager.CheckTopOverlap (squareMatrix, zPosition, yPosition, xPosition)){
			SquaresManager.manager.GameOver();
			return;
		}
		
		StartCoroutine(CheckInput());
		StartCoroutine(Fall());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Fall(){
		while (true){

			Debug.Log("yPosition:"+ yPosition);
			yPosition--;

#warning
			if (SquaresManager.manager.CheckTopOverlap(squareMatrix, zPosition, yPosition, xPosition)){
//				SquaresManager.manager.SetBlock(squareMatrix, 0, xPosition, yPosition + 1);
				Destroy(gameObject);
				break;
			}
			
			for (float i = yPosition + 1;i > yPosition;i -= Time.deltaTime * fallSpeed){

//				Debug.Log("i: "+ i + " fallSpeed: " + fallSpeed);
				transform.position = new Vector3(transform.position.x, i - childSize, transform.position.z);
				/*foreach(Transform child in transform){
					print(child.transform.position);
				}*/
				yield return null;
			}
			
		}
	}
	
	IEnumerator MoveSquare(int distanceX, int distanceZ){
		if (!SquaresManager.manager.CheckTopOverlap(squareMatrix, zPosition + distanceZ, yPosition, xPosition + distanceX)){
			transform.position = new Vector3(transform.position.x + distanceX, 
			                                 transform.position.y,
			                                 transform.position.z + distanceZ);
			xPosition += distanceX;
			zPosition += distanceZ;
			yield return new WaitForSeconds(.1f);
		}
	}

	void DebugSquare(Vector3 pos) {
		return;
//		for (int z = 0; z < size; z++) {
//			for (int y = 0; y < size; y++) {
//				for (int x = 0; x < size; x++) {
//					GameObject t = GameObject.Find ("z-" + z + "-y-" + y + "-x-" + x);
//
//					if (t) {
//						Destroy (t);
//					}
//
//					if (squareMatrix[z, y, x]) {
//						squareMatrix [z, y, x] = true;
//						Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (pos.x + x - childSize, pos.y + childSize - y, pos.z + z - childSize), Quaternion.identity);
//						cube.renderer.material.color = cubeColor;
//						cube.name = "z-" + z + "-y-" + y + "-x-" + x;
//					}
//				}
//			}
//		}
	}
	
	void RotateSquare(SquareRotate rotate){

		var tempMatrix = new bool[size, size, size];

		switch (rotate) {
		case SquareRotate.SquareRotateX:
		{
			transform.Rotate(90, 0, 0);

			for (int x = 0; x < size; x++) {
				for (int z = 0; z < size; z++) {
					for (int y = 0; y < size; y++) {
						tempMatrix[z, y, x] = squareMatrix[y, (size - 1) - z, x];
					}
				}
			}

			System.Array.Copy(tempMatrix, squareMatrix, size * size * size);

			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		case SquareRotate.SquareRotateY:
		{
			transform.Rotate(0, 90, 0);
			
			for (int y = 0; y < size; y++) {
				for (int z = 0; z < size; z++) {
					for (int x = 0; x < size; x++) {
						tempMatrix[z, y, x] = squareMatrix[x, y, (size - 1) - z];
					}
				}
			}
			
			System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			
			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		case SquareRotate.SquareRotateZ:
		{
			transform.Rotate(0, 0, 90);
			
			for (int z = 0; z < size; z++) {
				for (int y = 0; y < size; y++) {
					for (int x = 0; x < size; x++) {
						tempMatrix[z, y, x] = squareMatrix[z, x, (size - 1) - y];
					}
				}
			}
			
			System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			
			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		case SquareRotate.SquareRotate4DX:
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			for (int x = 0; x < size; x++) {
				for (int z = 0; z < size; z++) {
					for (int y = 0; y < size; y++) {
						tempMatrix[z, y, x] = squareMatrix[z, y, (size - 1) - x];
					}
				}
			}

			System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			
			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		}

//	    for (int y = 0; y < size; y++) {
//		     for (int x = 0; x < size; x++) {
//		          tempMatrix[0, y, x] = squareMatrix[0, x, (size-1)-y];
//	         }
//		}
//		
#warning
		if (!SquaresManager.manager.CheckTopOverlap(tempMatrix, zPosition, yPosition, xPosition)){
			System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
		}
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

			if (Input.GetKeyUp(KeyCode.Space)) {
				if (fallSpeed == SquaresManager.manager.blockNormalFallSpeed) {
					fallSpeed = SquaresManager.manager.blockDropSpeed;
				}
				else {
					fallSpeed = SquaresManager.manager.blockNormalFallSpeed;
				}
			}

			if (Input.GetKeyDown(KeyCode.W)) {
				RotateSquare(SquareRotate.SquareRotate4DX);
			}

			if (Input.GetKeyDown(KeyCode.S)) {
				RotateSquare(SquareRotate.SquareRotateY);
			}

			if (Input.GetKeyDown(KeyCode.A)) {
				RotateSquare(SquareRotate.SquareRotateX);
			}

			if (Input.GetKeyDown(KeyCode.D)) {
				RotateSquare(SquareRotate.SquareRotateZ);
			}

			yield return null;
		}	
	}
}
