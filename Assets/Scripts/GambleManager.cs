using System.Globalization;
using TMPro;
using UnityEngine;

public class GambleManager : MonoBehaviour
{
    public int slotLevel = 0;
    public DiceManager diceManager;
    private long unlockCost = 200;
    private long nextLevelCost;

    private long slotBetAmount = 0;
    private long slotBetMax = 0;

    private long coinBetAmount = 0;
    private long coinBetMax = 0;

    public bool winCoinBet = false;

    private float startTime;
    public bool isChanceTime = false;
    public long chanceTimeCost = 3000;
    public int levelToChance = 4;

    public GameObject slotLevelTextObject;
    private TextMeshProUGUI slotLevelText;
    public GameObject nextLevelCostTextObject;
    private TextMeshProUGUI nextLevelCostText;

    public GameObject betCostTextObject;
    private TextMeshProUGUI betCostText;
    public GameObject winAmountTextObject;
    private TextMeshProUGUI winAmountText;
    public GameObject slotMaxBetTextObject;
    private TextMeshProUGUI slotMaxBetText;

    public GameObject coinBetTextObject;
    private TextMeshProUGUI coinBetText;
    public GameObject coinWinTextObject;
    private TextMeshProUGUI coinWinText;
    public GameObject coinMaxBetTextObject;
    private TextMeshProUGUI coinMaxBetText;

    public GameObject chanceTimerTextObject;
    private TextMeshProUGUI chanceTimerText;

    public TextMeshProUGUI doubleDicePriceTextUI;
    public TextMeshProUGUI rangePriceTextUI;
    public TextMeshProUGUI energyPriceTextUI;
    public TextMeshProUGUI chancePriceTextUI;

    private void Start()
    {
        if (diceManager == null)
            diceManager = FindAnyObjectByType<DiceManager>();

        slotLevelText = slotLevelTextObject.GetComponent<TextMeshProUGUI>();
        nextLevelCostText = nextLevelCostTextObject.GetComponent<TextMeshProUGUI>();

        betCostText = betCostTextObject.GetComponent<TextMeshProUGUI>();
        winAmountText = winAmountTextObject.GetComponent<TextMeshProUGUI>();
        slotMaxBetText = slotMaxBetTextObject.GetComponent<TextMeshProUGUI>();

        coinBetText = coinBetTextObject.GetComponent<TextMeshProUGUI>();
        coinWinText = coinWinTextObject.GetComponent<TextMeshProUGUI>();
        coinMaxBetText = coinMaxBetTextObject.GetComponent<TextMeshProUGUI>();
        chanceTimerText = chanceTimerTextObject.GetComponent<TextMeshProUGUI>();
        CalculateCosts();
        UpdateUI();
    }

    private void Update()
    {
        AdjustBetAmounts();
        UpdateUI();

        if (isChanceTime)
        {
            ChanceTimeTimer();
        }
    }

    private void AdjustBetAmounts()
    {
        if (slotBetAmount > diceManager.tokens)
            slotBetAmount = diceManager.tokens;
        if (coinBetAmount > diceManager.tokens)
            coinBetAmount = diceManager.tokens;
    }

    private void CalculateCosts()
    {
        long baseBetCost = unlockCost / 4;
        long baseWinAmount = baseBetCost * 3;
        nextLevelCost = baseWinAmount * 3;

        slotBetMax = nextLevelCost / 3;
        coinBetMax = nextLevelCost / 2;

        if (slotBetAmount > slotBetMax)
            slotBetAmount = slotBetMax;
        if (coinBetAmount > coinBetMax)
            coinBetAmount = coinBetMax;
    }

    #region Métodos para ajustar apuesta en Slots
    public void IncreaseSlotBet()
    {
        if (slotLevel < 1) return; // Bloqueado hasta el nivel 1
        long newBet = slotBetAmount + 100;
        if (newBet <= diceManager.tokens && newBet <= slotBetMax)
        {
            slotBetAmount = newBet;
        }
    }

    public void DecreaseSlotBet()
    {
        slotBetAmount = (long)Mathf.Max((float)slotBetAmount - 100, 0);
    }

    public void SetSlotBetMax()
    {
        if (slotLevel < 1) return;
        if (slotBetMax <= diceManager.tokens)
            slotBetAmount = slotBetMax;
        else if (slotBetMax > diceManager.tokens)
            slotBetAmount = diceManager.tokens;
    }

    public void SetSlotBetMin()
    {
        slotBetAmount = 0;
    }

    public void SetSlotBetHalf()
    {
        if (slotLevel < 1) return;


        slotBetAmount = diceManager.tokens / 2;
    }

    public void SetCoinBetMax()
    {
        if (slotLevel < 2) return;
        if (coinBetMax <= diceManager.tokens)
            coinBetAmount = coinBetMax;
        else if (coinBetMax > diceManager.tokens)
            coinBetAmount = diceManager.tokens;
    }

    public void SetCoinBetMin()
    {
        coinBetAmount = 0;
    }

   public void SetCoinBetHalf()
    {
        if (slotLevel < 2) return;
        coinBetAmount = diceManager.tokens / 2;
    }
    #endregion

    #region Métodos para ajustar apuesta en Moneda
    public void IncreaseCoinBet()
    {
        if (slotLevel < 2) return; // Bloqueado hasta el nivel 2
        long newBet = coinBetAmount + 100;
        if (newBet <= diceManager.tokens && newBet <= coinBetMax)
        {
            coinBetAmount = newBet;
        }
    }

    public void DecreaseCoinBet()
    {
        coinBetAmount = (long)Mathf.Max((float)coinBetAmount - 100, 0);
    }
    #endregion

