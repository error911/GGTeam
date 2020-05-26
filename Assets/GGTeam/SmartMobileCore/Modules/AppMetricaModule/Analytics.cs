/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

// Uncomment the following line to disable location tracking
#define APP_METRICA_TRACK_LOCATION_DISABLED
// or just add APP_METRICA_TRACK_LOCATION_DISABLED into
// Player Settings -> Other Settings -> Scripting Define Symbols

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using UnityEngine.SocialPlatforms.Impl;

namespace GGTeam.SmartMobileCore
{

    public class Analytics : MonoBehaviour
    {
        public const string VERSION = "3.5.0";

        //[SerializeField]
        //private string ApiKey = "";

        //[SerializeField]
        private bool ExceptionsReporting = true;

        [SerializeField]
        private uint SessionTimeoutSec = 10;

        //[SerializeField]
        //private bool LocationTracking = true;

        //[SerializeField]
        //private bool Logs = true;

        [SerializeField]
        private bool HandleFirstActivationAsUpdate = false;

        [SerializeField]
        private bool StatisticsSending = true;

        private static bool _isInitialized = false;
        private bool _actualPauseStatus = false;

        private static IYandexAppMetrica _metrica = null;
        private static object syncRoot = new UnityEngine.Object();

        private GameManager Game
        {
            get
            {
                if (_Game != null) return _Game;
                _Game = FindObjectOfType<GameManager>();
                //if (_Game == null) { Debug.LogError("[GameManager] not found in root scene."); }
                return _Game;
            }
        }
        private GameManager _Game;
        private bool use_anilytics = true;
        private string deviceId = "";

        public IYandexAppMetrica Api => Instance;

        public static IYandexAppMetrica Instance
        {
            get
            {
                if (_metrica == null)
                {
                    lock (syncRoot)
                    {
#if UNITY_IPHONE || UNITY_IOS
                    if (_metrica == null && Application.platform == RuntimePlatform.IPhonePlayer) {
                        _metrica = new YandexAppMetricaIOS ();
                    }
#elif UNITY_ANDROID
                        if (_metrica == null && Application.platform == RuntimePlatform.Android)
                        {
                            _metrica = new YandexAppMetricaAndroid();
                        }
#endif
                        if (_metrica == null)
                        {
                            _metrica = new YandexAppMetricaDummy();
                        }
                    }
                }
                return _metrica;
            }
        }

        void SetupMetrica()
        {
            if (string.IsNullOrEmpty(Game.Config.Current.ANALYTICS_APP_KEY)) return;    // Отключаем аналитику
            var configuration = new YandexAppMetricaConfig(Game.Config.Current.ANALYTICS_APP_KEY)
            {
                SessionTimeout = (int)SessionTimeoutSec,
                Logs = Game.Config.Current.ANALYTICS_LOGS,
                HandleFirstActivationAsUpdate = HandleFirstActivationAsUpdate,
                StatisticsSending = StatisticsSending,
            };

#if !APP_METRICA_TRACK_LOCATION_DISABLED
            configuration.LocationTracking = Game.Config.Current.ANALYTICS_LOCATION_TRACKING;
            if (Game.Config.Current.ANALYTICS_LOCATION_TRACKING)
            {
                Input.location.Start();
            }
#else
        configuration.LocationTracking = false;
#endif

            Instance.ActivateWithConfiguration(configuration);
            ProcessCrashReports();
        }

        private void Awake()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                DontDestroyOnLoad(this.gameObject);
                SetupMetrica();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            if (!use_anilytics) return;

            Instance.RequestAppMetricaDeviceID(GetDevId);
            Instance.ResumeSession();
        }

        private void OnEnable()
        {
            ExceptionsReporting = Game.Config.Current.ANALYTICS_EXCEPTIONS_REPORTING;
            if (string.IsNullOrEmpty(Game.Config.Current.ANALYTICS_APP_KEY)) use_anilytics = false;

            if (!use_anilytics) return;

            if (ExceptionsReporting)
            {
#if UNITY_5 || UNITY_5_3_OR_NEWER
                Application.logMessageReceived += HandleLog;
#else
			Application.RegisterLogCallback(HandleLog);
#endif
            }
        }


