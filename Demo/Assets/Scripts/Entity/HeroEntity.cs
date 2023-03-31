using NetProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LockStepFrame;


namespace GameCore
{
    public class HeroEntity : OperableEntity, ISyncUnit
    {
        public HeroVO HeroVO => BaseVO as HeroVO;
        public uint NetUrl { get; private set; }
        public bool IsMain { get; private set; }

        public void InitHero(bool isMain, uint netUrl)
        {
            OnInit();
            EntityType = EntityType.Hero;
            IsMain = isMain;
            NetUrl = netUrl;
            LoadRes();
        }


        public void InputKey(OpKey opKey)
        {
            switch (opKey.KeyType)
            {
                case OpKeyType.Move:
                    FXInt x = FXInt.zero;
                    x.ScaledValue = opKey.MoveKey.X_Value;
                    FXInt z = FXInt.zero;
                    z.ScaledValue = opKey.MoveKey.Z_Value;
                    InputMove(new FXVector3(x, 0, z));
                    break;
            }
        }



        private void LoadRes()
        {
            try
            {
                string name = IsMain ? $"MyHero{Url}" : $"Hero{Url}";
                GameObject root = new GameObject(name);
                SetObj(root);
                //临时使用方块作为模型
                GameObject prefab = Resources.Load<GameObject>("Cube");
                GameObject go = GameObject.Instantiate(prefab);
                go.transform.SetParent(transform);
                OnLoadResComplete();
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.ToString()}");
            }
        }
    }
}
