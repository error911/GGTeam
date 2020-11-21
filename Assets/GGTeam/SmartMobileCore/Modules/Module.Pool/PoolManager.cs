using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    public class PoolManager
    {
        private Dictionary<Type, IPool> _pools = new Dictionary<Type, IPool>();
        private GameObject _root;


        //public override void Initialize()   //Transform poolRootObj
        //{
        //    Core.Coroutine.Start(CleanUpCycle());
        //}

        //public IPool PutElement(PoolElement prefab, int count = 1)// where T : PoolElement
        //{
        //    IPool pool;

        //    if (Exists(typeof(PoolElement)))
        //    {
        //        pool = GetPool<PoolElement>();
        //    }
        //    else
        //    {
        //        pool = CreatePool<PoolElement>(prefab);
        //    }

        //    pool.PutElement(prefab, count);

        //    return pool;
        //}

        public IPool PutElement<T>(T prefab, int count = 1) where T : SmartMobileCore.PoolElement
        {
            IPool pool;

            if (Exists(typeof(T)))
            {
                pool = GetPool<T>();
            }
            else
            {
                pool = CreatePool<T>(prefab);
            }

            pool.PutElement(prefab, count);

            return pool;
        }

        /// <summary>
        /// Положить элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="count"></param>
        /// <param name="startCleaningCycle"></param>
        /// <returns></returns>
        public IPool PutElement<T>(T prefab, int count = 1, bool startCleaningCycle = false) where T : PoolElement
        {
            IPool pool = PutElement(prefab, count);

            ((Pool)pool).CleanUp(startCleaningCycle);

            return pool;
        }

        /// <summary>
        /// Взять элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newParent"></param>
        /// <returns></returns>
        public T GetElement<T>(Transform newParent = null) where T : PoolElement
        {
            IPool pool;

            if (Exists(typeof(T)))
            {
                pool = GetPool<T>();
            }
            else
            {
                Debug.Log("Не найден пул типа: \"" + typeof(T).Name + "\". Пожалуйста, положите в него объект нужного типа перед вызовом операции GetElement<" + typeof(T).Name + ">");
                return null;
            }

            return pool.GetElements<T>(1, newParent)[0];
        }

        /// <summary>
        /// Взять несколько элементов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="newParent"></param>
        /// <returns></returns>
        public T[] GetElements<T>(int count = 1, Transform newParent = null) where T : PoolElement
        {
            IPool pool;

            if (Exists(typeof(T)))
            {
                pool = GetPool<T>();
            }
            else
            {
                Debug.Log("Не найден пул типа: \"" + typeof(T).Name + "\". Пожалуйста, положите в него объект нужного типа перед вызовом операции GetElements<" + typeof(T).Name + ">");
                return null;
            }

            return pool.GetElements<T>(count, newParent);
        }


        public IPool GetPool<T>() where T : PoolElement
        {
            return _pools[typeof(T)];
        }


        private void CreateModuleRoot()
        {
            _root = new GameObject("[PoolModule]");
            //        _root.transform.SetParent(Core.Root);
        }


        private bool Exists(Type type)
        {
            return _pools.ContainsKey(type);
        }


        private IPool CreatePool<T>(PoolElement originalPrefab) where T : PoolElement
        {
            var type = typeof(T);

            var poolRoot = new GameObject("[Pool of type]: " + type.Name);

            if (!_root)
            {
                CreateModuleRoot();
            }

            poolRoot.transform.SetParent(_root.transform);

            var pool = poolRoot.AddComponent<Pool>();

            pool.Initialize(originalPrefab);

            _pools.Add(type, pool);

            return pool;
        }


        private IEnumerator CleanUpCycle()
        {
            while (PoolConfig.PoolCleanUpRate > 0)
            {
                yield return new WaitForEndOfFrame();

                foreach (var pool in _pools)
                {
                    ((Pool)pool.Value).CleanCycle();
                }
            }
        }
    }
}