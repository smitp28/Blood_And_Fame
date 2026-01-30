using UnityEngine;
using UnityEngine.UI;
public class PopularityMeter : MonoBehaviour
{
    public static PopularityMeter instance;

    public float Max = 100f;
    public float Current = 0f;
    public float lerpspeed = 0.1f;
    public Image popularityBar;
    public float rate;
   
    private void Awake()
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
        popularityBar.fillAmount = Mathf.Lerp(popularityBar.fillAmount, ratio, lerpspeed);
    }

    void ColourChanger()
    {
        Color healthColor = Color.Lerp(Color.purple, Color.purple, Current / Max);
        popularityBar.color = healthColor;
    }

    public void ApplyPopularity(float amount)
    {
        Current += amount;
        Current = Mathf.Clamp(Current, 0, Max);
    }
}
