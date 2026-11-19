using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Escena del juego")]
    [Tooltip("Nombre exacto de la escena del nivel, sin .unity.")]
    public string nombreEscenaJuego;

    private bool cargando;

    public void Jugar()
    {
        if (cargando)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(nombreEscenaJuego))
        {
            Debug.LogWarning(
                "Escribe el nombre de la escena del juego " +
                "en el componente MenuPrincipal.",
                this
            );

            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nombreEscenaJuego))
        {
            Debug.LogError(
                "No se puede cargar la escena '" +
                nombreEscenaJuego +
                "'. Comprueba su nombre y agregala a " +
                "Build Profiles o Build Settings.",
                this
            );

            return;
        }

        cargando = true;

        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(
            nombreEscenaJuego,
            LoadSceneMode.Single
        );
    }
}