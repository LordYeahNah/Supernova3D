using System;
using UnityEngine;

public class ColonistController : MonoBehaviour
{
	protected BaseColonist _Colonist;							// Reference to the colonist details
	public BaseColonist Colonist => _Colonist;

	[SerializeField] private float _MovementSpeed;						// Speed the character moves at

	[Header("Colonist Generation")]
	[SerializeField] private bool _GenerateColonist = false;
	[SerializeField] private bool _AddToColony = false;
	

	public bool HasBeenSelected = false;					// Flag if the character has been selected

	/// <summary>
	/// Sets the colonist and this controller to the colonist
	/// </summary>
	/// <param name="colonist">Colonist to set</param>
	public void Setup(BaseColonist colonist)
	{
		_Colonist = colonist;							// Set the colonist

		// Validate the colonist and set its controller
		if (_Colonist != null)
		{
			_Colonist.Controller = this;				// Set the controller
			// Check if we want to add this character to the colony
			if (_AddToColony)
			{
                // TODO: Add coloinst to colony
			}
		}
		
	}
}
