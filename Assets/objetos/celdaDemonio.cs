using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarTextoSprite : MonoBehaviour
{
    [Header("Cuadro de texto")]
    public GameObject cuadroTexto;

    [Header("Tiempos de transicion")]
    [Min(0f)] public float tiempoAparicion = 0.5f;
    [Min(0f)] public float tiempoDesaparicion = 0.5f;

    [Header("Configuracion")]
    public string etiquetaJugador = "Player";
    public bool mostrarMensajes = false;

    private CanvasGroup grupo;
    private Coroutine transicion;

    private readonly HashSet<Collider2D> collidersJugador =
        new HashSet<Collider2D>();

    private void Awake()
    {
        if (cuadroTexto == null)
        {
            Debug.LogWarning(
                "Asigna el cuadro de texto en MostrarTextoSprite.",
                this
            );

            enabled = false;
            return;
        }

        grupo = cuadroTexto.GetComponent<CanvasGroup>();

        if (grupo == null)
        {
            grupo = cuadroTexto.AddComponent<CanvasGroup>();
        }

        grupo.alpha = 0f;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;

        cuadroTexto.SetActive(false);
    }

    private bool EsJugador(Collider2D other)
    {
        if (other.CompareTag(etiquetaJugador))
        {
            return true;
        }

        return other.attachedRigidbody != null &&
               other.attachedRigidbody.CompareTag(etiquetaJugador);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!EsJugador(other))
        {
            return;
        }

        if (!collidersJugador.Add(other))
        {
            return;
        }

        if (collidersJugador.Count == 1)
        {
            CambiarVisibilidad(true);

            if (mostrarMensajes)
            {
                Debug.Log("El jugador se acerco al sprite.", this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!collidersJugador.Remove(other))
        {
            return;
        }

        if (collidersJugador.Count == 0)
        {
            CambiarVisibilidad(false);

            if (mostrarMensajes)
            {
                Debug.Log("El jugador se alejo del sprite.", this);
            }
        }
    }

    private void CambiarVisibilidad(bool mostrar)
    {
        if (grupo == null || cuadroTexto == null)
        {
            return;
        }

        if (transicion != null)
        {
            StopCoroutine(transicion);
            transicion = null;
        }

        float destino = mostrar ? 1f : 0f;
        float duracion = mostrar
            ? tiempoAparicion
            : tiempoDesaparicion;

        if (mostrar)
        {
            cuadroTexto.SetActive(true);
        }

        if (duracion <= 0f)
        {
            grupo.alpha = destino;
            cuadroTexto.SetActive(mostrar);
            return;
        }

        transicion = StartCoroutine(
            AnimarTransparencia(destino, duracion, mostrar)
        );
    }

    private IEnumerator AnimarTransparencia(
        float destino,
        float duracion,
        bool mostrar
    )
    {
        float inicio = grupo.alpha;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;

            grupo.alpha = Mathf.Lerp(
                inicio,
                destino,
                Mathf.Clamp01(tiempo / duracion)
            );

            yield return null;
        }

        grupo.alpha = destino;

        if (!mostrar)
        {
            cuadroTexto.SetActive(false);
        }

        transicion = null;
    }

    private void OnDisable()
    {
        if (transicion != null)
        {
            StopCoroutine(transicion);
            transicion = null;
        }

        collidersJugador.Clear();

        if (grupo != null)
        {
            grupo.alpha = 0f;
        }

        if (cuadroTexto != null)
        {
            cuadroTexto.SetActive(false);
        }
    }
}