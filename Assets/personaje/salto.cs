using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SaltoJugador : MonoBehaviour
{
    [Header("Salto")]
    public float fuerzaSalto = 12f;

    [Range(0.2f, 0.8f)]
    public float multiplicadorSaltoCorto = 0.45f;

    [Header("Gravedad")]
    public float gravedadSubiendo = 2.2f;

    public float gravedadCayendo = 3.2f;

    public float gravedadEnElPico = 1.2f;

    public float velocidadMaximaDeCaida = 18f;

    [Header("Perdon de salto")]
    public float tiempoCoyote = 0.12f;

    public float tiempoBufferSalto = 0.15f;

    [Header("Referencias")]
    public MovimientoJugador movimientoJugador;

    [Header("Depuracion")]
    public bool mostrarMensajes = false;

    private Rigidbody2D rb;

    private float gravedadOriginal;

    private float tiempoSinSuelo = 999f;

    private float tiempoDesdeUltimoSalto = 999f;

    private bool puedeCortarSalto;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        gravedadOriginal = rb.gravityScale;

        if (movimientoJugador == null)
        {
            movimientoJugador = GetComponent<MovimientoJugador>();
        }
    }

    private void Update()
    {
        ActualizarTiempos();

        Saltar();

        ControlarGravedad();

        CortarSalto();
    }

    private void ActualizarTiempos()
    {
        bool estaEnSuelo = false;

        if (movimientoJugador != null)
        {
            estaEnSuelo = movimientoJugador.estaEnSuelo;
        }

        if (estaEnSuelo)
        {
            tiempoSinSuelo = 0f;

            puedeCortarSalto = false;
        }
        else
        {
            tiempoSinSuelo += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tiempoDesdeUltimoSalto = 0f;
        }
        else
        {
            tiempoDesdeUltimoSalto += Time.deltaTime;
        }
    }

    private void Saltar()
    {
        bool puedeSaltar =
            tiempoSinSuelo <= tiempoCoyote &&
            tiempoDesdeUltimoSalto <= tiempoBufferSalto;

        if (puedeSaltar)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            tiempoSinSuelo = tiempoCoyote + 1f;

            tiempoDesdeUltimoSalto = tiempoBufferSalto + 1f;

            puedeCortarSalto = true;

            if (mostrarMensajes)
            {
                Debug.Log("Salto ejecutado.");
            }
        }
    }

    private void ControlarGravedad()
    {
        float velocidadVertical = rb.linearVelocity.y;

        if (velocidadVertical > 2f)
        {
            rb.gravityScale = gravedadSubiendo;
        }
        else if (
            velocidadVertical <= 2f &&
            velocidadVertical >= -2f
        )
        {
            rb.gravityScale = gravedadEnElPico;
        }
        else
        {
            rb.gravityScale = gravedadCayendo;
        }

        if (rb.linearVelocity.y < -velocidadMaximaDeCaida)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                -velocidadMaximaDeCaida
            );
        }
    }

    private void CortarSalto()
    {
        if (
            Input.GetKeyUp(KeyCode.Space) &&
            puedeCortarSalto &&
            rb.linearVelocity.y > 0f
        )
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * multiplicadorSaltoCorto
            );

            puedeCortarSalto = false;
        }
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.gravityScale = gravedadOriginal;
        }
    }
}