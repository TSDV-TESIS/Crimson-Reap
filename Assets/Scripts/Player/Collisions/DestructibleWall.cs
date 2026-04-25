using UnityEngine;
using System.Collections;
using Utils;

public class DestructibleWall : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject fracturedWall;
    private BlockColliderHider _hider;

    [Header("Configuración")]
    public float explosionForce = 500f;
    public float cleanUpDelay = 4f;

    void Awake()
    {
        _hider = GetComponent<BlockColliderHider>();
        if (fracturedWall != null) fracturedWall.SetActive(false);
    }

    public void Shatter(Vector3 impactPoint)
    {
        if (_hider != null) _hider.Hide();
        else gameObject.SetActive(false);

        if (fracturedWall != null)
        {
            fracturedWall.SetActive(true);
            fracturedWall.transform.SetParent(null); 
            foreach (Rigidbody rb in fracturedWall.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(explosionForce, impactPoint, 3f);
            }

            StartCoroutine(CleanUp(fracturedWall));
        }
    }

    IEnumerator CleanUp(GameObject obj)
    {
        yield return new WaitForSeconds(cleanUpDelay);
        // recorda meter el shader de disolución, ema
        Destroy(obj);
        Destroy(gameObject);
    }
}