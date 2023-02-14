using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LockStepFrame
{
	public partial class Utility
	{
		public class AssemblyUtil
		{
			/// <summary>
			/// 获取类的所有派生类
			/// 必须为非抽象类
			/// </summary>
			/// <typeparam name="T">基类</typeparam>
			/// <param name="assembly"></param>
			/// <returns></returns>
			public static Type[] GetDerivedTypes<T>( Assembly assembly = null)
				where T : class
			{
				Type type = typeof(T);
				Type[] types;
				if (assembly == null)
				{
					types = type.Assembly.GetTypes();
				}
				else
				{
					types = assembly.GetTypes();
				}
				types = types.Where(t => { return type.IsAssignableFrom(t) && type.IsClass && !type.IsAbstract; }).ToArray();
				return types;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <typeparam name="T">目标特性</typeparam>
			/// <typeparam name="K">基类</typeparam>
			public static K[] GetInstanceByAttribute<T,K>(bool inherit)
				where K:class
				where T:Attribute
			{
				List<K> result = new List<K>();
				var types = GetDerivedTypes<K>();
				var len = types.Length;
                for (int i = 0; i < len; i++)
                {
                    if (types[i].GetCustomAttributes<T>(inherit).Count() > 0)
                    {
						result.Add(GetTypeInstance(types[i]) as K);
                    }
                }
				return result.ToArray();
			}

			/// <summary>
			/// 反射工具，得到反射类的对象；
			/// 不可反射Mono子类，被反射对象必须是具有无参公共构造
			/// </summary>
			/// <param name="type">类型</param>
			/// <returns>实例化后的对象</returns>
			public static object GetTypeInstance(Type type)
			{
				return Activator.CreateInstance(type);
			}
		}
	}
}
