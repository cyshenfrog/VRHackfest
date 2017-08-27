﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public float unlockTime = 1;
    public float timer = 0;
    public Transform[] bodies;
    public bool unlocked;
    public Action unlockedEvent;
    public bool allowToCheck;
    public int unlockCount = 0;

	private void Start ()
    {
		
	}

    private void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Lock key = other.GetComponent<Lock>();
        if (key != null)
        {
            if (!key.done)
            {
                allowToCheck = false;
                return;
            }
            if (!key.solved ||
                (key.id == 1 && bodies[0].gameObject.activeSelf))
            {
                allowToCheck = false;
                key.Reset();
                return;
            }
        }
        allowToCheck = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Lock key = other.GetComponent<Lock>();
        if (allowToCheck && key != null)
        {
            Game.Instance.info.text = timer.ToString();
            timer += Time.deltaTime;
            if (timer >= unlockTime)
            {
                timer = 0;
                bodies[key.id].gameObject.SetActive(false);
                key.Reset();
                unlockCount++;
                if (unlockCount == bodies.Length)
                {
                    unlocked = true;
                    if (unlockedEvent != null)
                    {
                        unlockedEvent();
                    }
                }
            }
        }
        else
        {
            timer = 0;
        }
    }
}