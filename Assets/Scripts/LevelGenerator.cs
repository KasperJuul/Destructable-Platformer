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
            if (transform.position.x < maxX) 
            {
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;
                formerDirection = direction;
                direction = Random.Range(3, 6);
                if (direction == 3)
                {
                    roomType = 1;
                }
            }
            else // If Max x reached, go down
            {
                if (transform.position.y <= minY)
                {
                    stopGeneration = true;
                    return;
                }
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;
                formerDirection = direction;
                direction = Random.Range(1, 4);
                if (direction == 3)
                {
                    roomType = 3;
                }
                else
                {
                    roomType = 2;
                }
            }
        }
        else if (direction < 3) // MOVE LEFT
        {
            if (transform.position.x > minX)
            {
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                formerDirection = direction;
                direction = Random.Range(1, 4);
                if (direction == 3)
                {
                    roomType = 1;
                }
            }
            else
            {
                if (transform.position.y <= minY)
                {
                    stopGeneration = true;
                    return;
                }
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;
                formerDirection = direction;
                direction = Random.Range(3, 6);
                if (direction == 3)
                {
                    roomType = 3;
                }
                else
                {
                    roomType = 2;
                }
            }
        }
        else if (direction == 3) // MOVE DOWN
        {
            if (transform.position.y > minY)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;
                if (formerDirection == 3)
                {
                    roomType = 3;
                }
                else
                {
                    roomType = 2;
                }
                formerDirection = direction;
                direction = Random.Range(1, 6);
            }
            else
            {
                // STOP LEVEL GENERATION
                stopGeneration = true;
                return;
            }
        }

        Instantiate(rooms[roomType], transform.position, Quaternion.identity);
        if (direction == 3)
        {
            roomType = 2;
        }
        else
        {
            roomType = 0;
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
            Down();
        }
    }

    private void Down()
    {
        
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
