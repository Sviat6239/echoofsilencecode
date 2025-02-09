using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats playerStats;

    void Update()
    {
        // Ввод для движения игрока
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isJumping = Input.GetKeyDown(KeyCode.Space);

        playerStats.SetMovement(isMoving);
        playerStats.SetRunning(isMoving && isRunning);
        playerStats.SetJumping(isJumping);
    }
}
