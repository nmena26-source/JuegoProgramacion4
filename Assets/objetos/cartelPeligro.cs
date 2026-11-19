using UnityEngine;
using System.Collections;

public class CartelPeligro : MonoBehaviour
{
    [Header("Cuadro de texto")]
    public GameObject cuadroTexto;

    [Header("Aparicion suave")]
    [Min(0.05f)]
    public float duracionAparicion = 0.5f;

    [Min(0.05f)]
    public float duracionDesaparicion = 0.5f;

    [Header("Jugador")]
    public string etiquetaJugador = "Player";

    [Header("Depuracion")]
    public bool mostrarMensajes = false;

    private CanvasGroup canvasGroup;
    private Coroutine animacionActual;

    private void Start()
    {
        if (cuadroTexto == null)
        {
            return;
        }

        canvasGroup = cuadroTexto.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = cuadroTexto.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        cuadroTexto.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag(etiquetaJugador))
        {
            return;
        }

        MostrarCuadro();

        if (mostrarMensajes)
        {
            Debug.Log(
                "El jugador esta cerca del cartel.",
                this
            );
        }
    }

    private void OnTriggerExit2D(Collider2D otro)
    {
        if (!otro.CompareTag(etiquetaJugador))
        {
            return;
        }

        OcultarCuadro();

        if (mostrarMensajes)
        {
            Debug.Log(
                "El jugador se alejo del cartel.",
                this
            );
        }
    }

    private void MostrarCuadro()
    {
        if (cuadroTexto == null)
        {
            return;
        }

        if (animacionActual != null)
        {
            StopCoroutine(animacionActual);
        }

        cuadroTexto.SetActive(true);

        animacionActual = StartCoroutine(
            CambiarTransparencia(1f, duracionAparicion)
        );
    }

    private void OcultarCuadro()
    {
        if (cuadroTexto == null)
        {
            return;
        }

        if (animacionActual != null)
        {
            StopCoroutine(animacionActual);
        }

        animacionActual = StartCoroutine(
            CambiarTransparencia(0f, duracionDesaparicion)
        );
    }

    private IEnumerator CambiarTransparencia(
        float alphaFinal,
        float duracion
    )
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        float alphaInicial = canvasGroup.alpha;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                alphaInicial,
                alphaFinal,
                tiempo / duracion
            );

            yield return null;
        }

        canvasGroup.alpha = alphaFinal;

        if (alphaFinal <= 0f)
        {
            cuadroTexto.SetActive(false);
        }
    }
}