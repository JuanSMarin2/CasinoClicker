using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    private bool gambleState;

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

    public GameObject winSlot;
    public GameObject loseSlot;

    public GameObject youWin;
    public GameObject youLose;

    public GameObject winCoin;
    public GameObject loseCoin;

    public AudioClip winClip;   // Asigna un AudioClip desde el Inspector
    public AudioClip loseClip;
    public AudioSource audioSource;
 
    public AudioClip clockClip;

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

        if (coinBetAmount < 0)
            coinBetAmount = 0;

        if (slotBetAmount < 0)
            slotBetAmount = 0;
    }

    public void GambleResult(bool hasWin)
    {
        if (hasWin)
        {
            StartCoroutine(ShowResult(youWin));
        }
        else
        {
            StartCoroutine(ShowResult(youLose));
        }
    }

    private IEnumerator ShowResult(GameObject resultObject)
    {
        resultObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        resultObject.SetActive(false);
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

    #region M�todos para ajustar apuesta en Slots
    public void IncreaseSlotBet()
    {
        if (slotLevel < 1) return; // Bloqueado hasta el nivel 1

        long increaseAmount = 100;
        if (slotLevel >= 5) increaseAmount = 1000;
     

        long newBet = slotBetAmount + increaseAmount;
        if (newBet <= diceManager.tokens && newBet <= slotBetMax)
        {
            slotBetAmount = newBet;
        }
    }

    public void DecreaseSlotBet()
    {
        long decreaseAmount = 100;
        if (slotLevel >= 5) decreaseAmount = 1000;
      

        slotBetAmount = (long)Mathf.Max((float)slotBetAmount - decreaseAmount, 0);
    }

    public void IncreaseCoinBet()
    {
        if (slotLevel < 2) return; // Bloqueado hasta el nivel 2

        long increaseAmount = 100;
        if (slotLevel >= 5) increaseAmount = 1000;
     

        long newBet = coinBetAmount + increaseAmount;
        if (newBet <= diceManager.tokens && newBet <= coinBetMax)
        {
            coinBetAmount = newBet;
        }
    }

    public void DecreaseCoinBet()
    {
        long decreaseAmount = 100;
        if (slotLevel >= 5) decreaseAmount = 1000;
      

        coinBetAmount = (long)Mathf.Max((float)coinBetAmount - decreaseAmount, 0);
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


        slotBetAmount = slotBetMax / 2;
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
        coinBetAmount = coinBetMax / 2;
    }
    #endregion

    #region M�todos para ajustar apuesta en Moneda

    #endregion

    public void GambleSlot()
    {
        if (slotBetAmount <= 0)
        {
            Debug.Log("Apuesta de Slot es 0. Ajusta la cantidad para apostar.");
            audioSource.PlayOneShot(loseClip);
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
                Debug.Log("�Slot: Ganaste " + reward + " fichas!");

                winSlot.SetActive(true);
                audioSource.PlayOneShot(winClip);

                gambleState = true;
                GambleResult(gambleState);
            }
            else
            {
                audioSource.PlayOneShot(loseClip);
                Debug.Log("Slot: Perdiste la apuesta de " + slotBetAmount + " fichas.");
                winSlot.SetActive(false);
                gambleState = false;
                GambleResult(gambleState);
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para apostar.");
            audioSource.PlayOneShot(loseClip);
        }
    }

    public void CoinFlip()
    {
        if (coinBetAmount <= 0)
        {
            Debug.Log("Apuesta de Moneda es 0. Ajusta la cantidad para apostar.");
            audioSource.PlayOneShot(loseClip);
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
                long reward = coinBetAmount * 2;
                diceManager.tokens += reward;
                Debug.Log("�Moneda: Ganaste " + reward + " fichas!");
                winCoinBet = false;
                audioSource.PlayOneShot(winClip);
                winCoin.SetActive(true);

                gambleState = true;
                GambleResult(gambleState);
            }
            else
            {
                audioSource.PlayOneShot(loseClip);
                winCoin.SetActive(false);
                Debug.Log("Moneda: Perdiste la apuesta de " + coinBetAmount + " fichas.");
                gambleState = false;
                GambleResult(gambleState);
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para apostar en Moneda.");
            audioSource.PlayOneShot(loseClip);
        }
    }

    public void UpgradeSlotLevel()
    {
        if (diceManager.tokens >= nextLevelCost && !isChanceTime)
        {
            slotLevel++;

            if (slotLevel <= 5)
            {
                unlockCost = (long)(unlockCost * 2.2f);
                CalculateCosts();
            }
            else {
                unlockCost = (long)(unlockCost * 2.7f);
                CalculateCosts();
            }

            Debug.Log("Subiste al nivel " + slotLevel);
            audioSource.PlayOneShot(winClip);
        }
        else
        {
            audioSource.PlayOneShot(loseClip);
        }

        if(slotLevel >= 33)
            SceneManager.LoadScene("Transcend");
    }

    public void ChanceTime()
    {
        if (diceManager.tokens >= chanceTimeCost && slotLevel >= levelToChance && !isChanceTime)
        {
            audioSource.PlayOneShot(clockClip);
            diceManager.tokens -= chanceTimeCost;
            isChanceTime = true;
            startTime = Time.time;
            chanceTimeCost *= 30;
            levelToChance += 5;
            Debug.Log("IsChanceTime " + isChanceTime);
        }
        else
        {
           
            audioSource.PlayOneShot(loseClip);
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
        else if (slotLevel < 32)
        {
            nextLevelCostText.text = "Fichas para subir de nivel: " + diceManager.FormatNumber(nextLevelCost);
        }
        else
        {
            nextLevelCostText.text = "Fichas para Trascender: " + diceManager.FormatNumber(nextLevelCost);
        }
       

        if (slotLevel >= 1)
        {
            betCostText.text = "Cantidad apostada: " + diceManager.FormatNumber(slotBetAmount);
            winAmountText.text = "Recompensa: " + diceManager.FormatNumber((slotBetAmount * 5));
            slotMaxBetText.text = "Maximo para apostar: " + diceManager.FormatNumber(slotBetMax);
        }
        else
        {
            betCostText.text = "Aun no puedes apostar.";
            winAmountText.text = "";
            slotMaxBetText.text = "";
        }

        if (slotLevel >= 2)
        {
            coinBetText.text = "Cantidad apostada: " + diceManager.FormatNumber(coinBetAmount);
            long coinReward = (coinBetAmount * 2);
            coinWinText.text = "Recompensa: " + diceManager.FormatNumber(coinReward);
            coinMaxBetText.text = "Maximo para apostar: " + diceManager.FormatNumber(coinBetMax);
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

