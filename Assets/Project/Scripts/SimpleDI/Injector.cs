using System;
using System.Collections.Generic;
using System.Reflection;

using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;

namespace Assets.Project
{
    public class Injector 
    {
        private readonly Dictionary<Type, Type> registeredTypesMap;
        protected readonly Dictionary<Type, object> resolvedInstancesMap;

        private readonly ContainerDI container;
        private Type typeKey;

        public Injector(Dictionary<Type, Type> registeredTypesMap, Dictionary<Type, object> resolvedInstancesMap, ContainerDI container = null)
        {
            this.registeredTypesMap = registeredTypesMap ?? throw new ArgumentNullException(nameof(registeredTypesMap));
            this.resolvedInstancesMap = resolvedInstancesMap ?? throw new ArgumentNullException(nameof(resolvedInstancesMap));
            this.container = container;
        }

        public virtual void PerfomInject(Type type, object target = null)
        {
            InjectConstructor(type, type);
        }

        private void InjectConstructor(Type type, Type typeKey = null)
        {
            ConstructorInfo constructor = GetConstructor(type);
            object[] args = GetAllParametersType(constructor);

            object obj = constructor.Invoke(args);

            if(typeKey!= null)
            {
                resolvedInstancesMap[typeKey] = obj;
            }
            else
            {
                resolvedInstancesMap[type] = obj;
            }
               
          
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetDeclaredConstructors();
            ConstructorInfo constructor = constructors[0];

            if (constructor == null)
            {
                var con = type.GetConstructor(Type.EmptyTypes);
                return con;
            }

            return constructor;
        }

        private object[] GetAllParametersType(ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] args = new object[parameters.Length];

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                    args[i] = GetObject(parameters[i].ParameterType);
            }

            return args;
        }

        protected object GetObject(Type type)
        {
            if (!resolvedInstancesMap.ContainsKey(type))
            {
                if (registeredTypesMap.ContainsKey(type))
                {
                    InjectConstructor(registeredTypesMap[type],type);
                    return resolvedInstancesMap[type];
                }
                else
                {
                    if (container != null)
                        return ForeachContainer(container, type);
                    else
                        throw new NullReferenceException($"No type in Containers {type}");
                }
            }

            return resolvedInstancesMap[type];
        }

        private object ForeachContainer(ContainerDI container, Type type)
        {
            if (!container.injectedsMap.ContainsKey(type))
            {
                if (container.registratedsMap.ContainsKey(type))
                {
                    InjectConstructor(container.registratedsMap[type]);
                    return container.injectedsMap[type];
                }
                else
                {
                    throw new NullReferenceException($"No type in registratedsMap {type}");
                }
            }

            return container.injectedsMap[type];
        }
    }
   
}