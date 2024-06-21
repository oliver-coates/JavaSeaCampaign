using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

public class ShipGunScript : MonoBehaviour
{

    public Transform target;
    private Transform _aimObject;

    [Header("Settings:")]
    [SerializeField] private float _turnSpeed;

    [Header("Rotation settings:")]
    [SerializeField] private GunRotationBlocker[] _rotationBlockers;

    [Header("Gizmos Settings:")]
    [SerializeField] private float _gizmoRotationRadius = 5f;

    private enum RotationDirection
    {
        Left,
        Right,
        Ahead
    }

    private void Awake()
    {
        _aimObject = new GameObject("Aim Assist Object").transform;
        _aimObject.SetParent(transform);

        _aimObject.localPosition = Vector3.zero;
        _aimObject.localScale = Vector3.one;
        _aimObject.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (GameMaster.turnUnderway)
        {
        }
                    TurnUpdate();

    }

    private void TurnUpdate()
    {
        if (target != null)
        {
            TurnTurret();
        }
    }

    private void TurnTurret()
    {
        float currentDegrees = transform.rotation.eulerAngles.y;

        // Find what direction we should be looking ideally
        float desiredDegrees = FindDesiredRotation(target.position);

        // Clamp this to be within the acceptable bounds of the rotation blocker
        float targetDegrees = ShearRotationByBlockers(desiredDegrees);

        // Determine which direction it is acceptable to turn
        RotationDirection direction = FindAllowedRotationDirection(currentDegrees, targetDegrees);
        
        // Apply the rotation
        float turnAmountThisFrame = Time.deltaTime * _turnSpeed;
        
        if (direction == RotationDirection.Left)
        {
            transform.Rotate(0, -turnAmountThisFrame, 0);
        }
        else if (direction == RotationDirection.Right)
        {
            transform.Rotate(0, turnAmountThisFrame, 0);
        }
        else
        {
            // Dead ahead - snap to target
            transform.rotation = Quaternion.Euler(0, targetDegrees, 0);
        }
    }

    /// <summary>
    /// Finds the rotation, in degrees, that the turret should be turned to face the target.
    /// </summary>
    private float FindDesiredRotation(Vector3 target)
    {
        Vector3 position = transform.position;
        position.y = 0f;
        Vector3 targetPosition = target;
        targetPosition.y = 0f;

        Vector3 directionToTarget = (targetPosition - position).normalized;

        _aimObject.transform.forward = directionToTarget;

        return _aimObject.transform.eulerAngles.y;
    }

    private float ShearRotationByBlockers(float desiredRotation)
    {
        foreach (GunRotationBlocker blocker in _rotationBlockers)
        {
            // Ignore no shoot zones
            if (blocker.type != GunRotationBlocker.RotationBlockType.FullyBlocked)
            {
                continue;
            }

            // If we are within:
            if (blocker.IsWithin(desiredRotation))
            {
                float distanceToEdgeA = Mathf.Abs(blocker.to - desiredRotation);
                float distanceToEdgeB = Mathf.Abs(blocker.from - desiredRotation);

                if (distanceToEdgeA < distanceToEdgeB)
                {
                    return blocker.to;
                }
                else
                {
                    return blocker.from;
                }
            }
        }

        // Desired rotation is NOT within the target direction.
        return desiredRotation;
    }

