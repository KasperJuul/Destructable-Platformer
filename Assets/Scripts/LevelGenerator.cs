using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] startingPositions;
    public GameObject[] rooms; // 0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB

    public float moveAmount;
    public int formerDirection;
    public int direction;
    public int roomType;

    private float timeStep;
    public float startTime = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    private bool stopGeneration = false;
    public bool automaticGeneration = false;

    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        roomType = 0;
        direction = Random.Range(1, 6);
        if (direction == 3)
        {
            roomType = 1;
            Instantiate(rooms[roomType], transform.position, Quaternion.identity);
        }
        else
        {
            if ((direction < 3 && transform.position.x <= minX) ||
                (direction > 3 && transform.position.x >= maxX))
            {
                roomType = 1;
            }
            Instantiate(rooms[roomType], transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (automaticGeneration)
        {
            if (timeStep <= 0 && !stopGeneration)
            {
                Move();
                timeStep = startTime;
            }
            else
            {
                timeStep -= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && !stopGeneration)
            {
                Move();
            }
        }
        
    }

    private void Move()
    {
        if (direction > 3) // MOVE RIGHT
        {
            Right();

        }
        else if (direction < 3) // MOVE LEFT
        {
            Left();

        }
        else if (direction == 3) // MOVE DOWN
        {
            Down();

        }
        if (transform.position.y < minY)
        {
            stopGeneration = true;
        }
        else if(!stopGeneration)
        {
            Instantiate(rooms[roomType], transform.position, Quaternion.identity);
        }
    }

    public void Right()
    {
        if (transform.position.x < maxX)
        {
            formerDirection = direction;        // save our current direction
            direction = Random.Range(3, 6);     // get new direction

            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y); // new position for next room;
            transform.position = newPos;



            CalcRoomType(formerDirection,direction);        // Get the type of room to be placed in the new position
        }
        else
        {
            direction = 3;
            Down();
        }
    }

    public void Left()
    {
        if (transform.position.x > minX)
        {
            formerDirection = direction;        // save our current direction
            direction = Random.Range(1, 4);     // get new direction

            Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y); // new position for next room;
            transform.position = newPos;

            if (newPos.x <= minX)
            {
                direction = 3;
            }

            CalcRoomType(formerDirection, direction);        // Get the type of room to be placed in the new position
        }
        else
        {
            direction = 3;
            Down();
        }
    }

    private void Down()
    {
        if (transform.position.y > minY)
        {
            formerDirection = direction;

            Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
            transform.position = newPos;

            if (newPos.x <= minX)
            {
                direction = Random.Range(3, 6);
            }
            else if (newPos.x >= maxX)
            {
                direction = Random.Range(1, 4);
            }

            CalcRoomType(formerDirection, direction);
        }
        else
        {
            // STOP LEVEL GENERATION
            stopGeneration = true;
        }
    }

    private void CalcRoomType(int formerDirection, int direction)
    {
        if (formerDirection == 3)
        {
            if(direction == 3)
            {
                roomType = 3;
            }
            else
            {
                roomType = 2;
            }
        }
        else
        {
            if (direction == 3)
            {
                roomType = 1;
            }
            else
            {
                roomType = 0;
            }
        }
    }
}
