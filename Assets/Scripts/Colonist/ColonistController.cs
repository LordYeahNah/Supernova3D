using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering;

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

		if(_GenerateColonist)
		{
			_Colonist = ColonistGenerator.GenerateColonist(false);
			_Colonist.Controller = this;
		}
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

	public void StopMovement()
	{
		if(_Seeker)
			_Seeker.CancelCurrentPathRequest();
	}
}
