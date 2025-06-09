using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Player : MonoBehaviour
{
    // Singelton
    GameManager gameManager;
    // Input Map
    Controls input;
    // Obstacle & Movement
    List<GameObject> grid = new List<GameObject>(); // <-- to see the grid
    Transform moveArea; // <-- to see the path
    RaycastHit hit; // <-- to see what has infront
    bool canMove;
    bool findObstacle; // if there is an obstacle infront of the player (use to check if can vomit)
    Object objEat; // <-- object eat
    List<Transform> limits = new List<Transform>(); // <-- to find limit
    // Transform[] limits;
    // Win condition
    GameObject winPos;

    //non necessario
    Transform form;
    Vector3 skinny;
    Vector3 fat = Vector3.one * 0.9f;

    // leggi sotto vv
    Vector3 startPos;

    [SerializeField]
    Hints hintManager;

    private void Awake()
    {
        // Input Map
        input = new Controls();
        // Player direction
        input.PlayerMovement.Up.started += context => MoveUp();
        input.PlayerMovement.Down.started += context => MoveDown();
        input.PlayerMovement.Left.started += context => MoveLeft();
        input.PlayerMovement.Right.started += context => MoveRight();
        // Eat
        input.PlayerAction.EatSplit.started += context => Eat();
        // Reset level
        input.PlayerAction.Reset.started += context => Reset();
        input.PlayerAction.Quit.started += context => Quit();
    }

    private void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    private void Start()
    {
        // Singelton
        gameManager = GameManager.Instance;
        // Win condition
        winPos = gameManager.GetWinColumns().gameObject;
        // Limit
        limits = gameManager.GetLimits();
        // Grid
        Transform map = gameManager.GetMap().transform;
        foreach (Transform child in map)
        {
            grid.Add(child.gameObject);
        }
        // second floor implementation <-- attenzione qui
        foreach(Transform b in gameManager.GetBoxes())
        {
            grid.Add(b.gameObject);
        }
        // and stairs
        foreach (Transform s in gameManager.GetStairs())
        {
            grid.Add(s.gameObject);
        }
        // Future player position
        moveArea = transform.GetChild(1);

        // non necessario
        form = transform.GetChild(0);
        skinny = form.localScale;

        // bug fix scale
        startPos = transform.position + Vector3.up*2;
        // dovrei alzare la sua posizione per controllare se sono in quel preciso caso
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #region Movement & check Next Position
    void MoveUp()
    {
        transform.eulerAngles = Vector3.down * -90;
        CheckMovement();
        ScaleCheck();
        if (canMove)
            transform.position += Vector3.right;
        CheckWinCondition();
    }
    void MoveDown()
    {
        transform.eulerAngles = Vector3.down * 90;
        CheckMovement();
        ScaleCheck();
        if (canMove)
            transform.position += Vector3.left; 
        CheckWinCondition();
    }
    void MoveLeft()
    {
        transform.eulerAngles = Vector3.zero;
        CheckMovement();
        ScaleCheck();
        if (canMove)
            transform.position += Vector3.forward;
        CheckWinCondition();
    }
    void MoveRight()
    {
        transform.eulerAngles = Vector3.up * -180;
        CheckMovement();
        ScaleCheck();
        if (canMove)
            transform.position += Vector3.back;
        CheckWinCondition();
    }

    void CheckMovement()
    {
        int move = 0;
        foreach (GameObject tile in grid)
        {
            if (moveArea.position == tile.transform.position)
                move++;
        }

        if (move > 0)
            canMove = true;
        else
            canMove = false;

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            canMove = false;
            findObstacle = true;

            if (hit.collider.CompareTag("Invisible") && objEat == null || hit.collider.CompareTag("Stair"))
            {
                canMove = true;
                findObstacle = true;
            }
        }
        else
            findObstacle = false;
    }
    void ScaleCheck()
    {
        if (!Physics.Raycast(moveArea.position + Vector3.up, transform.TransformDirection(Vector3.down), out RaycastHit hitFloor, 1)
            && Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.down), out RaycastHit playerTile, 1)
                && playerTile.collider != null && playerTile.collider.CompareTag("Stair")
                    && Physics.Raycast(moveArea.position + Vector3.up / 2, transform.TransformDirection(Vector3.down), out RaycastHit g, 1.3f) 
                        && g.collider != null && !g.collider.CompareTag("Invisible"))
        {
            canMove = true;
            transform.position += Vector3.down;
        }
    }
    void CheckWinCondition()
    {
        // Check platform pos
        gameManager.Check();
        // Check win condition
        if (transform.position + Vector3.up == winPos.transform.position)
        {
            Debug.Log("YOU WIN!");
            /*if (SceneManager.GetActiveScene().buildIndex == 10)
                Application.Quit();
            else*/

            if (SceneManager.GetActiveScene().buildIndex != 40)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
            {
                Quit();
            }
        }
    }
    #endregion
    #region Eat
    void Eat()
    {
        CheckMovement();

        // Check if split infront of the Win position
        if (winPos.transform.position + Vector3.down != moveArea.position && objEat != null) // Split
        {
            if (!canMove && !findObstacle)
            {
                objEat.gameObject.SetActive(true);
                objEat.gameObject.transform.position = moveArea.position;
                // grid.Add(objEat.gameObject);
                objEat = null;
                // Check platform pos
                gameManager.Check();

                form.localScale = skinny;
                if (hintManager)
                    hintManager.EatHint();
            }
            else if (canMove && !findObstacle)
            {
                objEat.gameObject.SetActive(true);
                objEat.gameObject.transform.position = moveArea.position + Vector3.up;
                objEat = null;
                // Check platform pos
                gameManager.Check();
                // non necessario
                form.localScale = skinny;
                if (hintManager)
                    hintManager.EatHint();
            }
        }
        else if (hit.collider != null && objEat == null) // Eat
        {
            bool canEat = true;
            // Check limits position
            if (limits != null)
                foreach (Transform l in limits)
                {
                    if (l.position + Vector3.down == transform.position)
                        canEat = false;
                }
            // if the player isn't on a door 
            if (canEat)
            {
                objEat = hit.collider.GetComponent<Object>();
                if (objEat != null)
                {
                    objEat.gameObject.SetActive(false);
                    objEat.gameObject.transform.position = Vector3.up * 2;
                    // Check platform pos
                    gameManager.Check();

                    form.localScale = fat;
                    if (hintManager)
                        hintManager.SplitHint();
                }
            }
        }
    }
    #endregion
    #region InputMap
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    #endregion
}