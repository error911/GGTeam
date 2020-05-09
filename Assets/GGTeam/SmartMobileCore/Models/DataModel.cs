// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    [Serializable]
    public abstract class DataModel
    {
        private readonly string filename_default = "";
        const string filePrefix = "GGTeam.SmartMobileCore.DataModel.";

        /// <summary>
        /// Сохранить
        /// </summary>
        public void Save()
        {
//Debug.Log("====File " + filename);
            _Save(filename_default);
        }

        /// <summary>
        /// Сохранить под другим именем
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            string s = this.filename_default + "." + filename;
            object data = base.MemberwiseClone();
            PlayerPrefs.SetString(s, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        private void _Save(string _filename)
        {
            object data = base.MemberwiseClone();
            PlayerPrefs.SetString(_filename, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
            //System.Attribute[] attrs = System.Attribute.GetCustomAttributes(o);
        }

        /// <summary>
        /// Загрузить
        /// </summary>
        /// <returns>успешно (true) или нет (false)</returns>
        public bool Load()
        {
            return _Load(filename_default);
        }

        /// <summary>
        /// Загрузить под другим именем
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Load(string filename)
        {
            string s = this.filename_default + "." + filename;
            return _Load(s);
        }

        private bool _Load(string _filename)
        {
            string s_data = PlayerPrefs.GetString(_filename, "");
            if (s_data.Length == 0) return false;
            JsonUtility.FromJsonOverwrite(s_data, this);
            return true;
        }


        /// <summary>
        /// Очистить
        /// </summary>
        public void Clear()
        {
            PlayerPrefs.DeleteKey(filename_default);
            PlayerPrefs.Save();
        }


        //[NonSerialized]
        //public int  LastCompletedLevel = -1;

        protected DataModel()
        {
            filename_default = filePrefix + base.GetType().Name;
        }

        /*
        public string QGet()
        {

            object o = base.MemberwiseClone();

            var t = o.GetType();
            var q = base.GetType();
            //var bt = t.MemberType;// BaseType;
            //var bt = base.GetType().GetNestedType(t.Name);
            foreach (var item in q.GetMembers())
            {
                // нет Method / DataModel / MyDataModel / Set_LastCompletedLevel
                // да  Field / MyDataModel / MyDataModel / privet_mir
                //if (item.MemberType.GetType() == item.DeclaringType) Debug.Log("!!!!!!!!!!!!!!!");
                Debug.Log(item.MemberType + " / " + item.DeclaringType + " / " + item.ReflectedType + " / " + item.Name);
                //Debug.Log(item.Name);
            }
            //Debug.Log(bt.Name);
            //System.Attribute[] attrs = System.Attribute.GetCustomAttributes(o);
            //Debug.Log();

            return JsonUtility.ToJson(q.MemberType.GetType().Name);
        }
        */

        //private DataHeader<T> data;

        //public GameDataModel(DataHeader<T> data)
        //{
        //    this.data = data;
        //}

        //!public void Set_LastCompletedLevel(int value)
        //{
        //    LastCompletedLevel = value;
            //data.SaveData();
        //}

        //public string ToJson() => JsonUtility.ToJson(this);

        //public void FromJson(string dataJson)
        //{
        //    var data = JsonUtility.FromJson<GameDataModel>(dataJson);

        //    LastCompletedLevel = data.LastCompletedLevel;
        //}

    }
}

//[System.AttributeUsage(System.AttributeTargets.Class |
//                       System.AttributeTargets.Struct |
//                        System.AttributeTargets.Field)
//]

[System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
public class DataToFile : System.Attribute
{
    private string name;
    public double version;

    public DataToFile(string name)
    {
        this.name = name;
        version = 1.0;
    }
}


[System.AttributeUsage(System.AttributeTargets.Class)]
public class UniqueId : System.Attribute
{
    private int id;

    public UniqueId(int id)
    {
        this.id = id;
    }
}