        private void GetDevId(string arg1, YandexAppMetricaRequestDeviceIDError? arg2)
        {
            if (!string.IsNullOrEmpty(arg1))
            {
                deviceId = arg1;

                /*
                YandexAppMetricaUserProfile userProfile = new YandexAppMetricaUserProfile();
                
                UserProfile userProfile = new UserProfile(deviceId, deviceId, false);
                
                // Updating predefined attributes.
                .apply(Attribute.name().withValue("John"))
        .apply(Attribute.gender().withValue(GenderAttribute.Gender.MALE))
        .apply(Attribute.birthDate().withAge(24))
        .apply(Attribute.notificationsEnabled().withValue(false))
        // Updating custom attributes.
        .apply(Attribute.customString("string_attribute").withValue("string"))
        .apply(Attribute.customNumber("number_attribute").withValue(55))
        .apply(Attribute.customCounter("counter_attribute").withDelta(1))
        .build();
                
                // Setting the ProfileID using the method of the YandexMetrica class.
                Instance.SetUserProfileID(deviceId);

                // Sending the UserProfile instance.
                Instance.ReportUserProfile(userProfile);
                */
            }
        }
        


        private void OnDisable()
        {
            if (ExceptionsReporting)    //Game.Config.Current.ANALYTICS_EXCEPTIONS_REPORTING
            {
#if UNITY_5 || UNITY_5_3_OR_NEWER
                Application.logMessageReceived -= HandleLog;
#else
			Application.RegisterLogCallback(null);
#endif
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (_actualPauseStatus != pauseStatus)
            {
                _actualPauseStatus = pauseStatus;
                if (pauseStatus)
                {
                    Instance.PauseSession();
                }
                else
                {
                    Instance.ResumeSession();
                }
            }
        }

        public void ProcessCrashReports()
        {
            if (ExceptionsReporting)    //Game.Config.Current.ANALYTICS_EXCEPTIONS_REPORTING
            {
                var reports = CrashReport.reports;
                foreach (var report in reports)
                {
                    var crashLog = string.Format("Time: {0}\nText: {1}", report.time, report.text);
                    Instance.ReportError("Crash", crashLog);
                    report.Remove();
                }
            }
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                Instance.ReportError(condition, stackTrace);
            }
        }


