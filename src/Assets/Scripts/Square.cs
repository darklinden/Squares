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

		Dbg.Assert(size < SquaresManager.manager.maxBlockSize, "Blocks must not be larger than " + SquaresManager.manager.maxBlockSize);

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
							squareMatrix [z, size - 1 - y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
							cube.name = z + "-" + (size - 1 - y) + "-" + x;
						}
					}
						break;
					case 1:
					{
						if (page2 [y] [x] == '1') {
							squareMatrix [z, size - 1 - y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
							cube.name = z + "-" + (size - 1 - y) + "-" + x;
						}
					}
						break;
					case 2:
					{
						if (page3 [y] [x] == '1') {
							squareMatrix [z, size - 1 - y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
							cube.name = z + "-" + (size - 1 - y) + "-" + x;
						}
					}
						break;
					case 3:
					{
						if (page4 [y] [x] == '1') {
							squareMatrix [z, size - 1 - y, x] = true;
							Transform cube = (Transform)Instantiate (SquaresManager.manager.cube, new Vector3 (x - childSize, childSize - y, z - childSize), Quaternion.identity);
							cube.renderer.material.color = cubeColor;
							cube.parent = transform;
							cube.name = z + "-" + (size - 1 - y) + "-" + x;
						}
					}
						break;
					}
				}
			}
		}

		transform.position = new Vector3((SquaresManager.manager.GetFieldWidth() - size) / 2.0f, 
		                                 SquaresManager.manager.GetFieldHeight() - childSize - size, 
		                                 -(SquaresManager.manager.GetFieldWidth() - size) / 2.0f);
		xPosition = transform.position.x;
		yPosition = transform.position.y;
		zPosition = transform.position.z;
		fallSpeed = SquaresManager.manager.blockNormalFallSpeed;
		
		if (SquaresManager.manager.CheckSquareOverlap (squareMatrix, zPosition, yPosition, xPosition)) {
			SquaresManager.manager.GameOver();
			return;
		}
		
		StartCoroutine(CheckInput());
		StartCoroutine(Fall());
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator Fall() {
		while (true) {

			yPosition--;

			if (SquaresManager.manager.CheckSquareOverlap(squareMatrix, zPosition, yPosition, xPosition)) {
				Debug.Log("Place Square: " + transform.position + " at: z: " + zPosition + " y:" + (yPosition + 1) + " x: " + xPosition);
				SquaresManager.manager.PlaceSquareCube(squareMatrix, zPosition, yPosition + 1, xPosition);
				Destroy(gameObject);
				break;
			}
			
			for (float i = yPosition + 1; i > yPosition;i -= Time.deltaTime * fallSpeed) {
				transform.position = new Vector3(transform.position.x, i, transform.position.z);
				yield return null;
			}
			
		}
	}
	
	IEnumerator MoveSquare(int distanceX, int distanceZ) {
		if (!SquaresManager.manager.CheckSquareOverlap(squareMatrix, zPosition + distanceZ, yPosition, xPosition + distanceX)) {
			transform.position = new Vector3(transform.position.x + distanceX, 
			                                 transform.position.y,
			                                 transform.position.z + distanceZ);
			xPosition += distanceX;
			zPosition += distanceZ;
			yield return new WaitForSeconds(.1f);
		}
	}

	void DebugSquare(Vector3 pos) {
#if false
		for (int z = 0; z < size; z++) {
			for (int y = 0; y < size; y++) {
				for (int x = 0; x < size; x++) {
					if (squareMatrix[z, y, x]) {
						Dbg.Box (new Vector3 (pos.x + x - childSize - .5f, pos.y + childSize - y - .5f, pos.z + z - childSize - .5f));
					}
				}
			}
		}
#endif
	}
	
	void RotateSquare(SquareRotate rotate) {

		Debug.Log ("Rotation: " + transform.rotation);

		var tempMatrix = new bool[size, size, size];

		switch (rotate) {
		case SquareRotate.SquareRotateX:
		{
			for (int x = 0; x < size; x++) {
				for (int z = 0; z < size; z++) {
					for (int y = 0; y < size; y++) {
						tempMatrix[z, y, x] = squareMatrix[(size - 1) - y, z, x];
					}
				}
			}

			if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, zPosition, yPosition, xPosition)) {
				transform.Rotate(90, 0, 0, Space.World);
				System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			}
			else {
				float tmpzPos = zPosition;
				float tmpxPos = xPosition;

				if (tmpxPos - (size * 0.5f) < SquaresManager.manager.leftWall.position.x) { tmpxPos = SquaresManager.manager.leftWall.position.x + (size * 0.5f); }
				if (tmpxPos + (size * 0.5f) > SquaresManager.manager.rightWall.position.x) { tmpxPos = SquaresManager.manager.rightWall.position.x - (size * 0.5f); }
				if (tmpzPos - (size * 0.5f) < SquaresManager.manager.frontWall.position.z) { tmpzPos = SquaresManager.manager.frontWall.position.z + (size * 0.5f); }
				if (tmpzPos + (size * 0.5f) > SquaresManager.manager.backWall.position.z) { tmpzPos = SquaresManager.manager.backWall.position.z - (size * 0.5f); }

				if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, tmpzPos, yPosition, tmpxPos)) {
					transform.position = new Vector3(tmpxPos, 
					                                 transform.position.y,
					                                 tmpzPos);
					xPosition = tmpxPos;
					zPosition = tmpzPos;
					transform.Rotate(90, 0, 0, Space.World);
					System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
				}
			}

			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		case SquareRotate.SquareRotateY:
		{
			for (int y = 0; y < size; y++) {
				for (int z = 0; z < size; z++) {
					for (int x = 0; x < size; x++) {
						tempMatrix[z, y, x] = squareMatrix[x, y, (size - 1) - z];
					}
				}
			}

			if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, zPosition, yPosition, xPosition)) {
				transform.Rotate(0, 90, 0, Space.World);
				System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			}
			else {
				float tmpzPos = zPosition;
				float tmpxPos = xPosition;
				
				if (tmpxPos - (size * 0.5f) < SquaresManager.manager.leftWall.position.x) { tmpxPos = SquaresManager.manager.leftWall.position.x + (size * 0.5f); }
				if (tmpxPos + (size * 0.5f) > SquaresManager.manager.rightWall.position.x) { tmpxPos = SquaresManager.manager.rightWall.position.x - (size * 0.5f); }
				if (tmpzPos - (size * 0.5f) < SquaresManager.manager.frontWall.position.z) { tmpzPos = SquaresManager.manager.frontWall.position.z + (size * 0.5f); }
				if (tmpzPos + (size * 0.5f) > SquaresManager.manager.backWall.position.z) { tmpzPos = SquaresManager.manager.backWall.position.z - (size * 0.5f); }
				
				if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, tmpzPos, yPosition, tmpxPos)) {
					transform.position = new Vector3(tmpxPos, 
					                                 transform.position.y,
					                                 tmpzPos);
					xPosition = tmpxPos;
					zPosition = tmpzPos;
					transform.Rotate(0, 90, 0, Space.World);
					System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
				}
			}

			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		case SquareRotate.SquareRotateZ:
		{
			for (int z = 0; z < size; z++) {
				for (int y = 0; y < size; y++) {
					for (int x = 0; x < size; x++) {
						tempMatrix[z, y, x] = squareMatrix[z, (size - 1) - x, y];
					}
				}
			}

			if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, zPosition, yPosition, xPosition)) {
				transform.Rotate(0, 0, 90, Space.World);
				System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			}
			else {
				float tmpzPos = zPosition;
				float tmpxPos = xPosition;
				
				if (tmpxPos - (size * 0.5f) < SquaresManager.manager.leftWall.position.x) { tmpxPos = SquaresManager.manager.leftWall.position.x + (size * 0.5f); }
				if (tmpxPos + (size * 0.5f) > SquaresManager.manager.rightWall.position.x) { tmpxPos = SquaresManager.manager.rightWall.position.x - (size * 0.5f); }
				if (tmpzPos - (size * 0.5f) < SquaresManager.manager.frontWall.position.z) { tmpzPos = SquaresManager.manager.frontWall.position.z + (size * 0.5f); }
				if (tmpzPos + (size * 0.5f) > SquaresManager.manager.backWall.position.z) { tmpzPos = SquaresManager.manager.backWall.position.z - (size * 0.5f); }
				
				if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, tmpzPos, yPosition, tmpxPos)) {
					transform.position = new Vector3(tmpxPos, 
					                                 transform.position.y,
					                                 tmpzPos);
					xPosition = tmpxPos;
					zPosition = tmpzPos;
					transform.Rotate(0, 0, 90, Space.World);
					System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
				}
			}

			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		case SquareRotate.SquareRotate4DX:
		{
			for (int x = 0; x < size; x++) {
				for (int z = 0; z < size; z++) {
					for (int y = 0; y < size; y++) {
						tempMatrix[z, y, x] = squareMatrix[z, y, (size - 1) - x];
					}
				}
			}

			if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, zPosition, yPosition, xPosition)) {
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
			}
			else {
				float tmpzPos = zPosition;
				float tmpxPos = xPosition;
				
				if (tmpxPos - (size * 0.5f) < SquaresManager.manager.leftWall.position.x) { tmpxPos = SquaresManager.manager.leftWall.position.x + (size * 0.5f); }
				if (tmpxPos + (size * 0.5f) > SquaresManager.manager.rightWall.position.x) { tmpxPos = SquaresManager.manager.rightWall.position.x - (size * 0.5f); }
				if (tmpzPos - (size * 0.5f) < SquaresManager.manager.frontWall.position.z) { tmpzPos = SquaresManager.manager.frontWall.position.z + (size * 0.5f); }
				if (tmpzPos + (size * 0.5f) > SquaresManager.manager.backWall.position.z) { tmpzPos = SquaresManager.manager.backWall.position.z - (size * 0.5f); }
				
				if (!SquaresManager.manager.CheckSquareOverlap(tempMatrix, tmpzPos, yPosition, tmpxPos)) {
					transform.position = new Vector3(tmpxPos, 
					                                 transform.position.y,
					                                 tmpzPos);
					xPosition = tmpxPos;
					zPosition = tmpzPos;
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
					System.Array.Copy(tempMatrix, squareMatrix, size * size * size);
				}
			}

			DebugSquare(new Vector3(10, 28, 0));
		}
			break;
		}
	}
	
	IEnumerator CheckInput() {
		
		while(true) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
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

			if (Input.GetKeyDown(KeyCode.DownArrow)) {
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

			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
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

			if (Input.GetKeyDown(KeyCode.RightArrow)) {
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

//			if (Input.GetKeyDown(KeyCode.W)) {
//				RotateSquare(SquareRotate.SquareRotate4DX);
//			}

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
