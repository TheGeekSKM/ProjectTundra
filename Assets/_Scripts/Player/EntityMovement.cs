using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{
    private EntityStamina _entityStamina;

    PlayerStatsData _playerStatsData;
    Rigidbody2D _rb;

    private CameraMovement _cameraM;
    private bool _cameraFinished;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
        _entityStamina = GetComponent<EntityStamina>();
        _cameraM = Camera.main.GetComponent<CameraMovement>();
    }

	void OnEnable()
	{
		_cameraM.OnCameraMovementFinish += CameraDone;
	}
	void OnDisable()
	{
		_cameraM.OnCameraMovementFinish -= CameraDone;
	}


	/// <summary>
	///  Move the entity in the specified direction
	/// </summary>
	/// <param name="direction">Input a Vector2 Direction that you want the entity to move</param>
	void Move(Vector2 direction)
    {
        if (_entityStamina.CurrentActionPoints <= 0) return;

		var ray = Physics2D.Raycast(_rb.position, direction, 1, LayerMask.GetMask("Walls"));

		direction.Normalize();


		if (ray.collider != null && !ray.collider.CompareTag("RoomEdges")) return;
        else
        {
			if (ray.collider != null && ray.collider.CompareTag("RoomEdges"))
			{
				if (gameObject.CompareTag("Player"))
					StartCoroutine(CameraInterupt(direction));
				else return;
			}
			else
				_rb.MovePosition(_rb.position + direction);
        }
        
		// Debug.Log("Subtracted " + _movementCost + " movement point from total action points, resulting in " + _playerStatsData.CurrentActionPoints + " total points!");
    }
    IEnumerator CameraInterupt(Vector2 direction)
    {
        _rb.MovePosition(_rb.position + direction / 2);
        //_cameraM.Move();
        yield return new WaitUntil(() => _cameraFinished);
        _rb.MovePosition(_rb.position + direction / 2);
        _cameraFinished = false;
        yield break;
    }

    void CameraDone()
    {
        _cameraFinished = true;
    }

    void SubtractAP()
    {
		Debug.Log("Subtracting " + _playerStatsData.MovementCost + " AP from entity");
        _entityStamina.SubtractAP(_playerStatsData.MovementCost);
    }

    [ContextMenu("Move Up")]
    public void MoveUp(bool subtractAP)
    {
        Move(Vector2.up);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Down")]
    public void MoveDown(bool subtractAP)
    {
		Move(Vector2.down);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Left")]
    public void MoveLeft(bool subtractAP)
    {
		Move(Vector2.left);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Right")]
    public void MoveRight(bool subtractAP)
    {
		Move(Vector2.right);
        if (subtractAP) SubtractAP();
    }
}
