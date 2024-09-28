using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Magnet : MonoBehaviour
{
    public int polarity = 1;
    public float pullStrength = 1.5f;
    public float sphereCastRadius = 0.5f;
    RaycastHit hitData;

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hitData, Mathf.Infinity))
        {
            Debug.Log("Hit:" + hitData.collider.gameObject.name);
            Rigidbody tmpRb;
            if (hitData.collider.gameObject.TryGetComponent<Rigidbody>(out tmpRb))
            {
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
        Debug.Log(magneticStrength * pullStrength * polarity);
        magneticObject.AddForce(TargetMe(objectPos).normalized * magneticStrength * pullStrength * polarity, ForceMode.Force);
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
