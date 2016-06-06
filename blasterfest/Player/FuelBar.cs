using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.Extensions;

public class FuelBar : MonoBehaviour {

	[SerializeField]
	private float _burnTime = 1; // time in seconds
	[SerializeField]
	private float _rechargeTime = 1;
	[SerializeField]
	private Sprite[] colorForegroundSprites, colorBackgroundSprites;
	[SerializeField]
	private Image _background;

	private Image fuelBar;


	void Awake ()
	{
		fuelBar = GetComponent<Image>();
	}

	public void SetColor (PlayerNumber playerNumber) {
		fuelBar.sprite = colorForegroundSprites[(int)playerNumber];
		_background.sprite = colorBackgroundSprites[(int)playerNumber];
	}

	public void Burn ()
	{
		fuelBar.fillAmount -= Time.deltaTime / _burnTime;

	}

	public void Recharge ()
	{
		fuelBar.fillAmount += Time.deltaTime / _rechargeTime;
	}

	public float FuelCount ()
	{
		return fuelBar.fillAmount;
	}

	public void Refill ()
	{
		fuelBar.fillAmount = 1;
	}

	public void Empty ()
	{
		fuelBar.fillAmount = 0;
	}
}
