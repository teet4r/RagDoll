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

    [Header("�չ����� �ִ� ȸ����")]
    [SerializeField]
    float maxSteer;

    [Header("������ �ִ� ������")]
    [SerializeField]
    float maxTorque;

    [Header("������ �ִ� �극��ũ ��")]
    [SerializeField]
    float maxBrake;

    [Header("���� ���ǵ�")]
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

    void FixedUpdate() // �������� ����
    {
        currentSpeed = rb.velocity.sqrMagnitude;
        steer = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f); // A, D
        forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f); // WŰ�� ����
        back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 0f); // SŰ�� ����

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

        // �޹��� torque ȸ����
        rearLeft.motorTorque = maxTorque * motor;
        rearRight.motorTorque = maxTorque * motor;

        // �չ��� steer ȸ������
        frontLeft.steerAngle = maxSteer * steer;
        frontRight.steerAngle = maxSteer * steer;

        // ���� �𵨸��� �� �ݶ��̴��� ���� ȸ�����Ѿ� ��
        frontRightTr.localEulerAngles = new Vector3(frontRightTr.localEulerAngles.x, steer * maxSteer, frontRightTr.localEulerAngles.z);
        frontLeftTr.localEulerAngles = new Vector3(frontLeftTr.localEulerAngles.x, steer * maxSteer, frontLeftTr.localEulerAngles.z);

        // �� �ݶ��̴��� RPM���� �о �𵨸��� ���� ȸ��
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
