using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class Magnet1 : MonoBehaviour
{
    public int polarity = 1;
    public float pullStrength = 1.5f;
    public float sphereCastRadius = 0.25f;
    public float maxForce = 100f;  // Maximum force to avoid too strong pulls
    public float distanceExponent = 3f;
    public float dampingFactor = .1f;
    public float maxDistance = 5f;
    public float stickDistance = .5f;  // Distance at which the object sticks to the magnet
    public float maxStrength = 5f;

    RaycastHit hitData;
    HashSet<GameObject> gameObjects = new();
    HashSet<Rigidbody> stuckObjects = new();  // List to store stuck objects

    public bool magnetizing = false;
    private Renderer magnetMaterial;
    private Color ogColor;
    public Movement1 movement;

    private void Start()
    {
        magnetMaterial = GetComponent<Renderer>();
        ogColor = magnetMaterial.material.color;
    }

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
            magnetMaterial.material.color = Color.red;

            // Detect objects to magnetize
            if (Physics.SphereCast(transform.position + (Vector3.down * .5f), sphereCastRadius, Vector3.down, out hitData, Mathf.Infinity))
            {
                Rigidbody tmpRb;
                if (hitData.collider.gameObject.TryGetComponent<Rigidbody>(out tmpRb))
                {
                    Debug.Log(hitData.collider.gameObject.name);
                    if (!gameObjects.Contains(tmpRb.gameObject))
                    {
                        gameObjects.Add(tmpRb.gameObject);
                    }

                    Magnetic1 magnetic;
                    if (hitData.collider.TryGetComponent<Magnetic1>(out magnetic))
                    {
                        // Magnetize the object
                        Debug.Log(tmpRb);
                        Magnetize(tmpRb, hitData.collider.transform.position, polarity, magnetic.magneticStrength);
                    }
                }
            }
        }
        else
        {
            // Release all stuck objects when magnetizing is turned off
            ReleaseStuckObjects();
            magnetMaterial.material.color = ogColor;
        }
    }

    void Magnetize(Rigidbody magneticObject, Vector3 objectPos, int polarity, float magneticStrength)
    {
        float distance = Vector3.Distance(transform.position, objectPos);

        // Magnetizing objects if within maxDistance
        if (distance < maxDistance)
        {
            float tDistance = Mathf.InverseLerp(maxDistance, 0f, distance);
            //float strength = Mathf.Lerp(0f, maxStrength, tDistance);
            float strength = 1 / Mathf.Pow(tDistance, 2);
            float scaledForce = magneticStrength * strength * polarity * pullStrength;
            Debug.Log("distance =" + distance);
            Debug.Log("objectPos = " + objectPos);
            Debug.Log("object = " + magneticObject);
            if (distance > stickDistance)  // Pull object toward magnet if not close enough to stick
            {
                Debug.Log("final vector3 = " + TargetMe(objectPos) * scaledForce);                            
                magneticObject.AddForce(TargetMe(objectPos) * scaledForce, ForceMode.Force);
            }
            if (distance < stickDistance && !stuckObjects.Contains(magneticObject))
            {
                Debug.Log("Stuck");
                StickObject(magneticObject);
            }
            
        }
    }

    // Method to make objects stick to the magnet
    // Method to make objects stick to the magnet
    void StickObject(Rigidbody magneticObject)
    {
        // Stop the object's movement and disable gravity
        magneticObject.velocity = Vector3.zero;
        magneticObject.useGravity = false;
        magneticObject.isKinematic = true;

        // Create an empty "holder" GameObject at the magnet's position
        GameObject holder = new GameObject("MagnetHolder");
        holder.transform.position = transform.position;  // Place exactly at the magnet's location
        holder.transform.rotation = transform.rotation;  // Match rotation
        holder.transform.SetParent(this.transform, true);  // Set the holder as a child of the magnet

        // Set the magnetic object to match the exact position of the magnet or adjust its local position
        magneticObject.transform.SetParent(holder.transform, true);
        magneticObject.transform.localPosition = Vector3.down * 2;  // Ensure object is centered relative to the magnet

        stuckObjects.Add(magneticObject);  // Add object to stuckObjects list
    }



    // Method to release all stuck objects
    void ReleaseStuckObjects()
    {
        foreach (Rigidbody stuckObject in stuckObjects)
        {
            stuckObject.isKinematic = false;  // Re-enable physics interaction
            stuckObject.useGravity = true;
            stuckObject.transform.SetParent(null);  // Unparent the object so it no longer follows the magnet
        }
        stuckObjects.Clear();  // Clear the list of stuck objects
    }

    // Get Vector direction of any other transform pointed towards this game object
    Vector3 TargetMe(Vector3 objectPos)
    {
        return this.transform.position - objectPos;
    }
}

