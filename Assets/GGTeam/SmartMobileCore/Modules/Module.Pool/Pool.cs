using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGTeam.SmartMobileCore.Modules.PoolModule
{
    public enum PoolExpandMethodType
    {
        Expand,
        Replace
    }

    internal sealed class Pool : MonoBehaviour, IPool
    {
        PoolExpandMethodType PoolExpandMethod = PoolExpandMethodType.Expand;

        private List<PoolElement> _pool;
        private List<PoolElement> _released;
        private PoolElement _original;
        private bool _cleanUp;
        private float _nextCleanTime;

        public int CountTotal => _pool.Count + _released.Count;
        public int CountReserved => _pool.Count;
        public int CountReleased => _released.Count;

        public void Initialize(PoolElement original)
        {
            _original = original;
            _pool = new List<PoolElement>();
            _released = new List<PoolElement>();

            gameObject.SetActive(false);
        }

        public IPoolElement GetElement(Transform newParent = null)
        {
            var element = GetAndRelease();

            if (!element)
            {
                switch (PoolExpandMethod)
                {
                    case PoolExpandMethodType.Expand:

                        PutElement(_original, 1);
                        element = GetAndRelease();

                        break;
                    case PoolExpandMethodType.Replace:

                        ReturnElement(_released.FirstOrDefault());
                        element = GetAndRelease();

                        break;
                }
            }

            if (element && newParent)
            {
                element.transform.SetParent(newParent);
                element.transform.SetPositionAndRotation(newParent.position, newParent.rotation);
            }

            RefreshCleanUpTime();

            return element;
        }

        public IPoolElement[] GetElements(int count, Transform newParent = null)
        {
            var result = new IPoolElement[count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetElement(newParent);
            }

            return result;
        }


        public T[] GetElements<T>(int count, Transform newParent = null) where T : PoolElement
        {
            var result = new T[count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetElement(newParent) as T;
            }

            return result;
        }


        public void PutElement(PoolElement element, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var x = Duplicate(element);

                x.Initialize(this);
                _pool.Add(x);

                RefreshCleanUpTime();
            }
        }


        public void ReturnElement(PoolElement element)
        {
            if (_released.Contains(element))
                _released.Remove(element);

            _pool.Add(element);

            element.OnReturn();

            element.transform.SetParent(transform);
            element.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }


        internal void CleanCycle()
        {
            if (!_cleanUp) return;

            if (_nextCleanTime >= PoolConfig.PoolCleanUpRate)
            {
                RemoveFirst();
                RefreshCleanUpTime();
            }
            else
            {
                _nextCleanTime += Time.deltaTime;
            }
        }


        internal void CleanUp(bool value)
        {
            _cleanUp = value;
            RefreshCleanUpTime();
        }


        private void RefreshCleanUpTime()
        {
            _nextCleanTime = 0;
        }


        private PoolElement Duplicate(PoolElement element)
        {
            if (element == null)
            {
                Debug.LogWarning("Не могу создать пустой элемент");
                return null;
            }

            var result = Instantiate(element, transform);

            if (!result.gameObject.activeSelf)
                result.gameObject.SetActive(true);

            return result;
        }


        private void RemoveFirst()
        {
            var element = (PoolElement)GetElement();

            _released.Remove(element);

            Destroy(element.gameObject);
        }


        private PoolElement GetAndRelease()
        {
            if (_pool.Count > 0)
            {
                var element = _pool[0];

                _pool.Remove(element);
                _released.Add(element);

                element.OnRelease();

                element.transform.SetParent(null);

                return element;
            }

            return default;
        }
    }
}