        #region === Отчеты Игры ===
        public void Report_Loading()
        {
            if (!use_anilytics) return;
            //string report = "Loading" + testCounter++;
            string report = e_Loading;
            var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, parameters);
        }
        #endregion

        #region === Отчеты Уровней ===
        //+
        public void Report_LevelStart(int lvlNum, int score, float stars = 0)
        {
            if (!use_anilytics) return;
            string report = e_Level;
            var par3 = new Dictionary<string, object>(3) { { e_DevId, deviceId }, { e_Level_Score, score }, { e_Level_Stars, stars } };
            var par2 = new Dictionary<string, object>(1) { { e_Level_Start, par3 } };
            var par1 = new Dictionary<string, object>(1) { { e_Level_Number + "_" + lvlNum, par2 } };

            //string report = e_Level_Start + "_" + lvlNum;
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
        }

        //+
        public void Report_LevelComplete(int lvlNum, int score, float stars)
        {
            if (!use_anilytics) return;
            string report = e_Level;
            var par3 = new Dictionary<string, object>(3) { { e_DevId, deviceId }, { e_Level_Score, score }, { e_Level_Stars, stars } };
            var par2 = new Dictionary<string, object>(1) { { e_Level_Complete, par3 } };
            var par1 = new Dictionary<string, object>(1) { { e_Level_Number + "_" + lvlNum, par2 } };
            Instance.ReportEvent(report, par1);
            Instance.SendEventsBuffer();

            //if (!use_anilytics) return;
            //string report = e_Level_Complete + "_" + lvlNum;
            //var parameters = new Dictionary<string, object>(3) { { "devId", deviceId }, { "stars", stars }, { "score", score } };
            //Instance.ReportEvent(report, parameters);
            //Instance.SendEventsBuffer();
        }
        
        //+
        public void Report_LevelFailed(int lvlNum, int score, float stars)
        {
            if (!use_anilytics) return;
            string report = e_Level;
            var par3 = new Dictionary<string, object>(3) { { e_DevId, deviceId }, { e_Level_Score, score }, { e_Level_Stars, stars } };
            var par2 = new Dictionary<string, object>(1) { { e_Level_Failed, par3 } };
            var par1 = new Dictionary<string, object>(1) { { e_Level_Number + "_" + lvlNum, par2 } };

            //string report = e_Level_Failed + "_" + lvlNum;
            //var parameters = new Dictionary<string, object>(3) { { "devId", deviceId }, { "stars", stars }, { "score", score } };
            Instance.ReportEvent(report, par1);
            Instance.SendEventsBuffer();
        }

        //+
        public void Report_LevelRestart(int lvlNum, int score, float stars = 0)
        {
            if (!use_anilytics) return;

            string report = e_Level;
            var par3 = new Dictionary<string, object>(3) { { e_DevId, deviceId }, { e_Level_Score, score }, { e_Level_Stars, stars } };
            var par2 = new Dictionary<string, object>(1) { { e_Level_Restart, par3 } };
            var par1 = new Dictionary<string, object>(1) { { e_Level_Number + "_" + lvlNum, par2 } };

            //string report = e_Level_Restart + "_" + lvlNum;
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
        }
        #endregion

        #region === Отчеты Меню ===
        public void Report_MenuOpen()
        {
            /*
            if (!use_anilytics) return;
            string report = e_Menu_Open;
            var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, parameters);
            */
        }

        //+
        public void Report_MenuSound(bool on)
        {
            if (!use_anilytics) return;
            string report = e_GameMenu;
            var par2 = new Dictionary<string, object>(1) { { e_GameMenu_Toggle, on } };
            var par1 = new Dictionary<string, object>(1) { { e_GameMenu_Sound, par2 } };

            //string report = e_MenuSound + "_" + on.ToString();
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
        }

        //+
        public void Report_MenuVibro(bool on)
        {
            if (!use_anilytics) return;
            string report = e_GameMenu;
            var par2 = new Dictionary<string, object>(1) { { e_GameMenu_Toggle, on } };
            var par1 = new Dictionary<string, object>(1) { { e_GameMenu_Vibro, par2 } };

            //string report = e_MenuVibro + "_" + on.ToString();
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
        }

        //*
        public void Report_MenuRate()
        {
            if (!use_anilytics) return;
            string report = e_GameMenu;
            var par2 = new Dictionary<string, object>(1) { { e_GameMenu_Toggle, true } };
            var par1 = new Dictionary<string, object>(1) { { e_GameMenu_Rate, par2 } };

            //string report = e_MenuRate;
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
        }

        //+
        public void Report_MenuNoAds()
        {
            if (!use_anilytics) return;
            string report = e_GameMenu;
            var par2 = new Dictionary<string, object>(1) { { e_GameMenu_Toggle, true } };
            var par1 = new Dictionary<string, object>(1) { { e_GameMenu_NoAds, par2 } };

            //string report = e_MenuNoAds;
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
        }

        //+
        public void Report_MenuSelectLevel()
        {
            if (!use_anilytics) return;
            string report = e_GameMenu;
            var par2 = new Dictionary<string, object>(1) { { e_GameMenu_Toggle, true } };
            var par1 = new Dictionary<string, object>(1) { { e_GameMenu_SelectLevel, par2 } };

            //string report = e_MenuSelectLevel;
            //var parameters = new Dictionary<string, object>(1) { { "devId", deviceId } };
            Instance.ReportEvent(report, par1);
            
        }



        #endregion

        #region === Отчеты Покупок ===
        //+
        public void Report_PurchaseStart(string id, double price = 0, string currency = "")
        {
            if (!use_anilytics) return;
            if (string.IsNullOrEmpty(id)) return;
            string report = e_Purchase;
            var par3 = new Dictionary<string, object>(5) { { e_DevId, deviceId }, { e_Purchase_TransactionId, "" }, { e_Purchase_Price, price }, { e_Purchase_Currency, currency }, { e_Purchase_Error, "" } };
            var par2 = new Dictionary<string, object>(1) { { id, par3 } };
            var par1 = new Dictionary<string, object>(1) { { e_Purchase_Start, par2 } };

            //var par1 = new Dictionary<string, object>(1) { { e_Level_Number + "_" + lvlNum, par2 } };

            //string report = e_PurchaseStart + "_" + id;
            //var parameters = new Dictionary<string, object>(4) { { "devId", deviceId }, { "completed", "init" }, { "price", price }, { "currency", currency } };
            Instance.ReportEvent(report, par1);
            
            /*
            if (complete)
            {
                var c = new YandexAppMetricaRevenue(price, currency);
                c.ProductID = id;
                Instance.ReportRevenue(c);
            }
            */
            Instance.SendEventsBuffer();
        }


        // Declaration of the Receipt structure for getting information about the IAP.
        [System.Serializable]
        public struct Receipt
        {
            public string Store;
            public string TransactionID;
            public string Payload;
        }

        // Additional information about the IAP for Android.
        [System.Serializable]
        public struct PayloadAndroid
        {
            public string Json;
            public string Signature;
        }


        /// <summary>
        /// Покупка
        /// </summary>
        /// <param name="id"></param>
        /// <param name="complete">удачно или нет</param>
        public void Report_PurchaseComplete(string id, bool complete, double price = 0, string currency = "", string transactionId = "", string receipt = "", string errmes = "")
        {
            if (!use_anilytics) return;
            string report = e_Purchase;
            if (complete)
            {
                var par3 = new Dictionary<string, object>(5) { { e_DevId, deviceId }, { e_Purchase_TransactionId, transactionId }, { e_Purchase_Price, price }, { e_Purchase_Currency, currency }, { e_Purchase_Error, "" } };
                var par2 = new Dictionary<string, object>(1) { { id, par3 } };
                var par1 = new Dictionary<string, object>(1) { { e_Purchase_Complete, par2 } };
                Instance.ReportEvent(report, par1);
            }
            else
            {
                var par3 = new Dictionary<string, object>(5) { { e_DevId, deviceId }, { e_Purchase_TransactionId, transactionId }, { e_Purchase_Price, price }, { e_Purchase_Currency, currency }, { e_Purchase_Error, errmes } };
                var par2 = new Dictionary<string, object>(1) { { id, par3 } };
                var par1 = new Dictionary<string, object>(1) { { e_Purchase_Failed, par2 } };
                Instance.ReportEvent(report, par1);
            }

            //string report = e_PurchaseComplete + "_" + id;
            //var parameters = new Dictionary<string, object>(4) { { "devId", deviceId }, { "completed", complete.ToString() }, { "price", price }, { "currency", currency } };

            if (complete)
            {
                var revenue = new YandexAppMetricaRevenue(price, currency);
                revenue.ProductID = id;

                if (!string.IsNullOrEmpty(receipt))
                {
                    YandexAppMetricaReceipt yaReceipt = new YandexAppMetricaReceipt();
                    Receipt r_receipt = JsonUtility.FromJson<Receipt>(receipt);
r_receipt.TransactionID = transactionId;
#if UNITY_ANDROID
                    PayloadAndroid payloadAndroid = JsonUtility.FromJson<PayloadAndroid>(r_receipt.Payload);
                    yaReceipt.Signature = payloadAndroid.Signature;
                    yaReceipt.Data = payloadAndroid.Json;

                    yaReceipt.TransactionID = transactionId;
#elif UNITY_IPHONE
            yaReceipt.TransactionID = receipt.TransactionID;
            yaReceipt.Data = receipt.Payload;
#endif
                    revenue.Receipt = yaReceipt;
                }

                Instance.ReportRevenue(revenue);
            }
            Instance.SendEventsBuffer();
        }
        #endregion

        string e_Loading = "Loading";
        
