/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Spiderman : MonoBehaviour {

    private const float SPEED = 50f;

    [SerializeField] private Transform pfImpactEffect;
    private Spiderman_Base spidermanBase;
    private State state;

    private enum State {
        Normal,
        Attacking,
    }

    private void Awake() {
        spidermanBase = gameObject.GetComponent<Spiderman_Base>();
        SetStateNormal();
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            HandleMovement();
            HandleAttack();
            break;
        case State.Attacking:
            HandleAttack();
            break;
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
    }

    private void SetStateAttacking() {
        state = State.Attacking;
    }

    private void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            spidermanBase.PlayIdleAnim();
        } else {
            spidermanBase.PlayMoveAnim(moveDir);
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
    }

    private void HandleAttack() {
        if (Input.GetMouseButtonDown(0)) {
            SetStateAttacking();
            Vector3 attackDir = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;

            if (spidermanBase.IsPlayingPunchAnimation()) {
                spidermanBase.PlayKickAnimation(attackDir, (Vector3 impactPosition) => {
                    Transform impactEffect = Instantiate(pfImpactEffect, impactPosition, Quaternion.identity);
                    impactEffect.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(attackDir));
                }, SetStateNormal);
            } else {
                spidermanBase.PlayPunchAnimation(attackDir, (Vector3 impactPosition) => {
                    Transform impactEffect = Instantiate(pfImpactEffect, impactPosition, Quaternion.identity);
                    impactEffect.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(attackDir));
                }, SetStateNormal);
            }
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
}
