using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RespawnJugador : MonoBehaviour
{
    [Header("Depuracion")]
    public bool mostrarMensajes = true;

    private Rigidbody2D cuerpo;
    private Vector3 posicionReaparicion;
    private Transform padreOriginal;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();

        posicionReaparicion = transform.position;
        padreOriginal = transform.parent;
    }

    public void GuardarCheckpoint(Vector3 nuevaPosicion)
    {
        posicionReaparicion = nuevaPosicion;

        if (mostrarMensajes)
        {
            Debug.Log(
                "Checkpoint guardado en: " + posicionReaparicion,
                this
            );
        }
    }

    [ContextMenu("Probar regreso al checkpoint")]
    public void Reaparecer()
    {
        // Evita reaparecer como hijo de una plataforma movil.
        transform.SetParent(padreOriginal, true);

        cuerpo.linearVelocity = Vector2.zero;
        cuerpo.angularVelocity = 0f;

        transform.position = posicionReaparicion;
        cuerpo.position = new Vector2(
            posicionReaparicion.x,
            posicionReaparicion.y
        );

        if (mostrarMensajes)
        {
            Debug.Log("Jugador devuelto al checkpoint.", this);
        }
    }
}