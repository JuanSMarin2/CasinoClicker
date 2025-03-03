using TMPro;
using UnityEngine;

public class DiceManagerUI : MonoBehaviour
{
    // Referencia a la l�gica del juego
    private DiceManager diceManager;

    // Referencias a los elementos UI (asignados desde el Inspector)
    public GameObject slotBackground;
    public GameObject upgrateBackground;
    public GameObject coinBackground;
    public GameObject infoUpgratesBackground;




    private void Start()
    {
        // Instanciamos la clase de l�gica del juego
  
    }

    private void Update()
    { 
    
    }


    // M�todos para controlar la visibilidad de paneles en la UI
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
}
