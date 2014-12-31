using UnityEngine;
using System.Collections;

public class SquaresManager : MonoBehaviour {
	
	public GameObject[] blocks;
	public Transform cube;
	public Transform ground;
	public Transform leftWall;
	public Transform rightWall;
	public Transform frontWall;
	public Transform backWall;

	public int maxBlockSize = 4;
	public int _fieldWidth = 10;
	public int _fieldHeight = 13;
	public float blockNormalFallSpeed = 2f;
	public float blockDropSpeed = 30f;
	public Texture2D cubeTexture;
	
	private int fieldWidth;
	private int fieldHeight;
	private bool[,,] fields;
	private int[] cubeYposition;
	private Transform[] cubeTransforms;
	private float xOffset;
	private float yOffset;
	private float zOffset;

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
	
		if (manager == null){
			manager = this;
		}
		
		if (PlayerPrefs.HasKey("Highest")){
			Highest = PlayerPrefs.GetInt("Highest");
		}
		else{
			PlayerPrefs.SetInt("Highest", 0);
		}
		
		blockRandom = Random.Range(0, blocks.Length);
//		
		fieldWidth = _fieldWidth + 2;
		fieldHeight = _fieldHeight + 1;
		fields = new bool[fieldWidth, fieldHeight, fieldWidth];
//		cubeYposition = new int[fieldWidth, fieldHeight, fieldWidth];
//		cubeTransforms = new Transform[fieldWidth, fieldHeight, fieldWidth];
//		

		for (int y = 0; y < fieldHeight; y++) {
			for (int z = 0; z < fieldWidth; z++) {
				for (int x = 0; x < fieldWidth; x++) {

					fields [z, y, x] = true;

					if (y == 0) {
						fields[z, y, x] = false;
					}
					else {
						if (x == 0 || x == fieldWidth - 1
						    || z == 0 || z == fieldWidth - 1) {
							fields[z, y, x] = false;
						}
					}

				}
			}
		}

		Debug.Log ("ground:" + ground.transform.position.y);
		Debug.Log ("left:" + leftWall.transform.position.x);
		Debug.Log ("right:" + rightWall.transform.position.x);
		Debug.Log ("front:" + frontWall.transform.position.z);
		Debug.Log ("back:" + backWall.transform.position.z);

		xOffset = 5.0f;
		yOffset = 0.5f;
		zOffset = 5.0f; 

//		//leftWall.position = new Vector3(maxBlockSize - .5f, leftWall.position.y, leftWall.position.z);
//		//rightWall.position = new Vector3(fieldWidth - maxBlockSize + .5f, rightWall.position.y, rightWall.position.z);
//		//Camera.main.transform.position = new Vector3(fieldWidth/2, fieldHeight/2, -16.0f);
//		

		CreateBlock(1);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public int GetFieldWidth(){
		return _fieldWidth;
	}
	
	public int GetFieldHeight(){
		return _fieldHeight;
	}
	
	public int GetBlockRandom(){
		return blockRandom;
	}
	
	void CreateBlock(int random){
		GameObject o = (GameObject)Instantiate(blocks[random]);
//		blockRandom = Random.Range(0, blocks.Length);
//		nextBlock = blocks[blockRandom];
//		nextB = (Square)nextBlock.GetComponent("Block");
//		nextSize = nextB.size;
//		nextblock = new string[nextSize];
//		nextblock = nextB.size;
	}

