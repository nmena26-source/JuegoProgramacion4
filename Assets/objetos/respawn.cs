using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Posicion de reaparicion")]
    public Transform puntoReaparicion;

    [Header("Depuracion")]
    public bool mostrarMensajes = true;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RespawnJugador jugador = null;

        if (other.attachedRigidbody != null)
        {
            jugador = other.attachedRigidbody
                .GetComponent<RespawnJugador>();
        }

        if (jugador == null)
        {
            jugador = other.GetComponentInParent<RespawnJugador>();
        }

        if (jugador == null)
        {
            return;
        }

        if (puntoReaparicion == null)
        {
            Debug.LogWarning(
                "Falta asignar Punto Reaparicion en " + gameObject.name,
                this
            );

            return;
        }

        jugador.GuardarCheckpoint(puntoReaparicion.position);

        if (mostrarMensajes)
        {
            Debug.Log(
                "Checkpoint activado: " + gameObject.name,
                this
            );
        }
    }
}