using TMPro;
using UnityEngine;

public class DiceManagerUI : MonoBehaviour
{
    // Referencia a la lógica del juego
    private DiceManager diceManager;

    // Referencias a los elementos UI (asignados desde el Inspector)
    public GameObject slotBackground;
    public GameObject upgrateBackground;
    public GameObject coinBackground;
    public GameObject infoUpgratesBackground;
    public GameObject exitBackground;




    private void Start()
    {
        // Instanciamos la clase de lógica del juego
  
    }

    private void Update()
    { 
    
    }


    // Métodos para controlar la visibilidad de paneles en la UI
    public void ActivateMachine()
    {
        slotBackground.SetActive(true);
    }

    public void DeactivateMachine()
    {
        slotBackground.SetActive(false);
    }

    public void ActivateCoin()
    {
        coinBackground.SetActive(true);
    }

    public void DeactivateCoin()
    {
        coinBackground.SetActive(false);
    }

    public void ActivateUpgrate()
    {
        upgrateBackground.SetActive(true);
    }

    public void DeactivateUpgrate()
    {
        upgrateBackground.SetActive(false);
    }
    public void ActivateinfoUpgrates()
    {
        infoUpgratesBackground.SetActive(true);
    }

    public void DeactivateinfoUpgrates()
    {
        infoUpgratesBackground.SetActive(false);
    }

    public void ActivateExit()
    {
        exitBackground.SetActive(true);
    }

    public void DeactivateExit()
    {
        exitBackground.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Cerrar juego");
    }
}
