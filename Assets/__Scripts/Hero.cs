using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    public enum Direction
    {
        up,
        down,
        left,
        right
    }

    Vector3 fromPosition;
    Vector3 toPosition;
    float moveTime;
    float moveSpeed;
    public bool isMoving;
    public Direction direction;
    public int moveStep;

    private DiceManager DM;

    void Start()
    {
        fromPosition = Vector3.zero;
        toPosition = Vector3.zero;
        moveTime = 0;
        moveSpeed = 10;
        isMoving = false;
        direction = Direction.up;
        moveStep = 0;
        DM = GameObject.Find("Dice").GetComponent<DiceManager>();
    }

    void Update()
    {
        if (moveStep > 0)
        {
            moving();
        }
        if (moveStep > 0 && !isMoving && !DM.isPauseToSelect)
        {
            setupMove();
        }
    }

    void setupMove()
    {
        fromPosition = this.transform.position;
        switch (direction)
        {
            case Direction.up:
                toPosition = this.transform.position + new Vector3(0, 0, 1);
                break;
            case Direction.down:
                toPosition = this.transform.position - new Vector3(0, 0, 1);
                break;
            case Direction.left:
                toPosition = this.transform.position - new Vector3(1, 0, 0);
                break;
            case Direction.right:
                toPosition = this.transform.position + new Vector3(1, 0, 0);
                break;
        }
        isMoving = true;
        moveTime = 0;
    }

    void moving()
    {
        if (isMoving)
        {
            moveTime += Time.deltaTime * moveSpeed;
            this.transform.position = Vector3.Lerp(fromPosition, toPosition, moveTime);
            if (moveTime > 1)
            {
                this.transform.position = toPosition;
                moveTime = 0;
                moveStep -= 1;
                isMoving = false;
            }
        }
    }

    void endMove()
    {
        moveStep = 0;
    }
}
