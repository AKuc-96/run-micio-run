using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float jumpForce = 20f; 
    [SerializeField] private float groundDistance = 0.25f; 
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private float crouchHeight = 0.5f;

    public float JumpForce => jumpForce;
    public float GroundDistance => groundDistance;
    public float JumpTime => jumpTime; 
    public float CrouchHeight => crouchHeight; 
}


