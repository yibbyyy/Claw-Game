using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public float magneticStrength = 1.0f;
    public float maxMagneticStrngth = 10f;
    Rigidbody rb;

    private GameObject magnetObject;
    private Magnet magnet;
    public bool magnetized = false;
    public bool stuck = false;
    private int polarity = 1;

    private float maxDistance = .25f;
    


    private void Awake()
    {
        magnetObject = GameObject.FindGameObjectWithTag("magnet");
        magnet = magnetObject.GetComponent<Magnet>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
        if (magnetized)
        {
            Debug.Log("magnetized = ");
            magnet.Magnetize(magnetObject, rb, transform.position, polarity, magneticStrength);
        }

        //rb.useGravity = true;
        if (Vector3.Distance(transform.position, magnetObject.transform.position) > maxDistance && stuck)
        {
            rb.useGravity = true;
            //rb.isKinematic = true;
            magnetized = false;
        }
        
        if (stuck)
        {
            stuck = false;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.lossyScale;

            transform.SetParent(magnetObject.transform, false);

            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
        
    }
}
