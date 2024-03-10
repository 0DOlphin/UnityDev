using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	/*
	cells: Масив грідів, які представляють плитки головоломки.
	random_cells: Масив для зберігання випадкового розташування плиток.
	r_cells: Масив для зберігання плиток, розміщених випадково.
	r_grid: Двовимірний масив, який використовується для зберігання випадкового розташування плиток.
	grid: Статичний двовимірний масив, що представляє головоломку.
	position: Статичний двовимірний масив, який зберігає позиції для розташування плиток головоломки.
	checkmas: Масив для перевірки можливості переміщення плиток.
	win: Статична змінна, що вказує на те, чи виграв гравець.
	*/

    public GameObject[] cells;
    private GameObject[] random_cells;
    private GameObject[] r_cells;
    private int[,] r_grid;
    public static GameObject[,] grid;
    public static Vector3[,] position;
    public int[] checkmas;
    private static bool win = false;

    public float startPosX;
    public float startPosY;
    public float offsetX;
    public float offsetY;

    private int sum = 0;
    private int zero = 0;
    private int h;
    private int v;

    private static GameObject txt;
    private Component[] boxes;
    private Component[] p_script;

    void Start () {   
	/*
	Ініціалізує змінні та масиви.
	Генерує випадковий початковий стан головоломки. 
	*/
        r_grid = new int[4, 4];
        txt = GameObject.FindGameObjectWithTag ("congratulations");
        boxes = GetComponentsInChildren<BoxCollider2D> ();
        p_script = GetComponentsInChildren<Puzzle> ();
        checkmas = new int[16];
        random_cells = new GameObject[cells.Length];
        r_cells = new GameObject[cells.Length];
        float posXreset = startPosX;
        position = new Vector3[4, 4];
        for (int y = 0; y < 4; y++) {
            startPosY -= offsetY;
            for (int x = 0; x < 4; x++) {
                startPosX += offsetX;
                position[x, y] = new Vector3(startPosX, startPosY, 0);
            }
            startPosX = posXreset;
        }
        RandomPuzzle(true);
    }

    void Update () {

    }

    public void StartNewGame(){
        win = false;
        txt.GetComponent<Text> ().color = new Color (0, 0, 0, 0);
        RandomPuzzle (true);
    }


    public void Possibility(int[] mas){

	// Перевіряє можливість переміщення плиток головоломки для досягнення виграшу.

        for (int i = 0; i < 16; i++) {
            if (mas[i] == 0) {
                zero = i / 4 + 1;
            }
            else 
                for (int k = i; k < 16; k++) {
                    if (mas[i] > mas[k] && mas[k] != 0) {
                        sum++;
                    }
                }
        }

        if ((zero + sum) % 2 == 0) {
            Debug.Log("There is a solution");
        } else {            
            Debug.Log("There is NO solution... Reshuffling... ");
            CreatePuzzle();
        }
    }

    void CreatePuzzle() {
	
	/*
	Створює новий стан головоломки.
	Перевіряє можливість переміщення плиток для досягнення виграшу.
	*/

        if (transform.childCount > 0) {
            for (int j = 0; j < transform.childCount; j++) {
                Destroy(transform.GetChild(j).gameObject);
            }
        }
        int i = 0;
        int ii = 0;
        grid = new GameObject[4, 4];
        h = Random.Range(0, 3);
        v = Random.Range(0, 3);

        GameObject clone = new GameObject();
        grid[h, v] = clone; 
        float posXreset = startPosX;

        for (int y = 0; y < 4; y++) { // Визначає, які плитки розташовані на випадкових позиціях. 

            for (int x = 0; x < 4; x++) { 

                if (grid[x, y] == null) {         
                    startPosX += offsetX;
                    grid[x, y] = Instantiate(random_cells[i], position[x, y], Quaternion.identity) as GameObject;
                    grid[x, y].name = "ID-" + i;
                    checkmas[ii] = grid[x, y].GetComponent<Puzzle>().ID;
                    grid[x, y].transform.parent = transform;
                    i++;
                    ii++;
                } else {
                    checkmas[ii] = 0;
                    ii++;
                }
            }
        }
        foreach (BoxCollider2D box2d in boxes)
            box2d.enabled = true;
        foreach (Puzzle puz in p_script)
            puz.enabled = true;

        Destroy(clone); 
        for (int q = 0; q < cells.Length; q++) {
            Destroy(random_cells[q]);
        }
        Possibility(checkmas);
    }

    void RandomPuzzle(bool r_s) {
        if (r_s == true) {
        int[] tmp = new int[cells.Length];
        for (int i = 0; i < cells.Length; i++) {
            tmp[i] = 1;
        }
        int c = 0;
        while (c < cells.Length) {
            int r = Random.Range(0, cells.Length);
            if (tmp[r] == 1) { 
                random_cells[c] = Instantiate(cells[r], new Vector3(0, 10, 0), Quaternion.identity) as GameObject;    
                    r_cells[c] = random_cells[c];
                tmp[r] = 0;
                c++;
            }
        }
            CreatePuzzle ();
        } else {
            CreatePuzzle ();
        }
    }

    static public void GameFinish() {
        int i = 1;
        for (int y = 0; y < 4; y++) {
            for (int x = 0; x < 4; x++) {
                if (grid[x, y]) { 
                    if (grid[x, y].GetComponent<Puzzle>().ID == i) i++; 
                } else {
                    i--;
                }
            }
        }
        if (i == 15) {
            for (int y = 0; y < 4; y++) {
                for (int x = 0; x < 4; x++) {
                    if (grid[x, y]) Destroy(grid[x, y].GetComponent<Puzzle>());
                }
            }
            win = true;
            txt.GetComponent<Text>().color = new Color(0, 0, 0, 1);

            Debug.Log("Finish!");
        }
    }
}
