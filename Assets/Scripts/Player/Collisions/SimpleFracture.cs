using UnityEngine;

public class SimpleFracture : MonoBehaviour
{

    public GameObject fracturedMesh;
    public float explosionForce = 400f;

    public void ExecuteShatter()
    {


        GetComponent<Collider>().enabled = false;

        if (fracturedMesh)
        {

            fracturedMesh.transform.SetParent(null);

            foreach (Rigidbody rb in fracturedMesh.GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
                rb.AddForce(explosionForce * (rb.transform.position-transform.position),ForceMode.Impulse);
            }
            Destroy(fracturedMesh, 5f);
        }


        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;


        Destroy(gameObject, 0.1f);
    }
}