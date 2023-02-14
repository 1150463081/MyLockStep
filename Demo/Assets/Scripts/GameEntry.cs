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

            var TimeMgr= ModuleManager.Instance.GetModule<TimerManager>();
            Debug.Log("��ʱ��ʼ");
            TimeMgr.StartTimer(5, 0, 1, () =>
            {
                Debug.Log("��ʱ����");
            }, null);
        }
        private void Update()
        {
            ModuleManager.Instance.OnUpdate();
        }
        private void OnDestroy()
        {
            ModuleManager.Instance.OnDestroy();
        }

    }
}
