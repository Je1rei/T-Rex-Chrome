
using UnityEngine;

public class Player : MonoBehaviour
{
    private readonly string _tagObstacles = "Obstacle";
    
    [SerializeField] private float _gravity = 9.81f * 2f;
    [SerializeField] private float _jumpForce = 8f;

    private CharacterController _characterController;
    private Vector3 _direction;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _direction = Vector3.zero;  
    }

    private void Update()
    {
        _direction += Vector3.down * _gravity * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            _direction = Vector3.down;

            if (Input.GetButton("Jump"))
            {
                _direction = Vector3.up * _jumpForce;
                GameManager.Instance.JumpSound();
            }
        }

        _characterController.Move(_direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tagObstacles))
        {
            GameManager.Instance.GameOver();
        }
    }

}
