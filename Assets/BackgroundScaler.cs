using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BackgroundScaler : MonoBehaviour
{
	[SerializeField] private CanvasScaler scaler;
	[SerializeField] private PixelPerfectCamera ppCamera;

	private void OnValidate()
	{
		if (ppCamera != null)
		{
			if (scaler.scaleFactor != ppCamera.pixelRatio)
				scaler.scaleFactor = ppCamera.pixelRatio;
		}
	}

	private void Awake()
	{
		if (ppCamera != null)
		{
			if (scaler.scaleFactor != ppCamera.pixelRatio)
				scaler.scaleFactor = ppCamera.pixelRatio;
		}
	}
}