	public bool CheckTopOverlap(bool [,,] matrix, float zPos, float yPos, float xPos){

		int size = matrix.GetLength(0);
		int ix = Mathf.RoundToInt (xPos + xOffset);
		int iy = Mathf.RoundToInt (yPos + yOffset);
		int iz = Mathf.RoundToInt (zPos + zOffset);

		for (int y = 0; y < size; y++) {
			for (int z = 0; z < size; z++) {
				for (int x = 0; x < size; x++) {
					Debug.Log("z: " + z + " y: " + y + " x: " + x);
					Debug.Log("zPos: " + zPos + " yPos: " + yPos + " xPos: " + xPos);
					Debug.Log("iz: " + iz + " iy: " + iy + " ix: " + ix);
//					Debug.Log("matrix: " + matrix[z, y, x] + " fields: " + fields[zPos - z, yPos - y, xPos - x]);

//					if (blockMatrix [z, y, x] && fields [zPos - z, xPos + x, yPos - y]) {
//						return true;
//					}
				}
			}
		}

		return false;
	}
//	
//		public void SetBlock(bool[,,] blockMatrix, int zPos, int xPos, int yPos){
//		
//		int size = blockMatrix.GetLength(0);
//		for (int y = 0;y < size;y++){
//			for (int x = 0;x < size;x++){
//				if (blockMatrix[y, x]){
//					Instantiate(cube, new Vector3(xPos + x, yPos - y, 0), Quaternion.identity);
//					fields[xPos + x, yPos - y] = true;
//				}
//			}
//		}
//		StartCoroutine(CheckRows(yPos - size, size));
//		
//	}
//	
//	IEnumerator CheckRows(int yStart, int size){
//		yield return null;
//		if (yStart < 1)yStart = 1;
//		int count = 1;
//		for (int y = yStart;y < yStart + size;y++){
//			int x;
//			for (x = maxBlockSize;x < fieldWidth - maxBlockSize;x++){
//				if (!fields[x, y]){
//					break;
//				}
//			}
//			if (x == fieldWidth - maxBlockSize){
//				yield return StartCoroutine(SetRows(y));
//				Score += 10 * count;
//				y--;
//				count++;
//			}
//		}
//		CreateBlock(blockRandom);
//	}
//	
//	IEnumerator SetRows(int yStart){
//		for (int y = yStart;y < fieldHeight - 1;y++){
//			for (int x = maxBlockSize;x < fieldWidth - maxBlockSize;x++){
//				fields[x, y] = fields[x, y + 1];
//			}
//		}
//		
//		for (int x = maxBlockSize;x < fieldWidth - maxBlockSize;x++){
//			fields[x, fieldHeight - 1] = false;
//		}
//		
//		var cubes = GameObject.FindGameObjectsWithTag("Cube");
//		int cubeToMove = 0;
//		for (int i = 0;i < cubes.Length;i++){
//			GameObject cube = cubes[i];
//			if (cube.transform.position.y > yStart){
//				cubeYposition[cubeToMove] = (int)(cube.transform.position.y);
//				cubeTransforms[cubeToMove++] = cube.transform;
//			}
//			else if (cube.transform.position.y == yStart){
//				Destroy(cube);
//			}
//		}
//		
//		float t = 0;
//		while (t <= 1f){
//			t += Time.deltaTime * 5f;
//			for(int i = 0;i < cubeToMove;i++){
//				cubeTransforms[i].position = new Vector3(cubeTransforms[i].position.x, Mathf.Lerp(cubeYposition[i], cubeYposition[i] - 1, t),
//					cubeTransforms[i].position.z);
//			}
//		    yield return null;
//		}
//		
//		if (++clearTimes == TimeToAddSpeed){
//			blockNormalFallSpeed += addSpeed;
//			clearTimes = 0;
//		}
//		
//	}
//	
	public void GameOver(){
		if (Score > PlayerPrefs.GetInt("Highest")){
			PlayerPrefs.SetInt("Highest", Score);
		}
		print("Game Over!!!");
	}

	void OnGUI(){
		GUI.Label(new Rect(180, 30, 80, 40),"Score:");
		GUI.Label(new Rect(240, 30, 100, 40),Score.ToString());
		GUI.Label(new Rect(180, 50, 80, 40),"Highest:");
		GUI.Label(new Rect(240, 50, 80, 40),Highest.ToString());
		
//		for (int y = 0;y < nextSize;y++){
//			for (int x = 0;x < nextSize;x++){
//				if (nextblock[y][x] == '1'){
//					GUI.Button(new Rect(180 + 30 * x, 100 + 30 * y, 30, 30), cubeTexture);
//				}
//			}
//		}


	}
	
}
