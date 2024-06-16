using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInstance : MonoBehaviour
{
    public float speed;
    public float turnSpeed;


    [Range(-10,10)] public float rudder;

    [Header("References:")]
    [SerializeField] private LineRenderer _lineRenderer;

    private void Update()
    {
        if (GameMaster.turnUnderway)
        {
            TurnUpdate();
        }
        else
        {
            CalculateTrajectory();
        }

    }   

    private void TurnUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.Rotate(0, rudder * turnSpeed * Time.deltaTime, 0);
    }

    private void CalculateTrajectory()
    {
        int increments = 1;

        _lineRenderer.positionCount = increments+1;

        Vector3 startPosition = transform.position + Vector3.up * 0.25f;
        _lineRenderer.SetPosition(0, startPosition);

        

        float timeStep = GameMaster.TURN_TIME / (float) increments;
        Vector3 position = startPosition;
        Vector3 direction = transform.forward;
        for (int i = 0; i < increments; i++)
        {
            float distanceToTravel = speed * timeStep;

            float theta = rudder * turnSpeed * Mathf.Deg2Rad;

            float rightAmount = Mathf.Sin(theta) * distanceToTravel;
            float forwardAmount = Mathf.Cos(theta) * distanceToTravel;

            Vector3 directionRight = new Vector3(direction.z, direction.y, direction.x);

            Vector3 newPosition = position;
            newPosition += direction * forwardAmount;
            newPosition += directionRight * rightAmount;
            
            direction = newPosition - position; 
            position = newPosition;     

            _lineRenderer.SetPosition(i + 1, newPosition);
        }

        // // 1 point for the start of each line + 1 at the end of the arc.
        // int numberOfLines = 10;
        // float angle = rudder * turnSpeed;

        // _lineRenderer.positionCount = numberOfLines + 1;

        // Vector3[] positions = new Vector3[numberOfLines + 1]; 

        // // the angle must be in radian in order to use Unity's Mathf
        // float radianAngle = Mathf.Deg2Rad * angle;

        // float dst = speed * GameMaster.TURN_TIME;
        // float xSpeed = Mathf.Cos(angle) * dst;
        // float ySpeed = Mathf.Sin(angle) * dst;

        // float trajectoryDistance = xSpeed * (ySpeed + Mathf.Sqrt(ySpeed * ySpeed));
        
        // for (int i = 0; i < numberOfLines +1; i++)
        // {
        //     // cast numberOfLines to float so the answer is calculated as a float
        //     float t = i /  (float)numberOfLines;
        //     float x = t * trajectoryDistance;

        //     float y = x * Mathf.Tan(radianAngle) - ((x * x) / (2 * speed * speed * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        //     positions[i] = (y * transform.right) + (x * transform.forward) + transform.position;
        // }

        // _lineRenderer.SetPositions(positions);

    }
}
