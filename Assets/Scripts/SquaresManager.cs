using UnityEngine;
using System.Collections;  
using System.Collections.Generic;  
using System.IO;  
using System.Text; 
using UnityEngine.UI;

public class SquaresManager : MonoBehaviour {
	
	public GameObject[] blocks;
	public Transform cube;
	public Transform ground;
	public Transform leftWall;
	public Transform rightWall;
	public Transform frontWall;
	public Transform backWall;
	public int _fieldWidth = 10;
	public int _fieldHeight = 13;
	public float cubeSizeWidth = 1.0f;
	public float blockNormalFallSpeed = 2f;
	public float blockDropSpeed = 30f;
	public Texture2D cubeTexture;

	public Text TxtScore;
	public Text TxtHighest;
	public Button BtnRetry;
	public RectTransform PanelGameover;

	public bool gameOver = false;
	
//	private int fieldWidth;
//	private int fieldHeight;
	private bool[,,] fields;
	private Dictionary<string, bool> posFields;
	private int[] cubeYposition;
	private Transform[] cubeTransforms;

	private int clearTimes;
	private float addSpeed = .3f;
	private int TimeToAddSpeed = 10;
	
	private int Score = 0;
	private int Highest = 0;
	private int blockRandom;
	private GameObject nextBlock;
	private Square nextB;
	private int nextSize;
	private string[] nextblock;
	
	public static SquaresManager manager;

	// Use this for initialization
	void Start () {
	
		if (manager == null) {
			manager = this;
		}
		
		if (PlayerPrefs.HasKey("Highest")) {
			Highest = PlayerPrefs.GetInt("Highest");
		}
		else{
			PlayerPrefs.SetInt("Highest", 0);
		}
		
		blockRandom = 6;//Random.Range(0, blocks.Length);

		Score = 0;
//		fieldWidth = _fieldWidth + 2;
//		fieldHeight = _fieldHeight;
//		fields = new bool[fieldWidth, fieldHeight, fieldWidth];
		cubeYposition = new int[_fieldWidth *  _fieldHeight * _fieldWidth];
		cubeTransforms = new Transform[_fieldWidth * _fieldHeight * _fieldWidth];
//		
		posFields = new Dictionary<string, bool> ();

//		for (int y = 0; y < fieldHeight; y++) {
//			for (int z = 0; z < fieldWidth; z++) {
//				for (int x = 0; x < fieldWidth; x++) {
//
//					string key = z + "-" + y + "-" + x;
//					posFields[key] = false;
//
//					fields [z, y, x] = false;
//
//					if (y == 0) {
//						posFields[key] = true;
//						fields[z, y, x] = true;
//					}
//					else {
//						if (x == 0 || x == fieldWidth - 1
//						    || z == 0 || z == fieldWidth - 1) {
//							posFields[key] = true;
//							fields[z, y, x] = true;
//						}
//					}
//
//				}
//			}
//		}

//		Debug.Log ("ground:" + ground.transform.position.y);
//		Debug.Log ("left:" + leftWall.transform.position.x);
//		Debug.Log ("right:" + rightWall.transform.position.x);
//		Debug.Log ("front:" + frontWall.transform.position.z);
//		Debug.Log ("back:" + backWall.transform.position.z);

		for (float y = ground.transform.position.y; y < _fieldHeight; y += 1f) {
			for (float z = frontWall.transform.position.z; z < backWall.transform.position.z; z += 1f) {
				for (float x = leftWall.transform.position.x; x < rightWall.transform.position.x; x += 1f) {

					int ix = Mathf.FloorToInt(x);
					int iy = Mathf.FloorToInt(y);
					int iz = Mathf.FloorToInt(z);

					string key = iz + "-" + iy + "-" + ix;
					posFields[key] = false;
//
//					if (y == 0) {
//						posFields[key] = true;
//					}
//					else {
//						if (x == 0 || x == fieldWidth - 1
//						    || z == 0 || z == fieldWidth - 1) {
//							posFields[key] = true;
//						}
//					}
				}
			}
		}

//		Debug.Log ("posFields:");
//		foreach (var it in posFields) {
//			Debug.Log (it.Key + " - " + it.Value);
//		}


//		xOffset = 6.0f;
//		yOffset = 0.5f;
//		zOffset = 6.0f; 

//		//leftWall.position = new Vector3(maxBlockSize - .5f, leftWall.position.y, leftWall.position.z);
//		//rightWall.position = new Vector3(fieldWidth - maxBlockSize + .5f, rightWall.position.y, rightWall.position.z);
//		//Camera.main.transform.position = new Vector3(fieldWidth/2, fieldHeight/2, -16.0f);
//		

		CreateBlock(blockRandom);

		if (BtnRetry) {
			BtnRetry.onClick.AddListener( () => {RestartGame();} );
		}
	}

