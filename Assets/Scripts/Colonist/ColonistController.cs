using System;
using Pathfinding;
using UnityEngine;

public class ColonistController : MonoBehaviour
{
	protected BaseColonist _Colonist;							// Reference to the colonist details
	public BaseColonist Colonist => _Colonist;

	[SerializeField] private float _MovementSpeed;						// Speed the character moves at

	[Header("Colonist Generation")]
	[SerializeField] private bool _GenerateColonist = false;
	[SerializeField] private bool _AddToColony = false;
	[SerializeField] private Seeker _Seeker;							// Reference to the AI Seeker
	[SerializeField] private Transform _DebugPosition;

	

	public bool HasBeenSelected = false;					// Flag if the character has been selected

	private void Awake()
	{
		if(!_Seeker)
			TryGetComponent<Seeker>(out _Seeker);
	}

	private void Start()
	{
		if(_Seeker)
			_Seeker.StartPath(this.transform.position, _DebugPosition.position, OnPathComplete);
	}

	public void OnPathComplete(Path p)
	{
		if(p.error)
		{
			Debug.Log("#ColonistController::OnPathComplete --> Failed to complete path");
		}
	}


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
