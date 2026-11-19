using UnityEngine;

public class balaEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    [Min(0.1f)]
    public float velocidad = 8f;

    [Header("Daño")]
    [Min(1)]
    public int daño = 15;

    public string etiquetaJugador = "Player";

    [Header("Vida de la bala")]
    [Min(0.1f)]
    public float tiempoDeVida = 5f;

    [Header("Depuracion")]
    public bool mostrarMensajes = false;

    private Rigidbody2D cuerpo;

    private void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();

        if (cuerpo == null)
        {
            Debug.LogError(
                "La bala necesita un Rigidbody2D.",
                this
            );

            return;
        }

        cuerpo.linearVelocity =
            transform.right * velocidad;

        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag(etiquetaJugador))
        {
            otro.SendMessageUpwards(
                "RecibirDaño",
                daño,
                SendMessageOptions.DontRequireReceiver
            );

            if (mostrarMensajes)
            {
                Debug.Log(
                    "La bala hizo " +
                    daño +
                    " de daño al jugador.",
                    this
                );
            }
        }

        Destroy(gameObject);
    }
}