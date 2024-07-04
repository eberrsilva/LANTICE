﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalJumpSpeed;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource runningAudioSource;

    private Transform target;

    public float ForwardMotion { get; set; } = 0;
    public float SidewaysMotion { get; set; } = 0;
    private float UpwardMotion { get; set; } = 0;

    private Vector3 HorizontalVelocity =>
        (target.forward * ForwardMotion + target.right * SidewaysMotion).normalized * horizontalSpeed;
    private Vector3 VerticalVelocity => Vector3.up * UpwardMotion;
    private Vector3 Velocity => HorizontalVelocity + VerticalVelocity;
    private bool IsJumping => UpwardMotion > 0;

    private Vector3 prevVelocity;

    private void Awake()
    {
        target = GetComponent<Transform>();
    }

    private void Update()
    {
        if (prevVelocity != Velocity && !runningAudioSource.isPlaying)
        {
            runningAudioSource.Play();
        }

        if (prevVelocity == Velocity && runningAudioSource.isPlaying)
        {
            runningAudioSource.Stop();
        }

        SetUpwardMotion(Time.deltaTime);
        ApplyMotion(Time.deltaTime, Velocity);

        prevVelocity = Velocity;
    }

    public void Jump()
    {
        if (!characterController.isGrounded)
            return;

        jumpAudioSource.Play();

        UpwardMotion = verticalJumpSpeed;
    }

    private void SetUpwardMotion(float deltaTime)
    {
        if (characterController.isGrounded && !IsJumping)
            UpwardMotion = 0;

        UpwardMotion -= gravity * deltaTime;
    }

    // Zero out motion after moving
    private void ApplyMotion(float deltaTime, Vector3 motion)
    {
        characterController.Move(deltaTime * motion);
        ForwardMotion = SidewaysMotion = 0;
    }
}