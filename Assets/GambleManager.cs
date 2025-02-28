using TMPro;
using UnityEngine;

public class GambleManager : MonoBehaviour
{
    public int slotLevel = 0; // Nivel del slot
    public int tokens = 0; // Referencia a las fichas del jugador
    public DiceManager diceManager; // Referencia al DiceManager

    private int unlockCost = 200; // Costo para desbloquear el nivel 1
    private int betCost; // Costo de la apuesta
    private int winAmount; // Monto del premio
    private int nextLevelCost; // Costo para subir de nivel


    public GameObject slotLevelTextObject;
    private TextMeshProUGUI slotLevelText;

    public GameObject betCostTextObject;
    private TextMeshProUGUI betCostText;

    public GameObject winAmountTextObject;
    private TextMeshProUGUI winAmountText;

    public GameObject nextLevelCostTextObject;
    private TextMeshProUGUI nextLevelCostText;

    private void Start()
    {
        // Inicializa referencias y calcula costos
        if (diceManager == null)
            diceManager = FindAnyObjectByType<DiceManager>();

        slotLevelText = slotLevelTextObject.GetComponent<TextMeshProUGUI>();
        betCostText = betCostTextObject.GetComponent<TextMeshProUGUI>();
        winAmountText = winAmountTextObject.GetComponent<TextMeshProUGUI>();
        nextLevelCostText = nextLevelCostTextObject.GetComponent<TextMeshProUGUI>();


        CalculateCosts();
        UpdateUI();
    }

    private void Update()
    {
      

        UpdateUI();
    }

    public void gambleSlot()
    {
        if (slotLevel != 0 && diceManager.tokens >= betCost)
        {
            diceManager.tokens -= betCost;

            int gamblingValue = Random.Range(1, 7);
            if (gamblingValue == 1)
            {
                diceManager.tokens += winAmount;
                Debug.Log("¡Ganaste " + winAmount + " fichas!");
            }
            else
            {
                Debug.Log("Perdiste la apuesta.");
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para apostar.");
        }
    }

    public void UpgrateSlotLevel()
    {
        if (diceManager.tokens >= nextLevelCost)
        {
          
            slotLevel++;
            unlockCost *= 3; // Se triplica el costo de desbloqueo en cada nivel
            CalculateCosts();
            Debug.Log("Subiste al nivel " + slotLevel);
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para subir de nivel.");
        }
    }



    private void CalculateCosts()
    {
        betCost = unlockCost / 4;
        winAmount = unlockCost * 3;
        nextLevelCost = winAmount * 3;
    }

    private void UpdateUI()
    {
        slotLevelText.text = "Nivel: " + slotLevel;

        if (slotLevel == 0)
        {
            betCostText.text = "Aun no puedes apostar";

        }
        else
        {
            betCostText.text = "Costo de apuesta: " + betCost;
        }

        if (slotLevel == 0) {
            winAmountText.text = "Recompensa: 0" ;
        }
        else
        {
            winAmountText.text = "Recompensa: " + winAmount;
        }
      
       
        nextLevelCostText.text = "Fichas necesarias para subir de nivel: " + nextLevelCost;
    }
}