    public void GambleSlot()
    {
        if (slotBetAmount <= 0)
        {
            Debug.Log("Apuesta de Slot es 0. Ajusta la cantidad para apostar.");
            return;
        }

        if (diceManager.tokens >= slotBetAmount)
        {
            diceManager.tokens -= slotBetAmount;
            int gamblingValue = !isChanceTime ? Random.Range(1, 5) : Random.Range(1, 3);
            Debug.Log(gamblingValue);

            if (gamblingValue == 1)
            {
                long reward = isChanceTime ? slotBetAmount * 3 : slotBetAmount * 4;
                diceManager.tokens += reward;
                Debug.Log("¡Slot: Ganaste " + reward + " fichas!");
            }
            else
            {
                Debug.Log("Slot: Perdiste la apuesta de " + slotBetAmount + " fichas.");
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para apostar.");
        }
    }

    public void CoinFlip()
    {
        if (coinBetAmount <= 0)
        {
            Debug.Log("Apuesta de Moneda es 0. Ajusta la cantidad para apostar.");
            return;
        }

        if (diceManager.tokens >= coinBetAmount)
        {
            diceManager.tokens -= coinBetAmount;
            int flip = !isChanceTime ? Random.Range(1, 3) : Random.Range(1, 5);
            Debug.Log("Flip: " + flip);

            if (!isChanceTime)
            {
                winCoinBet = (flip == 1);
            }
            else
            {
                winCoinBet = (flip <= 3);
            }
            if (winCoinBet)
            {
                long reward =  (long)Mathf.RoundToInt((float)coinBetAmount * 2f);
                diceManager.tokens += reward;
                Debug.Log("¡Moneda: Ganaste " + reward + " fichas!");
                winCoinBet = false;
            }
            else
            {
                Debug.Log("Moneda: Perdiste la apuesta de " + coinBetAmount + " fichas.");
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para apostar en Moneda.");
        }
    }

    public void UpgradeSlotLevel()
    {
        if (diceManager.tokens >= nextLevelCost && !isChanceTime)
        {
            slotLevel++;
            unlockCost = (long)(unlockCost * 2.5f);
            CalculateCosts();
            Debug.Log("Subiste al nivel " + slotLevel);
        }
    }

    public void ChanceTime()
    {
        if (diceManager.tokens >= chanceTimeCost && slotLevel >= levelToChance && !isChanceTime)
        {
            diceManager.tokens -= chanceTimeCost;
            isChanceTime = true;
            startTime = Time.time;
            chanceTimeCost *= 30;
            levelToChance += 5;
            Debug.Log("IsChanceTime " + isChanceTime);
        }
    }

    private void ChanceTimeTimer()
    {
        float elapsedTime = Time.time - startTime;
        if (elapsedTime < 15f)
        {
            chanceTimerText.text = "Chance Time por: " + elapsedTime.ToString("F0") + "/15";
        }
        else
        {
            Debug.Log("Fin ChanceTime");
            chanceTimerText.text = "";
            isChanceTime = false;
        }
    }

    private void UpdateUI()
    {
        slotLevelText.text = "Nivel: " + diceManager.FormatNumber(slotLevel);
        if (isChanceTime)
        {
            nextLevelCostText.text = "No puedes subir de nivel en chance time";
        }
        else
        {
            nextLevelCostText.text = "Fichas para subir de nivel: " + diceManager.FormatNumber(nextLevelCost);
        }
       

        if (slotLevel >= 1)
        {
            betCostText.text = "Cantidad apostada: " + diceManager.FormatNumber(slotBetAmount);
            winAmountText.text = "Recompensa: " + diceManager.FormatNumber((slotBetAmount * 5));
            slotMaxBetText.text = "Máximo para apostar: " + diceManager.FormatNumber(slotBetMax);
        }
        else
        {
            betCostText.text = "Aún no puedes apostar.";
            winAmountText.text = "";
            slotMaxBetText.text = "";
        }

        if (slotLevel >= 2)
        {
            coinBetText.text = "Cantidad apostada: " + diceManager.FormatNumber(coinBetAmount);
            coinWinText.text = "Recompensa: " + diceManager.FormatNumber(Mathf.RoundToInt(coinBetAmount * 1.8f));
            coinMaxBetText.text = "Máximo para apostar: " + diceManager.FormatNumber(coinBetMax);
            energyPriceTextUI.text = "Precio: " + diceManager.FormatNumber(diceManager.energyCost);
        }
        else
        {
            coinBetText.text = "Necesitas nivel 2";
            coinWinText.text = "";
            coinMaxBetText.text = "";
            energyPriceTextUI.text = "Necesitas nivel " + diceManager.levelToEnergy;
        }

        if (slotLevel >= diceManager.levelToDouble)
        {
            doubleDicePriceTextUI.text = "Precio: " + diceManager.FormatNumber(diceManager.doubleDiceCost);
        }
        else
        {
            doubleDicePriceTextUI.text = "Necesitas nivel " + diceManager.levelToDouble;
        }

        if (slotLevel >= diceManager.levelToRange)
        {
            rangePriceTextUI.text = "Precio: " + diceManager.FormatNumber(diceManager.rangeCost);
        }
        else
        {
            rangePriceTextUI.text = "Necesitas nivel " + diceManager.levelToRange;
        }

        if (slotLevel >= levelToChance)
        {
            chancePriceTextUI.text = "Precio: " + diceManager.FormatNumber(chanceTimeCost);
        }
        else
        {
            chancePriceTextUI.text = "Necesitas nivel: " + diceManager.FormatNumber(levelToChance);
        }
    }
}

