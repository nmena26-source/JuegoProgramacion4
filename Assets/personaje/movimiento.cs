using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;

    public float aceleracionEnAire = 10f;

    [Header("Suelo")]
    public Transform puntoSuelo;

    public float radioSuelo = 0.2f;

    public LayerMask capaSuelo;

    [HideInInspector]
    public bool estaEnSuelo;

    [Header("Animacion")]
    public Animator animator;

    [Header("Depuracion")]
    public bool mostrarMensajes = false;

    [Header("Referencias")]
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private float movimientoHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        ComprobarSuelo();

        LeerInput();

        Mover();

        Girar();

        ActualizarAnimacion();
    }

    private void ComprobarSuelo()
    {
        if (puntoSuelo == null)
        {
            estaEnSuelo = false;

            if (mostrarMensajes)
            {
                Debug.LogWarning("Punto Suelo no esta asignado.");
            }

            return;
        }

        estaEnSuelo = Physics2D.OverlapCircle(
            puntoSuelo.position,
            radioSuelo,
            capaSuelo
        );

        if (mostrarMensajes)
        {
            Debug.Log("¿Esta en suelo?: " + estaEnSuelo);
        }
    }

    public bool EstaEnSuelo()
    {
        return estaEnSuelo;
    }

    private void LeerInput()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");
    }

    private void Mover()
    {
        float velocidadObjetivo =
            movimientoHorizontal * velocidadMovimiento;

        float nuevaVelocidadX = rb.linearVelocity.x;

        if (estaEnSuelo)
        {
            nuevaVelocidadX = velocidadObjetivo;
        }
        else
        {
            nuevaVelocidadX = Mathf.MoveTowards(
                nuevaVelocidadX,
                velocidadObjetivo,
                aceleracionEnAire * Time.deltaTime * 10f
            );
        }

        rb.linearVelocity = new Vector2(
            nuevaVelocidadX,
            rb.linearVelocity.y
        );
    }

    private void Girar()
    {
        if (movimientoHorizontal > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (movimientoHorizontal < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void ActualizarAnimacion()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool("EstaEnSuelo", estaEnSuelo);

        bool corriendo =
            estaEnSuelo &&
            Mathf.Abs(movimientoHorizontal) > 0.01f;

        animator.SetBool("Corriendo", corriendo);
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            puntoSuelo.position,
            radioSuelo
        );
    }
}