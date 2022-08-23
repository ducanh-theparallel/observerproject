using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DemoObserver;

public class UITextManager : MonoBehaviour
{
	#region Init, config

	[SerializeField] Text jumpText = null;
	[SerializeField] Slider slider = null;

	void Awake()
	{
		// if missing data config, then destroy this script
		if (jumpText == null||slider==null )
		{
			DestroyImmediate(this);
		}
	}


	// Use this for initialization
	void Start ()
	{
		// register to receive events
		
		this.RegisterListener(EventID.OnCollisionBall, (param) => OnCollisionBall());
        this.RegisterListener(EventID.HighInSlide, (param) => HighInSlide());
    }

	#endregion



	#region Event callback

	int _jumpCount = 0;

	void OnCollisionBall()
	{
		_jumpCount++;
		jumpText.text = _jumpCount.ToString();
	}
	void HighInSlide()
    {
		slider.value = Ball.high;
    }

	#endregion
}