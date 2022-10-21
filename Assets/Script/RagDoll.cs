using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    public Rigidbody[] rbodys;
    private Animator anim;
    WaitForSeconds ws;


    BoxCollider bc;
    GameObject max;

    void Start()
    {
        anim = GetComponent<Animator>();

        rbodys = GetComponentsInChildren<Rigidbody>();
        SetRagDoll(true);//래그돌 비활성화
        ws = new WaitForSeconds(5.0f);
        StartCoroutine(WakeUpRagDoll());
    }

   void SetRagDoll(bool isEndable)
    {
        foreach(Rigidbody _rbody in rbodys)
        {
            _rbody.isKinematic = !isEndable;
        }   
    }

    public void OnCollisionEnter(Collision cols)
    {
        if(cols.gameObject.name == "Car")
        {
            anim.enabled = false;
            SetRagDoll(true);
        }
    }

    IEnumerator WakeUpRagDoll()
    {
        yield return ws;
        anim.enabled = false;
        SetRagDoll(true);
    }
   
}
