using Unity.Services.Core;
using UnityEngine;

public class InitializationExample : MonoBehaviour
{
	async void Awake()
	{
		try
		{
			await UnityServices.InitializeAsync();
		}
		catch (System.Exception e)
		{
			Debug.LogException(e);
		}
	}
}