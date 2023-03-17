using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LockStepFrame;

namespace GameCore 
{
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            ModuleManager.Instance.Init();
        }
        private void Start()
        {
            ModuleManager.Instance.OnStart();

        }
        private void Update()
        {
            ModuleManager.Instance.OnUpdate();
        }
        private void FixedUpdate()
        {
            ModuleManager.Instance.OnFixedUpdate();
        }
        private void OnDestroy()
        {
            ModuleManager.Instance.OnDestroy();
        }

    }
}
