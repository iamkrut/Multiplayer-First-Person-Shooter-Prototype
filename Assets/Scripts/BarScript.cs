using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BarScript : NetworkBehaviour {
    
	[SyncVar]
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;

	[SerializeField]
    private Text valueText;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private bool lerpColors;
    
    public float MaxValue { get; set; }

	public float Value {
        set {
            valueText.text = "Health: " + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    void Start()
    {
        if (lerpColors)
        {
            content.color = fullColor;
        }
    }


    void Update()
    {
		if (!isServer) {
			return;
		}
        HandleBar();
    }

	private void HandleBar()
    {
		Debug.Log ("call");
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        if (lerpColors)
        {
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }
    }

	public void ChangeHealthBar(int fillAmount){
		content.fillAmount = fillAmount;
		content.color = Color.Lerp (lowColor, fullColor, fillAmount);
	}

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //(80 - 0) * (1 - 0) / (100 - 0) + 0
        //    = 80 * 1 / 100
        //    =  0.8

        //(78 - 0) * (1 - 0) / (230 - 0) + 0
        //    = 78 * 1 / 230
        //    =  0.339

    }
}
