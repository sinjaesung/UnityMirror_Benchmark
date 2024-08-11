using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : NetworkBehaviour
{
    public float speed = 1;
    public float movementProbability = 0.5f;
    public float movementDistance = 20;

    public bool moving;
    public Vector3 start;
    public Vector3 destination;

    public override void OnStartServer()
    {
        Debug.Log("MonsterMovement OnStartServer>>");
        start = transform.position;
    }

    [ServerCallback]
    void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(transform.position, destination) <= 0.01f)
            {
                transform.position = destination;
                moving = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            }
        }
        else
        {
            float r = Random.value;
            if (r < movementProbability * Time.deltaTime)
            {
                Vector2 circlePos = Random.insideUnitCircle;
                Vector3 dir = new Vector3(circlePos.x, 0, circlePos.y);

                // set destination on random pos in a circle around start.
                // (don't want to wander off)
                destination = start + dir * movementDistance;
                moving = true;
            }
        }
    }
}