    private RotationDirection FindAllowedRotationDirection(float currentDegrees, float targetDegrees)
    {
        // Check that we are not pointing at the target
        float distanceBetweenDegrees = Mathf.Abs(currentDegrees - targetDegrees);
        if (distanceBetweenDegrees < 1f)
        {
            // If less than 1 degree, we are pointing at the target
            return RotationDirection.Ahead;
        }

        // Find the distances from the current rotation to the target rotation
        float distanceNormal = Mathf.Abs(currentDegrees - targetDegrees);
        float distanceAdjustedLeft = Mathf.Abs( (currentDegrees + 360) - targetDegrees);
        float distanceAdjustedRight = Mathf.Abs( (currentDegrees - 360) - targetDegrees);

        RotationDirection shortestDirection;

        // Find the shortest route to look at the target
        if (distanceNormal < distanceAdjustedLeft && distanceNormal < distanceAdjustedRight)
        {
            // Do normal calculation:
            if (targetDegrees > currentDegrees)
            {
                // The target is to the left
                shortestDirection = RotationDirection.Right;
            }
            else
            {
                // The target is to the right
                shortestDirection = RotationDirection.Left;
            }
        }
        else
        {
            if (distanceAdjustedLeft < distanceAdjustedRight)
            {
                shortestDirection = RotationDirection.Left;
            }
            else
            {
                shortestDirection = RotationDirection.Right;
            }
        }

        // Check if the shortest direction is going to put us inside a no-rotate zone
        RotationDirection allowedDirection = shortestDirection;

        // Find the 'rotation amount' between the current degrees and target degrees        
        float rotationDistance = 0f;
        if (shortestDirection == RotationDirection.Right)
        {
            if (currentDegrees < targetDegrees)
            {
                rotationDistance = targetDegrees - currentDegrees;
            }
            else
            {
                rotationDistance = (360f - currentDegrees) + targetDegrees;
            }
        }
        else if (shortestDirection == RotationDirection.Left)
        {
            if (currentDegrees > targetDegrees)
            {
                rotationDistance = currentDegrees - targetDegrees;
            }
            else
            {
                rotationDistance = (360f - targetDegrees) + currentDegrees;
            }
        }

        // Now that the rotation distance has been found, we can find the increment that each of our probes
        // are going to advance by
        int probeCount = 6;
        float probeIncrement = rotationDistance / (float) probeCount;
        bool foundBlock = false;

        // Debug.Log($"Conducting {probeCount} probes with an increment of {probeIncrement}");
        foreach (GunRotationBlocker blocker in _rotationBlockers)
        {
            // Ignore no shoot zones
            if (blocker.type != GunRotationBlocker.RotationBlockType.FullyBlocked)
            {
                continue;
            }

            // For each rotation blocker, check if any of our probes fall within it:
            for (int i = 0; i < probeCount; i++)
            {
                float increment;
                if (allowedDirection == RotationDirection.Left)
                {
                    increment = -i * probeIncrement;
                }
                else
                {
                    increment = i * probeIncrement;
                }

                float probeDegrees = currentDegrees + increment;
                if (probeDegrees >= 360f)
                {
                    probeDegrees -= 360f;
                    // Keep witin 360 degree bounds
                }

                if (blocker.IsWithin(probeDegrees))
                {
                    Debug.Log($"Probe Hit at {probeDegrees}");
                    // Flip direction
                    foundBlock = true;
                    if (allowedDirection == RotationDirection.Left)
                    {
                        allowedDirection = RotationDirection.Right;
                    }
                    else
                    {
                        allowedDirection = RotationDirection.Left;
                    }

                    break;
                }
            }  

            // If we have found a block, break
            if (foundBlock)
            {
                break;
            }
            
        }

        
        // Debug.Log($"Rotation Distance: current: {currentDegrees}, to: {targetDegrees}, distance: {rotationDistance}");
        return allowedDirection;        
    }

    private void OnDrawGizmosSelected() 
    {
        int drawSegments = 128;

        float x;
        float y;
        Vector3 prevPosition = transform.position + new Vector3(0, 0, _gizmoRotationRadius);

        for (int i = 0; i < (drawSegments+1); i++)
        {
            // Figure out where we should draw each segment:
            float degrees = ((float)i / (float) drawSegments) * 360f; 
            float theta =  degrees * Mathf.Deg2Rad;

            x = Mathf.Sin(theta) * _gizmoRotationRadius;
            y = Mathf.Cos(theta) * _gizmoRotationRadius; 

            Vector3 endPosition = transform.position;
            endPosition.z += y;
            endPosition.x += x;

            // Determine color
            foreach (GunRotationBlocker blocker in _rotationBlockers)
            {
                if (blocker.IsWithin(degrees))
                {
                    if (blocker.type == GunRotationBlocker.RotationBlockType.NoShoot)
                    {
                        // No shoot zone, draw yellow
                        Gizmos.color = Color.yellow;
                    }
                    else if (blocker.type == GunRotationBlocker.RotationBlockType.FullyBlocked)
                    {
                        // No turn zone, draw red
                        Gizmos.color = Color.red;
                    }
                    break;
                }
            
                // If it isn't within this segment, set to green:
                Gizmos.color = Color.green;
            }

            // Draw gizmo line
            Gizmos.DrawLine(prevPosition, endPosition);
            prevPosition = endPosition;
        }
        

    }

}

[System.Serializable]
public class GunRotationBlocker
{
    public enum RotationBlockType
    {
        NoShoot,
        FullyBlocked
    }

    [Range(0, 360)] public int from;
    [Range(0, 360)] public int to;
    public RotationBlockType type;

    public bool IsWithin(float degrees)
    {
        if (degrees >= from && degrees <= to)
        {
            return true;
        }
        else
        {
            if (from > to)
            {
                // When from > to, the field of rotation is crossing over the 0 degree mark
                // e.g. from == 360, to == 30.
                if (degrees >= from || degrees <= to)
                {
                    return true;
                }       
            }
            return false;
        }
    }


}

}