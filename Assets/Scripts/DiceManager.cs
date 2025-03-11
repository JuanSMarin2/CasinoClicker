using UnityEngine;
using TMPro;
using System.Globalization;
using Unity.VisualScripting;

public class DiceManager : MonoBehaviour
{
    public long tokens = 0;
    public int diceLevel = 1;    // Nivel de dado
    public int dealerLevel = 0;  // Número de dealers
    public long dicePrice = 400;  // Precio para comprar un dado
    public long dealerPrice = 200; // Precio para comprar un dealer

    public int doubleDiceAmount = 0;
    public long doubleDiceCost = 2000;
    public GambleManager gambleManager;

    private float dealerTimer = 0f;
    public float dealerCooldown = 1f;
    public long energyCost = 1000;
    public int totalEnergyUpgrades = 0; // Acumulado total, nunca se reinicia

    private int lowerRange = 1;
    private int upperRange = 7;
    public long rangeCost = 2500;

   

    // Componentes UI asignados desde el Inspector
    public TextMeshProUGUI tokensTextUI;
    public TextMeshProUGUI diceLevelTextUI;
    public TextMeshProUGUI dicePriceTextUI;
    public TextMeshProUGUI dealerLevelTextUI;
    public TextMeshProUGUI dealerPriceTextUI;

    public TextMeshProUGUI doubleBuyedTextUI;
    public TextMeshProUGUI rangeBuyedTextUI;
    public TextMeshProUGUI energyBuyedTextUI;

    public TextMeshProUGUI diceSummaryTextUI;
    public TextMeshProUGUI dealerSummaryTextUI;
    public TextMeshProUGUI rangeSummaryTextUI;

    public int levelToDouble = 3;
    public int levelToEnergy = 2;
    public int levelToRange = 3;

    int doubleBuyed = 0;
    int energyBuyed = 0;
    int rangeBuyed = 0;

    public AudioClip diceClip;   // Asigna un AudioClip desde el Inspector
    public AudioSource audioSource;
    public AudioClip spendClip;
    public AudioClip loserClip;


    public string TokensText
    {
        get { return "x" + FormatNumber(tokens); }
    }

    private void Update()
    {
        // Actualización de la producción de dealers cada segundo.
        dealerTimer += Time.deltaTime;
        if (dealerTimer >= dealerCooldown)
        {
            long production = (long)dealerLevel * Random.Range(lowerRange, upperRange); // Produce entre lowerRange y upperRange - 1 fichas por dealer.
            if (production < 0)
            tokens += -(production);
            else
            tokens += production;

            dealerTimer = 0f;
        }

        // Actualiza la UI en cada frame con los valores actuales
        if (tokensTextUI != null)
            tokensTextUI.text = TokensText;
        if (diceLevelTextUI != null)
            diceLevelTextUI.text = "x" + diceLevel;
        if (dicePriceTextUI != null)
            dicePriceTextUI.text = "+1 x" + FormatNumber(dicePrice);
        if (dealerLevelTextUI != null)
            dealerLevelTextUI.text = "x" + dealerLevel;
        if (dealerPriceTextUI != null)
            dealerPriceTextUI.text = "$" + FormatNumber(dealerPrice);

        if (diceSummaryTextUI != null)
        {
            diceSummaryTextUI.text = GetDiceSummaryText();
        }

        // Actualiza el resumen de dealers si está asignado.
        if (dealerSummaryTextUI != null)
        {
            dealerSummaryTextUI.text = GetDealerSummaryText();
        }

        rangeSummaryTextUI.text = "Rango de dados: " + lowerRange + " - " + (upperRange-1) ;

        doubleBuyedTextUI.text = doubleBuyed + "/5";
        rangeBuyedTextUI.text = rangeBuyed + "/5";
        energyBuyedTextUI.text = energyBuyed + "/5";

        if (tokens < 0)
        {
            tokens = long.MaxValue;
        }
        if (diceLevel < 1) diceLevel = int.MaxValue;
        if (dealerLevel < 0) dealerLevel = int.MaxValue;
        if (doubleDiceAmount < 0) doubleDiceAmount = int.MaxValue;
        if (totalEnergyUpgrades < 0) totalEnergyUpgrades = int.MaxValue;
        if (lowerRange < 1) lowerRange = int.MaxValue;
        if (upperRange <= lowerRange) upperRange = lowerRange + 1;
        if (dicePrice < 0) dicePrice = long.MaxValue;
        if (dealerPrice < 0) dealerPrice = long.MaxValue;
        if (doubleDiceCost < 0) doubleDiceCost = long.MaxValue;
        if (energyCost < 0) energyCost = long.MaxValue;
        if (rangeCost < 0) rangeCost = long.MaxValue;
    }
    string GetDiceSummaryText()
    {
        // Calcula el valor promedio de un dado: entre lowerRange y (upperRange - 1)
        float avgRoll = (lowerRange + (upperRange - 1)) / 2f;
        // Cada dado tiene un bonus según la cantidad de duplicados.
        float avgPerDice = avgRoll * (1f + doubleDiceAmount * 0.25f);
        // El total es el promedio por dado por la cantidad de dados.
        float avgTokensPerClick = diceLevel * avgPerDice;
        return string.Format("Tienes {0} dados (duplicados {1} veces) generando en promedio {2:N0} fichas por click.",
                             diceLevel, doubleDiceAmount, avgTokensPerClick);
    }