	void RestartGame () {
		Debug.Log ("Restart Game");

		var cubes = GameObject.FindGameObjectsWithTag("Cube");

		for (int i = 0; i < cubes.Length; i++) {
			GameObject cube = cubes[i];
			Dbg.Box (cube.transform.position);
			Destroy(cube);
		}

		posFields.Clear ();

		for (float y = ground.transform.position.y; y < _fieldHeight; y += 1f) {
			for (float z = frontWall.transform.position.z; z < backWall.transform.position.z; z += 1f) {
				for (float x = leftWall.transform.position.x; x < rightWall.transform.position.x; x += 1f) {
					
					int ix = Mathf.FloorToInt(x);
					int iy = Mathf.FloorToInt(y);
					int iz = Mathf.FloorToInt(z);
					
					string key = iz + "-" + iy + "-" + ix;
					posFields[key] = false;
				}
			}
		}

		blockRandom = 0;//Random.Range(0, blocks.Length);
		
		Score = 0;
		
		CreateBlock(blockRandom);

		if (PanelGameover.gameObject.activeSelf) {
			PanelGameover.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		TxtScore.text = Score.ToString ();
		TxtHighest.text = Highest.ToString ();

		if (Input.GetKey (KeyCode.Return)) {
			if (PanelGameover.gameObject.activeSelf) {
				RestartGame ();
			}
		}
	}

	public int GetFieldWidth() {
		return _fieldWidth;
	}
	
	public int GetFieldHeight() {
		return _fieldHeight;
	}
	
	public int GetBlockRandom() {
		return blockRandom;
	}
	
	void CreateBlock(int random) {
		GameObject o = (GameObject)Instantiate(blocks[random]);
		blockRandom = Random.Range(0, blocks.Length);
//		nextBlock = blocks[blockRandom];
//		nextB = (Square)nextBlock.GetComponent("Block");
//		nextSize = nextB.size;
//		nextblock = new string[nextSize];
//		nextblock = nextB.size;
	}


	/*
	 *  return true if overlaped
	 */
	public bool CheckSquareOverlap(bool [,,] matrix, float zPos, float yPos, float xPos) {

		int size = matrix.GetLength(0);

		for (int y = 0; y < size; y++) {
			for (int z = 0; z < size; z++) {
				for (int x = 0; x < size; x++) {

					int ix = Mathf.FloorToInt (xPos + x - ((float)size * 0.5f));
					int iy = Mathf.FloorToInt (yPos + y - ((float)size * 0.5f));
					int iz = Mathf.FloorToInt (zPos + z - ((float)size * 0.5f));

					string key = iz + "-" + iy + "-" + ix;

					if (matrix [z, y, x]) {

						float fx = ix + .5f;
						float fy = iy + .5f;
						float fz = iz + .5f;

//						Dbg.Box(new Vector3(fx, fy, fz));

						if (!posFields.ContainsKey(key)) {
//							Debug.Log("Position Out of Range: z: " + z + " y: " + y + " x: " + x + " iz: " + iz + " iy: " + iy + " ix: " + ix);
							return true;
						}

						if (posFields [key]) {
							Debug.Log("Position Overlaped: z: " + z + " y: " + y + " x: " + x + " iz: " + iz + " iy: " + iy + " ix: " + ix);
							return true;
						}
					}
				}
			}
		}

		return false;
	}

	public void PlaceSquareCube(bool[,,] matrix, float zPos, float yPos, float xPos) {

		Debug.Log ("Place Square Cube: z:" + zPos + " y: " + yPos + " x: " + xPos);

		int size = matrix.GetLength(0);

		for (int y = 0; y < size; y++) {
			for (int z = 0; z < size; z++) {
				for (int x = 0; x < size; x++) {
					if (matrix[z, y, x]) {
						float fx = xPos + x - ((float)size * 0.5f) + .5f;
						float fy = yPos + y - ((float)size * 0.5f) + .5f;
						float fz = zPos + z - ((float)size * 0.5f) + .5f;

//						int ix = Mathf.FloorToInt (xPos + x - ((float)size * 0.5f));
//						int iy = Mathf.FloorToInt (yPos + y - ((float)size * 0.5f));
//						int iz = Mathf.FloorToInt (zPos + z - ((float)size * 0.5f));

						string key = Mathf.FloorToInt(fz) + "-" + Mathf.FloorToInt(fy) + "-" + Mathf.FloorToInt(fx);

						Instantiate(cube, new Vector3(fx, fy, fz), Quaternion.identity);
						posFields [key] = true;
					}
				}
			}
		}

		StartCoroutine(CheckFloorFull(yPos, size));
	}

	IEnumerator CheckFloorFull(float yPos, float size) {

		yield return null;

		int count = 1;

		for (float y = yPos - ((float)size * 0.5f) + .5f; y < yPos + ((float)size * 0.5f) - .5f; y += 1f) {

			bool full = true;

			for (float z = frontWall.transform.position.z; z < backWall.transform.position.z; z += 1f) {
				for (float x = leftWall.transform.position.x; x < rightWall.transform.position.x; x += 1f) {
					
					string key = Mathf.FloorToInt(z) + "-" + Mathf.FloorToInt(y) + "-" + Mathf.FloorToInt(x);

					if (!posFields.ContainsKey(key)) {
						Debug.Log ("No Such Pos: " + key);
						full = false;
					}
					else {
						if (!posFields[key]) {
							full = false;
							break;
						}
					}
				}

				if (!full) break;
			}

			if (full) {
				yield return StartCoroutine(RemoveFloor(y));
				Score += 10 * count;
				y -= 1f;
				count++;
			}
		}

		CreateBlock(blockRandom);
	}
	
	IEnumerator RemoveFloor(float yPos) {

		Debug.Log ("RemoveFloor: " + yPos);
//		Debug.Break ();

		for (float y = yPos; y < _fieldHeight; y += 1f) {
			for (float z = frontWall.transform.position.z; z < backWall.transform.position.z; z += 1f) {
				for (float x = leftWall.transform.position.x; x < rightWall.transform.position.x; x += 1f) {

					int ix = Mathf.FloorToInt(x);
					int iy = Mathf.FloorToInt(y);
					int iz = Mathf.FloorToInt(z);

					string keyup = iz + "-" + (iy + 1) + "-" + ix;
					string keydown = iz + "-" + iy + "-" + ix;

					if (posFields.ContainsKey(keydown)) {
						if (posFields.ContainsKey(keyup)) {
							posFields[keydown] = posFields[keyup];
						}
						else {
							posFields[keydown] = false;
						}
					}
				}
			}
		}
		
		var cubes = GameObject.FindGameObjectsWithTag("Cube");
		int cubeToMove = 0;
		for (int i = 0;i < cubes.Length;i++) {
			GameObject cube = cubes[i];

			if (Mathf.FloorToInt(cube.transform.position.y) > Mathf.FloorToInt(yPos)) {
				cubeYposition[cubeToMove] = (int)(cube.transform.position.y);
				cubeTransforms[cubeToMove++] = cube.transform;
			}
			else if (Mathf.FloorToInt(cube.transform.position.y) == Mathf.FloorToInt(yPos)) {
				Dbg.Box(cube.transform.position);
				Destroy(cube);
			}
		}
		
		float t = 0;
		while (t <= 1f) {
			t += Time.deltaTime * 5f;
			for(int i = 0;i < cubeToMove;i++) {
				cubeTransforms[i].position = new Vector3(cubeTransforms[i].position.x, Mathf.Lerp(cubeYposition[i], cubeYposition[i] - 1, t),
					cubeTransforms[i].position.z);
			}
		    yield return null;
		}
		
		if (++clearTimes == TimeToAddSpeed) {
			blockNormalFallSpeed += addSpeed;
			clearTimes = 0;
		}
	}
	
	public void GameOver() {
		if (Score > PlayerPrefs.GetInt("Highest")) {
			PlayerPrefs.SetInt("Highest", Score);
		}

		gameOver = true;

		if (!PanelGameover.gameObject.activeSelf) {
			PanelGameover.gameObject.SetActive(true);
		}
		print("Game Over!!!");
	}

	void OnGUI() {

//		GUI.Box (new Rect (20, 20, 240, 160), "");
//
//		var centerStyle = new GUIStyle(GUI.skin.GetStyle("Label"));;
//		centerStyle.alignment = TextAnchor.UpperCenter;
//		centerStyle.fontSize = 25;
//
//		var leftStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
//		leftStyle.alignment = TextAnchor.MiddleLeft;
//		leftStyle.fontSize = 18;
//
//		var rightStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
//		rightStyle.alignment = TextAnchor.MiddleRight;
//		rightStyle.fontSize = 18;
//
//		GUI.Label (new Rect (20, 40, 240, 40), "Panel", centerStyle);
//		GUI.Label(new Rect(40, 80, 80, 40), "Score:", leftStyle);
//		GUI.Label(new Rect(120, 80, 100, 40), Score.ToString(), rightStyle);
//		GUI.Label(new Rect(40, 120, 80, 40), "Highest:", leftStyle);
//		GUI.Label(new Rect(120, 120, 100, 40), Highest.ToString(), rightStyle);
//
//		if (gameOver) {
//
//		}
	}
	
}
