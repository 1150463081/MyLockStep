﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LockStepFrame
{
    public class Module
    {
        public virtual void OnInit()
        {
            Debug.Log($"[{GetType()}] OnInit");
        }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnDestroy() { }
    }
}
