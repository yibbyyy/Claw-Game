using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Magnet : MonoBehaviour
{
    public int polarity = 1;
    public float pullStrength = 1.5f;
    public float sphereCastRadius = 0.25f;
    public float maxForce = 100f;  // Maximum force to avoid too strong pulls
    public float distanceExponent = 3f;
    public float dampingFactor = .1f;
    public float maxDistance = 5f;
    public float maxStrength = 5f;
    RaycastHit hitData;
    HashSet<GameObject> gameObjects = new();
    
    

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hitData, Mathf.Infinity))
        {
            //Debug.Log("Hit:" + hitData.collider.gameObject.name);
            Rigidbody tmpRb;
            if (hitData.collider.gameObject.TryGetComponent<Rigidbody>(out tmpRb))
            {
                if (!gameObjects.Contains(tmpRb.gameObject))
                {
                    gameObjects.Add(tmpRb.gameObject);
                }
                Magnetic magnetic;
                if (hitData.collider.TryGetComponent<Magnetic>(out magnetic))
                {
                    Magnetize(tmpRb, hitData.collider.transform.position, polarity, magnetic.magneticStrength);
                }
            }
        }
        

    }

    void Magnetize(Rigidbody magneticObject, Vector3 objectPos, int polarity, float magneticStrength)
    {
        float distance = Vector3.Distance(transform.position, objectPos);

        if (distance < maxDistance)
        {
            float tDistance = Mathf.InverseLerp(maxDistance, 0f, distance); // Give a decimal representing how far between 0 distance and max distance.
            float strength = Mathf.Lerp(0f, maxStrength, tDistance);
            float scaledForce = magneticStrength * strength * polarity;
            magneticObject.AddForce(TargetMe(hitData.collider.transform.position) * scaledForce, ForceMode.Force);

        }

        /*
        //distance = Mathf.Max(minDistance, distance);

        //float distanceFactor = magneticObject.mass / Mathf.Pow(distance, distanceExponent) + 1;

        //float scaledForce = magneticStrength * pullStrength * polarity * distanceFactor;
        //scaledForce = Mathf.Clamp(scaledForce, 0, maxForce);

        //scaledForce *= 2;
        scaledForce = Mathf.Max(scaledForce, pastVelocity);
        Debug.Log("Applying Force: " + scaledForce + "With distance: " + distance);
        magneticObject.AddForce(TargetMe(objectPos).normalized * scaledForce, ForceMode.Force);
        pastVelocity = magneticObject.velocity.magnitude;
        float stickDistance = minDistance * 10;
       
        // Aggressive damping as object gets closer to magnet
        if (distance <= minDistance)
        {
            Debug.Log("Damping applied");
            // Reduce velocity significantly as object approaches stick distance
            //magneticObject.velocity *= dampingFactor;
            //magneticObject.velocity = pastVelocity;
            // If velocity is low enough, stop the object entirely
            if (magneticObject.velocity.magnitude <= velocityThreshold)
            {
                //magneticObject.velocity = Vector3.zero;  // Stop movement completely
                //magneticObject.useGravity = false;       // Disable gravity to prevent bouncing
            }
        }
        else
        {
            // Enable gravity when far from magnet
            //magneticObject.useGravity = true;
        }*/
    }

    /// <summary>
    /// Get Vector direction of any other transform pointed towards this game object
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    Vector3 TargetMe(Vector3 objectPos)
    {
        return this.transform.position - objectPos;
    }
}
