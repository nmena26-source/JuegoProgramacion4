using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    [Header("Objetivo")]
    public string etiquetaJugador = "Player";

    [Header("Disparo")]
    public GameObject balaPrefab;

    public Transform puntoDeDisparo;

    [Min(0.1f)]
    public float tiempoEntreDisparos = 1.5f;

    [Header("Deteccion")]
    [Min(0.1f)]
    public float distanciaMaximaParaDisparar = 10f;

    public bool necesitaVerAlJugador = true;

    [Header("Depuracion")]
    public bool mostrarMensajes = false;

    private Transform jugador;
    private float temporizador;

    private void Start()
    {
        temporizador = 0f;
    }

    private void Update()
    {
        if (balaPrefab == null || puntoDeDisparo == null)
        {
            return;
        }

        if (jugador == null)
        {
            GameObject jugadorEncontrado =
                GameObject.FindGameObjectWithTag(etiquetaJugador);

            if (jugadorEncontrado != null)
            {
                jugador = jugadorEncontrado.transform;
            }
        }

        if (jugador == null)
        {
            return;
        }

        float distancia =
            Vector2.Distance(transform.position, jugador.position);

        if (distancia > distanciaMaximaParaDisparar)
        {
            return;
        }

        if (necesitaVerAlJugador)
        {
            if (!PuedeVerAlJugador())
            {
                return;
            }
        }

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreDisparos)
        {
            Disparar();
            temporizador = 0f;
        }
    }

    private bool PuedeVerAlJugador()
    {
        Vector2 direccion =
            jugador.position - puntoDeDisparo.position;

        float distancia =
            Vector2.Distance(
                transform.position,
                jugador.position
            );

        RaycastHit2D golpe = Physics2D.Raycast(
            puntoDeDisparo.position,
            direccion.normalized,
            distancia
        );

        if (golpe.collider == null)
        {
            return false;
        }

        return golpe.collider.CompareTag(etiquetaJugador);
    }

    private void Disparar()
    {
        GameObject bala =
            Instantiate(
                balaPrefab,
                puntoDeDisparo.position,
                puntoDeDisparo.rotation
            );

        balaEnemigo scriptBala =
            bala.GetComponent<balaEnemigo>();

        if (scriptBala != null)
        {
            scriptBala.etiquetaJugador = etiquetaJugador;

            if (mostrarMensajes)
            {
                Debug.Log(
                    gameObject.name + " disparo una bala.",
                    this
                );
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoDeDisparo == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaMaximaParaDisparar
        );
    }
}