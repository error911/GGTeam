using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GGTeam.Tools.Audio
{
    [Serializable]
    public sealed class SavableValue<T>
    {
        private readonly string playerPrefsPath;
        private T value;
        private T prevValue;
		#pragma warning disable
        
        /// <summary>
        /// Событие, вызываемое при изменении переменной
        /// </summary>
        public event Action OnChanged = () => { };

        /// <summary>
        /// Значение
        /// </summary>
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.prevValue = this.value;
                this.value = value;
                SaveToPrefs();
                OnChanged.Invoke();
            }
        }

        [Obsolete("Временное значение. Не желательно использовать")]
        public T PrevValue
        {
            get { return prevValue; }
        }
        

        public SavableValue(string playerPrefsPath, T defaultValue = default(T))
        {
			if (string.IsNullOrEmpty(playerPrefsPath))
				throw new Exception("Пустой PlayerPrefs путь в saveble value");

            this.playerPrefsPath = playerPrefsPath;

            value = defaultValue;
            prevValue = defaultValue;

            LoadFromPrefs();
        }

        private void LoadFromPrefs()
        {
            if (!PlayerPrefs.HasKey(playerPrefsPath))
            {
                SaveToPrefs();
                return;
            }

            var stringToDeserialize = PlayerPrefs.GetString(playerPrefsPath, "");

            var bytes = Convert.FromBase64String(stringToDeserialize);
            var memorystream = new MemoryStream(bytes);
            var bf = new BinaryFormatter();

            value = (T)bf.Deserialize(memorystream);
        }

        private void SaveToPrefs()
        {
            var memorystream = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(memorystream, value);
            var stringToSave = Convert.ToBase64String(memorystream.ToArray());

            PlayerPrefs.SetString(playerPrefsPath, stringToSave);
        }
    }
}