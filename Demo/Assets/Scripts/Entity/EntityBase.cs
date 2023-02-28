using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GameCore
{
    public enum EntityType
    {
        Hero,
    }
    public class EntityBase
    {
        public int Url { get; set; }
        public EntityType EntityType { get; protected set; }
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }

        public void SetObj(GameObject go)
        {
            gameObject = go;
            transform = go.transform;
        }
    }
}
