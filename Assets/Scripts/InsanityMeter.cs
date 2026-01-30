using UnityEngine;
using UnityEngine.UI;
public class InsanityMeter : MonoBehaviour
{
    public static InsanityMeter instance;

    public float Max = 100f;
    public float Current=0f;
    public float lerpspeed = 0.1f;
    public Image insanityBar;
    public float rate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void ApplyInsanity(float amount)
    {
        Current += amount;
        Current = Mathf.Clamp(Current, 0, Max);
    }
}
