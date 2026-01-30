using UnityEngine;
using UnityEngine.UI;
public class InsanityMeter : MonoBehaviour
{
    public float Max = 100f;
    public float Current=0f;
    public float lerpspeed = 0.1f;
    public Image healthBar;
    public float rate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, ratio, lerpspeed);
    }

    void ColourChanger()
    {
        Color healthColor = Color.Lerp(Color.green, Color.red, Current / Max);
        healthBar.color = healthColor;
    }
}
