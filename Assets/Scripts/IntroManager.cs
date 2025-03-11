using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public List<GameObject> objectsToDeactivate;
    public AudioClip continueClip;
    public AudioSource audioSource;

    // Índice del siguiente objeto a desactivar
    private int currentIndex = 0;

    // Esta función se conecta al OnClick del botón
    public void OnButtonClick()
    {
        if (currentIndex < objectsToDeactivate.Count)
        {
            // Desactiva el objeto en la posición actual de la lista
            objectsToDeactivate[currentIndex].SetActive(false);
            currentIndex++;
            audioSource.PlayOneShot(continueClip);

            // Si se desactivó el último objeto, carga la escena "Game"
            if (currentIndex >= objectsToDeactivate.Count)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    public void Reborn()
    {
        SceneManager.LoadScene("Intro");
    }
}
