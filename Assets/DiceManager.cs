using UnityEngine;
using TMPro;

public class DiceManager : MonoBehaviour
{
    public int tokens = 0;
    public int diceLevel = 1;    // Número de dados adicionales comprados
    public int dealerLevel = 0;  // Número de dealers comprados
    public int dicePrice = 400;  // Precio inicial para comprar un dado adicional
    public int dealerPrice = 200; // Precio inicial para comprar un dealer

    // UI para tokens, dados y dealers
    public GameObject slotBackground;

    public GameObject tokenTextObject;
    private TextMeshProUGUI tokenText;

    public GameObject diceLevelTextObject;
    private TextMeshProUGUI diceLevelText;

    public GameObject diceCostTextObject;
    private TextMeshProUGUI diceCostText;

    public GameObject dealerLevelTextObject;
    private TextMeshProUGUI dealerLevelText;

    public GameObject dealerCostTextObject;
    private TextMeshProUGUI dealerCostText;

    // Timer para la producción de dealers (cada segundo)
    private float dealerTimer = 0f;

    private void Start()
    {
        tokenText = tokenTextObject.GetComponent<TextMeshProUGUI>();

        diceLevelText = diceLevelTextObject.GetComponent<TextMeshProUGUI>();

        diceCostText = diceCostTextObject.GetComponent<TextMeshProUGUI>();

        dealerLevelText = dealerLevelTextObject.GetComponent<TextMeshProUGUI>();

        dealerCostText = dealerCostTextObject.GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        // Actualizar la UI
        tokenText.text = "Fichas: " + tokens;
        diceLevelText.text = "Dados: " + diceLevel;
        diceCostText.text = "Precio de dado: " + dicePrice;
        dealerLevelText.text = "Dealers: " + dealerLevel;
        dealerCostText.text = "Precio de dealer: " + dealerPrice;

        // Cada segundo, los dealers generan fichas
        dealerTimer += Time.deltaTime;
        if (dealerTimer >= 1f)
        {
            // Por cada dealer se genera un número aleatorio entre 1 y 3
            int production = dealerLevel * Random.Range(1, 4);
            tokens += production;
            dealerTimer = 0f;
        }
    }

    // Función para tirar el dado
    public void RollDice()
    {
        int roll = Random.Range(1, 4);
        // Se suma la tirada base más un bono proporcional al nivel de dado
        tokens += roll + diceLevel * roll;
    }

    // Función para comprar dados adicionales
    public void BuyDice()
    {
        if (tokens >= dicePrice)
        {
            tokens -= dicePrice;
            diceLevel++;
            dicePrice = Mathf.RoundToInt(dicePrice * 1.5f); // El precio aumenta por 1.5
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para comprar un dado adicional.");
        }
    }

    // Función para comprar dealers
    public void BuyDealer()
    {
        if (tokens >= dealerPrice)
        {
            tokens -= dealerPrice;
            dealerLevel++;
            dealerPrice = Mathf.RoundToInt(dealerPrice * 1.3f); // El precio aumenta por 1.5
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para comprar un dealer.");
        }
    }

    public void activateMachine()
    {

        slotBackground.SetActive(true);
    }

    public void deactivateMachine()
    {

        slotBackground.SetActive(false);
    }
}
