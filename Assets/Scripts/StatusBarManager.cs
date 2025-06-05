using UnityEngine;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    public CharacterController characterController;

    [Header("UI Elements")]
    public Slider chargeBar;
    public Slider virusScanBar;
    public Button chargeButton;
    public Button virusScanButton;

    [Header("Settings")]
    [Range(1f, 100f)]
    public float decreaseRate = 50f;    // Zamanla azalması
    public float increaseAmount = 5000f;  // Butona basınca artış miktarı
    public float criticalLevel = 2000f;   // Kritik seviye (yüzde olarak)

    [Header("Colors")]
    public Color normalColor = Color.green;
    public Color criticalColor = Color.red;

    private Image chargeFill;
    private Image virusFill;
    private bool isCharging = false;
    private bool isScanning = false;

    private float chargeTimer = 0f;
    private float virusTimer = 0f;
    private float decreaseInterval = 0.1f; // 100 ms'de bir azalsın

    private void Start()
    {
        chargeButton.onClick.AddListener(FillChargeBar);
        virusScanButton.onClick.AddListener(FillVirusScanBar);

        chargeFill = chargeBar.fillRect.GetComponent<Image>();
        virusFill = virusScanBar.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        // Şarj Barı azalma
        chargeTimer += Time.deltaTime;
        if (chargeTimer >= decreaseInterval && !isCharging)
        {
            chargeBar.value = Mathf.Max(0, chargeBar.value - decreaseRate * decreaseInterval);
            chargeTimer = 0f; // Timer sıfırlanıyor
        }

        // Virüs Tarama Barı azalma
        virusTimer += Time.deltaTime;
        if (virusTimer >= decreaseInterval && !isScanning)
        {
            virusScanBar.value = Mathf.Max(0, virusScanBar.value - decreaseRate * decreaseInterval);
            virusTimer = 0f; // Timer sıfırlanıyor
        }

        // Kritik seviyede mi kontrol et
        UpdateBarColor(chargeBar, chargeFill);
        UpdateBarColor(virusScanBar, virusFill);

        bool isCritical = (chargeBar.value <= criticalLevel) && (virusScanBar.value <= criticalLevel);
        characterController.SetSadState(isCritical);
    }

    private void FillChargeBar()
    {
        chargeBar.value = Mathf.Min(chargeBar.value + increaseAmount, chargeBar.maxValue);
    }

    private void FillVirusScanBar()
    {
        virusScanBar.value = Mathf.Min(virusScanBar.value + increaseAmount, virusScanBar.maxValue);
    }

    private void UpdateBarColor(Slider bar, Image fillImage)
    {
        if (bar.value <= criticalLevel)
        {
            fillImage.color = criticalColor;
        }
        else
        {
            fillImage.color = normalColor;
        }
    }
}
