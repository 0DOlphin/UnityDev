using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text winText;
    public TMP_Text lossText;
    public int startingScore = 100; 
    public int wallPenalty = 10; 
    public float speed = 2.0f;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private Camera mainCamera;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        score = startingScore;
        SetCountText();
        winText.gameObject.SetActive(false);
        lossText.gameObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; 
        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0; 

        Vector3 movement = (movementY * cameraForward.normalized + movementX * cameraRight.normalized).normalized * speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    void OnTriggerEnter(Collider other)
    {
    if (other.gameObject.CompareTag("Wall"))
        {
            score -= wallPenalty; 
            SetCountText(); 
            if (score <= 0)
            {
                lossText.gameObject.SetActive(true);
            }
        }
            else if (other.gameObject.CompareTag("Finish"))
            {
                winText.gameObject.SetActive(true);
                other.gameObject.SetActive(false);
            }
                else if (other.gameObject.CompareTag("Bonus"))
                {
                    score += 1000;
                    SetCountText();
                    other.gameObject.SetActive(false);
                }
    }

    void SetCountText()
    {
        countText.text = "Score: " + score.ToString();
    }

}
