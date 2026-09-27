using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform GFX;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, playerConfig.GroundDistance, groundLayer);

        #region JUMPING

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.linearVelocity = Vector2.up * playerConfig.JumpForce;
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < playerConfig.JumpTime)
            {
                rb.linearVelocity = Vector2.up * playerConfig.JumpForce;

                jumpTimer += Time.deltaTime;
            }
            else
            {
               isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }
        #endregion
        #region CROUCHING
        if (isGrounded && Input.GetButton("Crouch"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, playerConfig.CrouchHeight, GFX.localScale.z);
            
            if (isJumping)
               {   
                  GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
               }
        }


        if (Input.GetButtonUp("Crouch"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
        #endregion
    }
}
