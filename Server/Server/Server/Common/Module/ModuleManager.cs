using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ModuleManager : Singleton<ModuleManager>
    {
        private Module[] moduleArr;
        private int moduleCount;
        private Dictionary<Type, Module> moduleDict = new Dictionary<Type, Module>();

        public void Init()
        {
            moduleArr = Utility.AssemblyUtil.GetInstanceByAttribute<ModuleAttribute, Module>(false);
            moduleCount = moduleArr.Length;

            for (int i = 0; i < moduleCount; i++)
            {
                moduleDict[moduleArr[i].GetType()] = moduleArr[i];
            }

            for (int i = 0; i < moduleCount; i++)
            {
                moduleArr[i].Init();
            }
        }
        public void Update()
        {
            for (int i = 0; i < moduleCount; i++)
            {
                moduleArr[i].Update();
            }
        }

        public T GetModule<T>()
            where T : Module
        {
            var type = typeof(T);
            if (moduleDict.ContainsKey(typeof(T)))
            {
                return moduleDict[type] as T;
            }
            return null;
        }
    }
}