//        string e_Level_Start = "Level_Start";
//        string e_Level_Complete = "Level_Complete";
//        string e_Level_Failed = "Level_Failed";
//        string e_Level_Restart = "Level_Restart";

        
//        string e_MenuSound = "Menu_Click_Sound";
//        string e_MenuVibro = "Menu_Click_Vibro";
//        string e_MenuRate = "Menu_Click_Rate";
//        string e_MenuNoAds = "Menu_Click_NoAds";
//        string e_MenuSelectLevel = "Menu_Click_SelectLevel";
        
//        string e_PurchaseStart = "PurchaseStart";
//        string e_PurchaseComplete = "PurchaseComplete";


        string e_Level = "Level";
            string e_Level_Number = "Number";
                string e_Level_Complete = "Level_Complete";
                    string e_DevId = "DevID";
                    string e_Level_Stars = "Level_Stars";
                    string e_Level_Score = "Level_Score";
                string e_Level_Failed = "Level_Failed";
                string e_Level_Restart = "Level_Restart";
                string e_Level_Start = "Level_Start";


//        string e_GameMenu_Open = "GameMenu_Open";
        string e_GameMenu = "GameMenu";
            string e_GameMenu_Sound = "Click_Sound";
                string e_GameMenu_Toggle = "Toggle";  // on off
            string e_GameMenu_Vibro = "Click_Vibro";
            string e_GameMenu_Rate = "Click_Rate";
            string e_GameMenu_NoAds = "Click_NoAds";
            string e_GameMenu_SelectLevel = "Click_SelectLevel";

        string e_Purchase = "Purchase";
            //string e_Purchase_Id = "Product_";
                string e_Purchase_Start = "Purchase_Start";
                string e_Purchase_Complete = "Purchase_Complete";
                string e_Purchase_Failed = "Purchase_Failed";
                string e_Purchase_Price = "Price";
                    string e_Purchase_Currency = "Currency";
                    //string e_Purchase_Receipt = "Receipt";
                    string e_Purchase_TransactionId = "TransactionId";
                    string e_Purchase_Error = "Error";
            


    }
}