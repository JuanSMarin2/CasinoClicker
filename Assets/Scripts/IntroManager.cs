using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public List<GameObject> objectsToDeactivate;
    public AudioClip continueClip;
    public AudioSource audioSource;

    // �ndice del siguiente objeto a desactivar
    private int currentIndex = 0;

    // Esta funci�n se conecta al OnClick del bot�n
    public void OnButtonClick()
    {
        if (currentIndex < objectsToDeactivate.Count)
        {
            // Desactiva el objeto en la posici�n actual de la lista
            objectsToDeactivate[currentIndex].SetActive(false);
            currentIndex++;
            audioSource.PlayOneShot(continueClip);

            // Si se desactiv� el �ltimo objeto, carga la escena "Game"
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
