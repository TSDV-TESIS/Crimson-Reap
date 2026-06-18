using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFracture : MonoBehaviour
{
    [Header("Configuración Física")]
    public GameObject fracturedMesh;
    public float explosionForce = 400f;

    [Header("Configuración del Shader (Dissolve)")]
    [Tooltip("Tiempo en segundos que tarda en ponerse negro antes de disolverse.")]
    public float timeToTurnBlack = 0.5f;

    [Tooltip("Tiempo en segundos que tarda en desaparecer completamente.")]
    public float dissolveDuration = 2.0f;

    // Nombres internos de las variables del Shader (Verifica esto en tu Shader Graph)
    // Si en tu Shader Graph la "Reference" tiene guion bajo al principio, déjalo así.
    private readonly string prop_IsBreak = "_Is_Break_Wall";
    private readonly string prop_GradientHeight = "_Dissolve_GradientHeight";
    private readonly string prop_Amount = "_Dissolve_Amount";

    public void ExecuteShatter()
    {
        // 1. Desactivamos el collider original para que el jugador pueda pasar de inmediato
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;

        if (fracturedMesh)
        {
            // 2. Soltamos la pared rota
            fracturedMesh.transform.SetParent(null);

            // 3. Física: Aplicamos fuerza a los pedazos
            Rigidbody[] rbs = fracturedMesh.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = false;
                rb.AddForce(explosionForce * (rb.transform.position - transform.position), ForceMode.Impulse);
            }

            // 4. Iniciamos el Efecto Visual
            // Buscamos todos los renderers de los pedazos para aplicar el shader
            Renderer[] renderers = fracturedMesh.GetComponentsInChildren<Renderer>();

            if (renderers.Length > 0)
            {
                // Usamos un componente auxiliar para que la corrutina no muera cuando se destruya este script
                DissolveController controller = fracturedMesh.AddComponent<DissolveController>();
                controller.StartDissolve(renderers, timeToTurnBlack, dissolveDuration);
            }

            // 5. Destruimos el objeto original (la pared entera)
            // Los pedazos se destruirán solos al terminar el efecto
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

// --- CLASE AUXILIAR (Se encarga de la animación del material) ---
// Esta clase vive en los pedazos y se asegura de que el efecto se complete.
public class DissolveController : MonoBehaviour
{
    // IDs de las propiedades para acceder rápido al shader
    private static readonly int ID_IsBreak = Shader.PropertyToID("_Is_Break_Wall");
    private static readonly int ID_GradientHeight = Shader.PropertyToID("_Dissolve_GradientHeight");
    private static readonly int ID_Amount = Shader.PropertyToID("_Dissolve_Amount");

    private Renderer[] _renderers;
    private MaterialPropertyBlock _propBlock;

    public void StartDissolve(Renderer[] renderers, float blackTime, float dissolveTime)
    {
        _renderers = renderers;
        _propBlock = new MaterialPropertyBlock();
        StartCoroutine(DissolveRoutine(blackTime, dissolveTime));
    }

    private IEnumerator DissolveRoutine(float blackTime, float dissolveTime)
    {
        // --- FASE 1: ACTIVAR Y PONER NEGRO ---
        // Establecemos Is_Break_Wall = 1 y animamos GradientHeight de 2 a 0

        float timer = 0f;

        // Configuración Inicial
        foreach (Renderer r in _renderers)
        {
            r.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(ID_IsBreak, 1); // Activamos el efecto
            _propBlock.SetFloat(ID_GradientHeight, 2f); // Valor inicial
            _propBlock.SetFloat(ID_Amount, -0.6f); // Valor inicial
            r.SetPropertyBlock(_propBlock);
        }

        // Animación de "ennegrecimiento"
        while (timer < blackTime)
        {
            timer += Time.deltaTime;
            float progress = timer / blackTime;

            foreach (Renderer r in _renderers)
            {
                r.GetPropertyBlock(_propBlock);
                // Interpolamos de 2 a 0
                _propBlock.SetFloat(ID_GradientHeight, Mathf.Lerp(2f, 0f, progress));
                r.SetPropertyBlock(_propBlock);
            }
            yield return null; // Esperamos un frame
        }

        // --- FASE 2: DISOLUCIÓN REAL ---
        // Animamos Dissolve_Amount de -0.6 a 0.6

        timer = 0f;
        float startAmount = -0.6f;
        float endAmount = 0.6f;

        while (timer < dissolveTime)
        {
            timer += Time.deltaTime;
            float progress = timer / dissolveTime;

            foreach (Renderer r in _renderers)
            {
                r.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat(ID_Amount, Mathf.Lerp(startAmount, endAmount, progress));
                r.SetPropertyBlock(_propBlock);
            }
            yield return null;
        }

        // --- FASE 3: LIMPIEZA ---
        // Destruimos los pedazos después de que termine el efecto
        Destroy(gameObject);
    }
}