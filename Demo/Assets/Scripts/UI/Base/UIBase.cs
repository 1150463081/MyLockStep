using LockStepFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class UIBase : MonoBehaviour
    {
        protected NetWorkMgr NetWorkMgr;

        private void Awake()
        {
           
            OnInit();
        }
        private void Start()
        {
            NetWorkMgr = ModuleManager.Instance.GetModule<NetWorkMgr>();
            OnStart();
        }
        private void OnEnable()
        {
            OnShow();
        }
        private void OnDisable()
        {
            OnClose();
        }
        protected virtual void OnDestroy()
        {
            
        }
        protected virtual void OnInit() { }
        protected virtual void OnStart() { }
        protected virtual void OnShow() { }
        protected virtual void OnClose() { }

        protected T GetModule<T>()
            where T : Module
        {
            return ModuleManager.Instance.GetModule<T>();
        }
    }
}
