using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarAlMorir : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerHealth vidaJugador;

    [Header("Reinicio")]
    public float tiempoAntesDeReiniciar = 2f;

    public bool reiniciarEscenaActual = true;

    public string nombreEscena = "NombreDeTuEscena";

    private bool reinicioProgramado = false;

    private void Awake()
    {
        if (vidaJugador == null)
        {
            vidaJugador = GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (vidaJugador == null)
        {
            return;
        }

        if (
            vidaJugador.IsDead &&
            !reinicioProgramado
        )
        {
            reinicioProgramado = true;

            Invoke("ReiniciarEscena", tiempoAntesDeReiniciar);
        }
    }

    private void ReiniciarEscena()
    {
        if (reiniciarEscenaActual)
        {
            string escenaActual =
                SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(escenaActual);
        }
        else
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}