using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : MonoBehaviour
{
    [Header("Wheel Collider")]
    public WheelCollider frontRight;
    public WheelCollider frontLeft;
    public WheelCollider rearRight;
    public WheelCollider rearLeft;

    [Header("Wheel Transform")]
    public Transform frontRightTr;
    public Transform frontLeftTr;
    public Transform rearRightTr;
    public Transform rearLeftTr;

    [Header("Center Of Mass")]
    [SerializeField]
    Vector3 centerOfMass;

    [Header("앞바퀴의 최대 회전각")]
    [SerializeField]
    float maxSteer;

    [Header("배퀴의 최대 마찰력")]
    [SerializeField]
    float maxTorque;

    [Header("바퀴의 최대 브레이크 값")]
    [SerializeField]
    float maxBrake;

    [Header("현재 스피드")]
    float currentSpeed;

    [Header("Lights")]
    [SerializeField]
    HeadlightController headlightController;
    [SerializeField]
    BacklightController backlightController;
 
    Rigidbody rb;
    float steer = 0f;
    float forward = 0f;
    float back = 0f;
    bool isReverse = false;
    float motor = 0f;
    float brake = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    void FixedUpdate() // 프레임이 일정
    {
        currentSpeed = rb.velocity.sqrMagnitude;
        steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f); // A, D
        forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f); // W키만 받음
        back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 0f); // S키만 받음

        if (Input.GetKey(KeyCode.W))
            StartCoroutine(ForwardCar());
        if (Input.GetKey(KeyCode.S))
            StartCoroutine(BackwardCar());
        if (Input.GetKey(KeyCode.F))
        {
            if (headlightController.isOn)
                headlightController.TurnOff();
            else
                headlightController.TurnOn();
        }

        if (isReverse)
        {
            motor = -1 * back;
            brake = forward;
            backlightController.TurnOn();
        }
        else
        {
            motor = forward;
            brake = back;
            backlightController.TurnOff();
        }

        // 뒷바퀴 torque 회전력
        rearLeft.motorTorque = maxTorque * motor;
        rearRight.motorTorque = maxTorque * motor;

        // 앞바퀴 steer 회전각도
        frontLeft.steerAngle = maxSteer * steer;
        frontRight.steerAngle = maxSteer * steer;

        // 바퀴 모델링도 휠 콜라이더랑 같이 회전시켜야 함
        frontRightTr.localEulerAngles = new Vector3(frontRightTr.localEulerAngles.x, steer * maxSteer, frontRightTr.localEulerAngles.z);
        frontLeftTr.localEulerAngles = new Vector3(frontLeftTr.localEulerAngles.x, steer * maxSteer, frontLeftTr.localEulerAngles.z);

        // 휠 콜라이더의 RPM값을 읽어서 모델링도 같이 회전
        frontLeftTr.Rotate(frontLeft.rpm * Time.deltaTime, 0f, 0f);
        frontRightTr.Rotate(frontRight.rpm * Time.deltaTime, 0f, 0f);
        rearRightTr.Rotate(rearRight.rpm * Time.deltaTime, 0f, 0f);
        rearLeftTr.Rotate(rearLeft.rpm * Time.deltaTime, 0f, 0f);
    }

    IEnumerator ForwardCar()
    {
        yield return new WaitForSeconds(0.1f);
        currentSpeed = 0f;
        if (back > 0)
            isReverse = true;
        if (forward > 0)
            isReverse = false;
    }

    IEnumerator BackwardCar()
    {
        yield return new WaitForSeconds(0.1f);
        currentSpeed = 0.1f;
        if (back > 0)
            isReverse = true;
        if (forward > 0)
            isReverse = false;
    }
}
