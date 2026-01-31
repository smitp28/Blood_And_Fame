using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
public class InsanityMeter : MonoBehaviour
{
    public static InsanityMeter instance;

    public float Max = 100f;
    public float Current=0f;
    public float lerpspeed = 0.1f;
    public Image insanityBar;
    public float rate;
    private CinemachineImpulseSource impulseSource;
     public float shakeAt80Interval = 2f;
    public float shakeAt90Interval = 1f;

    private float shakeTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Current += rate * Time.deltaTime;
        HealthbarFiller();
        ColourChanger();
        HandleCameraShake();

    }

    void HealthbarFiller()
    {
        float ratio = Current / Max;
        insanityBar.fillAmount = Mathf.Lerp(insanityBar.fillAmount, ratio, lerpspeed);
    }

    void ColourChanger()
    {
        Color healthColor = Color.Lerp(Color.yellow, Color.yellow, Current / Max);
        insanityBar.color = healthColor;
    }

    void HandleCameraShake()
    {
        float insanityPercent = Current / Max;

        // Below 80% = no shake
        if (insanityPercent < 0.8f)
        {
            shakeTimer = 0f;
            return;
        }

        shakeTimer += Time.deltaTime;

        float interval = GetShakeInterval(insanityPercent);

        if (shakeTimer >= interval)
        {
            impulseSource.GenerateImpulse();
            shakeTimer = 0f;
        }
    }

     float GetShakeInterval(float percent)
    {
        if (percent >= 0.9f)
            return shakeAt90Interval;

        return shakeAt80Interval;
    }

    public void ApplyInsanity(float amount)
    {
        Current += amount;
        Current = Mathf.Clamp(Current, 0, Max);
    }
}
