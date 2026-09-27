using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Health Settings")]
    [SerializeField] private int initialLives = 1;
    [SerializeField] private int maxLives = 9;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 20f; 
    [SerializeField] private float groundDistance = 0.25f; 
    [SerializeField] private float jumpTime = 0.3f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.5f;

    public int InitialLIves => initialLives;
    public int MaxLives => maxLives;
    public float JumpForce => jumpForce;
    public float GroundDistance => groundDistance;
    public float JumpTime => jumpTime; 
    public float CrouchHeight => crouchHeight; 
}


