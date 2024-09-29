using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class Magnet : MonoBehaviour
{
    public GameObject magnet;

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
    public bool magnetizing = false;
    public bool holding = false;
    public int magnetizingLength;
    public int magnetizingCount = 0;
    

    private Renderer magnetMaterial;
    private Color ogColor;
    public Movement1 movement;


    private void Start()
    {
        magnetMaterial = GetComponent<Renderer>();
        ogColor = magnetMaterial.material.color;
    }
    // Update is called once per frame
    void Update()
    {
        if (movement.controllable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                magnetizing = true;
            }

            
        }

        if (magnetizing)
        {
            magnetizingCount++;
            magnetMaterial.material.color = Color.red;
            if (Physics.SphereCast(magnet.transform.position, sphereCastRadius, Vector3.down, out hitData, Mathf.Infinity))
            {
                Debug.Log(transform.position);
                //Debug.Log("Hit:" + hitData.collider.gameObject.name);
                Rigidbody tmpRb;
                if (hitData.collider.gameObject.TryGetComponent<Rigidbody>(out tmpRb))
                {

                    Debug.Log("FLAG = " + hitData.collider.gameObject.TryGetComponent<Rigidbody>(out tmpRb));
                    if (!gameObjects.Contains(tmpRb.gameObject))
                    {
                        gameObjects.Add(tmpRb.gameObject);
                    }
                    Magnetic magnetic;
                    if (hitData.collider.TryGetComponent<Magnetic>(out magnetic))
                    {
                        magnetic.magnetized = true;
                        Debug.Log("magnetized");
                        /*
                        if (magnetic.magnetized)
                        {

                            Magnetize(tmpRb, hitData.collider.transform.position, polarity, magnetic.magneticStrength);
                        }

                        */
                    }
                }
            }

        }
        else
        {
            magnetMaterial.material.color = ogColor;
        }



    }

    public void Magnetize(GameObject magnetObject, Rigidbody magneticObject, Vector3 objectPos, int polarity, float magneticStrength)
    {
        float distance = Vector3.Distance(magnet.transform.position, objectPos);
        if (distance < maxDistance)
        {
            //float tDistance = Mathf.InverseLerp(maxDistance, 0f, distance); // Give a decimal representing how far between 0 distance and max distance.
            
            float scaledForce = pullStrength * polarity;
            magneticObject.AddForce((magnetObject.transform.position - objectPos) * scaledForce, ForceMode.Force);

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
    public Vector3 TargetMe(GameObject magnet, Vector3 objectPos)
    {
        return magnet.transform.position - objectPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Debug.Log("collided wit mag");
            collision.rigidbody.useGravity = false;
            collision.rigidbody.velocity = Vector3.zero;

            collision.rigidbody.angularVelocity = Vector3.zero;
            //Vector3 scale = collision.transform.localScale;
            collision.rigidbody.freezeRotation = true;
            collision.rigidbody.isKinematic = false;
            collision.gameObject.GetComponent<Magnetic>().stuck = true;

            Debug.Log("stuck = " + collision.gameObject.GetComponent<Magnetic>().stuck);

            //collision.transform.localScale = scale; 
            

        }
    }
}
