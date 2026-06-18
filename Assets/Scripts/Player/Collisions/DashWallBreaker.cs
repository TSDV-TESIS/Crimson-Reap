using Player;
using UnityEngine;

public class DashWallBreaker : MonoBehaviour
{
    public float checkRadius = 2.0f; // Subimos el radio para que sea más sensible
    public LayerMask wallLayer;
   
    
    public void Check()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, checkRadius, wallLayer);

        foreach (var hit in hits)
        {
            // BUSCAMOS TU SCRIPT ESPECÍFICO: SimpleFracture
            SimpleFracture wall = hit.GetComponentInParent<SimpleFracture>();
            if (hit.CompareTag("BW_Blocking")) {
             hit.gameObject.SetActive(false);   
            }
            if (wall != null)
            {
                Debug.Log("¡Muro detectado!");
                wall.ExecuteShatter(); // Llamamos a la función que activa los pedazos
            }
        }
    }

    // Esto dibujará una esfera roja en la escena para que veas el alcance del sensor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, checkRadius);
    }
}