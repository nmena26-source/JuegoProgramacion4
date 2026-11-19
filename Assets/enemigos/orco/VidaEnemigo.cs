using UnityEngine;
using UnityEngine.UI;

public class VidaEnemigo : MonoBehaviour
{
    [Header("Vida")]
    [Min(1)]
    public int vidaMaxima = 120;

    [Header("Barra de vida")]
    [Tooltip("Arrastra aqui el Slider BarraVidaEnemigo.")]
    public Slider barraVida;

    [Tooltip("Oculta la barra cuando el enemigo muere.")]
    public bool ocultarBarraAlMorir = true;

    [Header("Animacion")]
    public Animator animator;
    public string triggerMuerte = "muerte";

    [Header("Muerte")]
    [Min(0f)]
    public float tiempoAntesDeDestruir = 1.5f;

    public bool destruirAlMorir = true;

    [Tooltip("Arrastra los scripts de movimiento, ataque y deteccion.")]
    public MonoBehaviour[] scriptsADesactivarAlMorir;

    [Header("Depuracion")]
    public bool mostrarMensajes = true;

    private int vidaActual;
    private bool estaMuerto;

    private void Awake()
    {
        vidaMaxima = Mathf.Max(1, vidaMaxima);
        vidaActual = vidaMaxima;
        estaMuerto = false;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        ConfigurarBarraVida();
    }

    private void ConfigurarBarraVida()
    {
        if (barraVida == null)
        {
            Debug.LogWarning(
                "Falta asignar el Slider de vida en " + gameObject.name,
                this
            );

            return;
        }

        barraVida.interactable = false;
        barraVida.minValue = 0;
        barraVida.maxValue = vidaMaxima;
        barraVida.wholeNumbers = true;

        barraVida.gameObject.SetActive(true);

        ActualizarBarraVida();
    }

    private void ActualizarBarraVida()
    {
        if (barraVida == null)
        {
            return;
        }

        barraVida.SetValueWithoutNotify(vidaActual);
    }

    public void RecibirDaño(int cantidad)
    {
        if (mostrarMensajes)
        {
            Debug.Log(
                gameObject.name +
                " recibio una llamada de daño. Cantidad: " +
                cantidad +
                ". Muerto: " +
                estaMuerto,
                this
            );
        }

        if (estaMuerto || cantidad <= 0)
        {
            return;
        }

        vidaActual = Mathf.Clamp(
            vidaActual - cantidad,
            0,
            vidaMaxima
        );

        ActualizarBarraVida();

        if (mostrarMensajes)
        {
            Debug.Log(
                gameObject.name +
                " - Vida restante: " +
                vidaActual +
                "/" +
                vidaMaxima,
                this
            );
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void Curar(int cantidad)
    {
        if (estaMuerto || cantidad <= 0)
        {
            return;
        }

        vidaActual = Mathf.Clamp(
            vidaActual + cantidad,
            0,
            vidaMaxima
        );

        ActualizarBarraVida();
    }

    public int ObtenerVidaActual()
    {
        return vidaActual;
    }

    public int ObtenerVidaMaxima()
    {
        return vidaMaxima;
    }

    public bool EstaMuerto()
    {
        return estaMuerto;
    }

    private void Morir()
    {
        if (estaMuerto)
        {
            return;
        }

        estaMuerto = true;

        if (mostrarMensajes)
        {
            Debug.Log(
                gameObject.name + " murio.",
                this
            );
        }

        if (ocultarBarraAlMorir && barraVida != null)
        {
            barraVida.gameObject.SetActive(false);
        }

        DesactivarComportamientos();
        DetenerFisica();
        DesactivarColliders();
        ReproducirMuerte();

        if (destruirAlMorir)
        {
            Destroy(gameObject, tiempoAntesDeDestruir);
        }
    }

    private void DesactivarComportamientos()
    {
        if (scriptsADesactivarAlMorir == null)
        {
            return;
        }

        foreach (MonoBehaviour script in scriptsADesactivarAlMorir)
        {
            if (script == null || script == this)
            {
                continue;
            }

            script.StopAllCoroutines();
            script.CancelInvoke();
            script.enabled = false;
        }
    }

    private void DetenerFisica()
    {
        Rigidbody2D cuerpo = GetComponent<Rigidbody2D>();

        if (cuerpo == null)
        {
            return;
        }

        cuerpo.linearVelocity = Vector2.zero;
        cuerpo.angularVelocity = 0f;
        cuerpo.simulated = false;
    }

    private void DesactivarColliders()
    {
        Collider2D[] colliders =
            GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
    }

    private void ReproducirMuerte()
    {
        if (animator == null)
        {
            Debug.LogWarning(
                "Falta asignar el Animator en " + gameObject.name,
                this
            );

            return;
        }

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning(
                "El Animator no tiene un Controller asignado.",
                this
            );

            return;
        }

        foreach (AnimatorControllerParameter parametro in animator.parameters)
        {
            if (
                parametro.name == triggerMuerte &&
                parametro.type == AnimatorControllerParameterType.Trigger
            )
            {
                animator.SetTrigger(triggerMuerte);
                return;
            }
        }

        Debug.LogWarning(
            "No existe un parametro Trigger llamado '" +
            triggerMuerte +
            "' en el Animator del enemigo.",
            this
        );
    }
}