    string GetDealerSummaryText()
    {
        // Calcula el valor promedio de un dealer, usando el mismo rango de dado.
        float avgRoll = (lowerRange + (upperRange - 1)) / 2f;
        float avgTokensPerTrigger = dealerLevel * avgRoll;
        // Dado que se produce cada dealerCooldown segundos, se obtiene el promedio por segundo.
        float avgTokensPerSecond = dealerCooldown > 0f ? avgTokensPerTrigger / dealerCooldown : 0f;
        return string.Format("Tienes {0} Dealers (energizados {1} veces) generando en promedio {2:N0} fichas por segundo.",
                             dealerLevel, totalEnergyUpgrades, avgTokensPerSecond);
    }

    /// <summary>
    /// Lanza el dado y suma el resultado a las fichas.
    /// </summary>
    public void RollDice()
    {
        long total = 0;
        for (int i = 0; i < diceLevel; i++)
        {
            int roll = Random.Range(lowerRange, upperRange); // Lanzamiento de dado
            int modifiedRoll = roll + Mathf.RoundToInt(roll * doubleDiceAmount * 0.25f); // Bonus moderado por doubleDice

            total += Mathf.Abs(modifiedRoll);
            Debug.Log("Dado " + (i + 1) + " resultó: " + roll + " → Modificado: " + modifiedRoll);
        }
        audioSource.PlayOneShot(diceClip);
        tokens += total;
        Debug.Log("Tokens obtenidos en total: " + total);
    }

    /// <summary>
    /// Compra un dado adicional si hay fichas suficientes.
    /// </summary>
    public void BuyDice()
    {
        if (tokens >= dicePrice)
        {
            tokens -= dicePrice;
            diceLevel++;
            dicePrice = (long)Mathf.Round(dicePrice * 1.3f);
            audioSource.PlayOneShot(spendClip);
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para comprar un dado adicional.");
            audioSource.PlayOneShot(loserClip);
        }
    }

    public void DoubleDice()
    {
        if (tokens >= doubleDiceCost && gambleManager.slotLevel >= levelToDouble)
        {
            tokens -= doubleDiceCost;
            if(gambleManager.slotLevel <= 10)
            {
                doubleDiceAmount++;
            }
            else
            {
                doubleDiceAmount = (int)Mathf.Round(doubleDiceAmount * 1.2f);
            }


           
            doubleDiceCost = (long)Mathf.Round(doubleDiceCost * 1.3f);
            audioSource.PlayOneShot(spendClip);
            doubleBuyed++;

            if (doubleBuyed >= 5)
            {
                levelToDouble += 1;
                doubleBuyed = 0;
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas o nivel para duplicar el dado");
            audioSource.PlayOneShot(loserClip);
        }
    }

    public void EnergyDrink()
    {
        if (tokens >= energyCost && gambleManager.slotLevel >= levelToEnergy)
        {
            tokens -= energyCost;
            dealerCooldown = dealerCooldown - dealerCooldown * 0.1f;
            energyCost = (long)Mathf.Round(energyCost * 1.5f);
            audioSource.PlayOneShot(spendClip);
            energyBuyed++;
            totalEnergyUpgrades++;

            if (energyBuyed >= 5)
            {
                levelToEnergy += 1;
                energyBuyed = 0;
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas o nivel para comprar bebida energizante");
            audioSource.PlayOneShot(loserClip);
        }
    }

    public void UpgrateRange()
    {
        if (tokens >= rangeCost && gambleManager.slotLevel >= levelToRange)
        {
            tokens -= rangeCost;

            if (gambleManager.slotLevel <= 8)
            {
                lowerRange += 5;
                upperRange += 5;
            }
            else 
            {
                if (lowerRange == 1)
                {
                    lowerRange  ++;
                    lowerRange = (int)Mathf.Round(lowerRange * 1.2f);
                    upperRange = (int)Mathf.Round(upperRange * 1.2f);
                }
                else {
                    lowerRange = (int)Mathf.Round(lowerRange * 1.2f);
                    upperRange = (int)Mathf.Round(upperRange * 1.2f);
                }
            }

            rangeCost = (long)Mathf.Round(rangeCost * 1.3f);
            audioSource.PlayOneShot(spendClip);
            rangeBuyed++;

            if (rangeBuyed >= 5)
            {
                levelToRange += 1;
                rangeBuyed = 0;
            }
        }
        else
        {
            Debug.Log("No tienes suficientes fichas o nivel para comprar aumento de rango");
            audioSource.PlayOneShot(loserClip);
        }
    }

    /// <summary>
    /// Compra un dealer si hay fichas suficientes.
    /// </summary>
    public void BuyDealer()
    {
        if (tokens >= dealerPrice)
        {
            tokens -= dealerPrice;
            dealerLevel++;
            dealerPrice = (long)Mathf.Round(dealerPrice * 1.3f);
            audioSource.PlayOneShot(spendClip);
        }
        else
        {
            Debug.Log("No tienes suficientes fichas para comprar un dealer.");
            audioSource.PlayOneShot(loserClip);
        }
    }



    /// <summary>
    /// Formatea números grandes para mostrarlos de forma legible.
    /// </summary>
    public string FormatNumber(long number)
    {
        if (number < 1_000_000_000)
            return number.ToString("N0", new CultureInfo("es-ES"));
        else if (number < 1_000_000_000_000)
            return (number / 1_000_000_000.0).ToString("0.##") + "B";
        else if (number < 1_000_000_000_000_000)
            return (number / 1_000_000_000_000.0).ToString("0.##") + "T";
        else if (number < 1_000_000_000_000_000_000)
            return (number / 1_000_000_000_000_000.0).ToString("0.##") + "Q";
        else
            return (number / 1_000_000_000_000_000_000.0).ToString("0.##") + "Qa";
    }
}
