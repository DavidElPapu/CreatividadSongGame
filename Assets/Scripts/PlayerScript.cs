using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private PlayerActions playerActions;
    public float playerMoveSpeed;
    public Rigidbody2D leftHand, rightHand, leftFoot, rightFoot;
    public Sprite openHandSprite, lockedHandSprite, openFootSprite, lockedFootSprite;
    private SpriteRenderer leftHandRenderer, rightHandRenderer, leftFootRenderer, rightFootRenderer;
    private ExtremitiesDetector leftHandDetector, rightHandDetector, leftFootDetector, rightFootDetector;
    private HingeJoint2D leftHandLockHinge, rightHandLockHinge, leftFootLockHinge, rightFootLockHinge;
    private bool isMovingHands, leftHandLocked, rightHandLocked, leftFootLocked, rightFootLocked;
    public GameObject dieScreen;


    private void Awake()
    {
        playerActions = new PlayerActions();
        isMovingHands = true;
        dieScreen.SetActive(false);
        leftHand.gameObject.TryGetComponent(out leftHandRenderer);
        leftHand.gameObject.TryGetComponent(out leftHandDetector);
        rightHand.gameObject.TryGetComponent(out rightHandRenderer);
        rightHand.gameObject.TryGetComponent(out rightHandDetector);
        leftFoot.gameObject.TryGetComponent(out leftFootRenderer);
        leftFoot.gameObject.TryGetComponent(out leftFootDetector);
        rightFoot.gameObject.TryGetComponent(out rightFootRenderer);
        rightFoot.gameObject.TryGetComponent(out rightFootDetector);
    }

    private void OnEnable()
    {
        playerActions.PlayerNoMeDeja.Enable();
        playerActions.PlayerNoMeDeja.GrabLeft.started += OnLeftGrab;
        playerActions.PlayerNoMeDeja.GrabRight.started += OnRightGrab;
    }

    private void OnDisable()
    {
        playerActions.PlayerNoMeDeja.Disable();
        playerActions.PlayerNoMeDeja.GrabLeft.started -= OnLeftGrab;
        playerActions.PlayerNoMeDeja.GrabRight.started -= OnRightGrab;
    }

    private void Start()
    {
        rightHandLockHinge = rightHand.AddComponent<HingeJoint2D>();
        leftHandLockHinge = leftHand.AddComponent<HingeJoint2D>();
        rightFootLockHinge = rightFoot.AddComponent<HingeJoint2D>();
        leftFootLockHinge = leftFoot.AddComponent<HingeJoint2D>();
        rightHandLockHinge.enabled = false;
        leftHandLockHinge.enabled = false;
        rightFootLockHinge.enabled = false;
        leftFootLockHinge.enabled = false;
        leftHandLocked = false;
        rightHandLocked = false;
        leftFootLocked = false;
        rightFootLocked = false;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        //WASD
        Vector2 moveLeftInput = playerActions.PlayerNoMeDeja.MoveLeft.ReadValue<Vector2>();
        Vector2 moveRightInput = playerActions.PlayerNoMeDeja.MoveRight.ReadValue<Vector2>();

        if (isMovingHands)
        {
            MoveLeftHand(moveLeftInput);
            MoveRightHand(moveRightInput);
        }
        else
        {
            MoveLeftFoot(moveLeftInput);
            MoveRightFoot(moveRightInput);
        }
    }

    private void MoveLeftHand(Vector2 moveInput)
    {
        if (leftHandLocked) return;
        leftHand.AddForce(moveInput * playerMoveSpeed, ForceMode2D.Force);
    }

    private void MoveRightHand(Vector2 moveInput)
    {
        if (rightHandLocked) return;
        rightHand.AddForce(moveInput * playerMoveSpeed, ForceMode2D.Force);
    }

    private void MoveLeftFoot(Vector2 moveInput)
    {
        if (leftFootLocked) return;
        leftFoot.AddForce(moveInput * playerMoveSpeed, ForceMode2D.Force);
    }

    private void MoveRightFoot(Vector2 moveInput)
    {
        if (rightFootLocked) return;
        rightFoot.AddForce(moveInput * playerMoveSpeed, ForceMode2D.Force);
    }

    public void OnSwitch()
    {
        isMovingHands = !isMovingHands;
    }

    private void OnLeftGrab(InputAction.CallbackContext context)
    {
        if (isMovingHands)
            LeftHandGrab();
        else
            LeftFootGrab();
    }

    private void OnRightGrab(InputAction.CallbackContext context)
    {
        if (isMovingHands)
            RightHandGrab();
        else
            RightFootGrab();
    }

    private void RightHandGrab()
    {
        if (rightHandRenderer.sprite == lockedHandSprite)
            rightHandRenderer.sprite = openHandSprite;
        else
            rightHandRenderer.sprite = lockedHandSprite;

        if (rightHandLocked)
        {
            rightHandLocked = false;
            rightHandLockHinge.enabled = false;
            rightHandRenderer.sprite = openHandSprite;
        }
        else if(rightHandDetector.IsHoldingGrab())
        {
            rightHandLocked = true;
            rightHandLockHinge.enabled = true;
            rightHandRenderer.sprite = lockedHandSprite;
        }
    }

    private void LeftHandGrab()
    {
        if (leftHandRenderer.sprite == lockedHandSprite)
            leftHandRenderer.sprite = openHandSprite;
        else
            leftHandRenderer.sprite = lockedHandSprite;

        if (leftHandLocked)
        {
            leftHandLocked = false;
            leftHandLockHinge.enabled = false;
            leftHandRenderer.sprite = openHandSprite;
        }
        else if (leftHandDetector.IsHoldingGrab())
        {
            leftHandLocked = true;
            leftHandLockHinge.enabled = true;
            leftHandRenderer.sprite = lockedHandSprite;
        }
    }

    private void RightFootGrab()
    {
        if (rightFootRenderer.sprite == lockedFootSprite)
            rightFootRenderer.sprite = openFootSprite;
        else
            rightFootRenderer.sprite = lockedFootSprite;

        if (rightFootLocked)
        {
            rightFootLocked = false;
            rightFootLockHinge.enabled = false;
            rightFootRenderer.sprite = openFootSprite;
        }
        else if (rightFootDetector.IsHoldingGrab())
        {
            rightFootLocked = true;
            rightFootLockHinge.enabled = true;
            rightFootRenderer.sprite = lockedFootSprite;
        }
    }

    private void LeftFootGrab()
    {
        if (leftFootRenderer.sprite == lockedFootSprite)
            leftFootRenderer.sprite = openFootSprite;
        else
            leftFootRenderer.sprite = lockedFootSprite;

        if (leftFootLocked)
        {
            leftFootLocked = false;
            leftFootLockHinge.enabled = false;
            leftFootRenderer.sprite = openFootSprite;
        }
        else if (leftFootDetector.IsHoldingGrab())
        {
            leftFootLocked = true;
            leftFootLockHinge.enabled = true;
            leftFootRenderer.sprite = lockedFootSprite;
        }
    }

    private void Die()
    {
        dieScreen.SetActive(true);
        rightHandLocked = false;
        rightHandLockHinge.enabled = false;
        rightHandRenderer.sprite = openHandSprite;
        leftHandLocked = false;
        leftHandLockHinge.enabled = false;
        leftHandRenderer.sprite = openHandSprite;
        rightFootLocked = false;
        rightFootLockHinge.enabled = false;
        rightFootRenderer.sprite = openFootSprite;
        leftFootLocked = false;
        leftFootLockHinge.enabled = false;
        leftFootRenderer.sprite = openFootSprite;
        playerActions.PlayerNoMeDeja.Disable();
    }
}
