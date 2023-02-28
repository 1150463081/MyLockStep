using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore
{
    public class HeroEntity : EntityBase
    {
        public bool IsMain { get; private set; }
        public void InitHero(bool isMain)
        {
            EntityType = EntityType.Hero;
            IsMain = isMain;
            LoadRes();
        }
        private void LoadRes()
        {
            try
            {
                string name = IsMain ? $"MyHero{Url}" : $"Hero{Url}";
                GameObject root = new GameObject(name);
                SetObj(root);
                //临时使用方块作为模型
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.SetParent(transform);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.ToString()}");
            }
        }
    }
}
