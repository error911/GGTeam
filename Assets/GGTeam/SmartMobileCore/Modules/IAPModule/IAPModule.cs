// ================================
// Free license: CC BY Murnik Roman
// Module
// ================================

#if UNITY_PURCHASING

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace GGTeam.SmartMobileCore.Modules.IAP
{
    [System.Serializable]
    public class ProductItem
    {
        [Tooltip("ID продукта как в магазине GooglePlay и AppStore")]
        [SerializeField] public string id;
        [Tooltip("Consumable: Расходуемый. NonConsumable: Продукт можно купить один раз и навсегда (например NoAds, Skin...)")]
        //[SerializeField] public bool consume;
        [SerializeField] public ProductType productType;
    }


    public class IAPModule : MonoBehaviour, IStoreListener
    {
        bool debugMode = false;

        [Tooltip("Название приложения в магазине GooglePlay и AppStore. Необходимое, для правильного отображения названия продукта.")]
        [SerializeField] public string removeAppCaption = "Game Name";
        [SerializeField] public List<ProductItem> allProducts = new List<ProductItem>();

        /// <summary>
        /// Событие, срабатываемое после успешной покупки данного товара
        /// </summary>
        public Action<PurchaseEventArgs> OnPurchaseCompleteListener;    //Product

        /// <summary>
        /// Событие, срабатываемое после НЕ успешной покупки данного товара
        /// </summary>
        public Action<Product, PurchaseFailureReason> OnPurchaseFailedListener;

        private List<IAPButton> activeButtons = new List<IAPButton>();

        // Allows outside sources to know whether the full initialization has taken place.
        public static bool initializationComplete;
        
        private static IAPModule instance;
        public static IAPModule Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance = this;
                    //CreateCodelessIAPStoreListenerInstance();
                    var f = FindObjectOfType<IAPModule>();
                    if (f != null) instance = f;
                    else
                    {
                        var go = new GameObject("[IAPManager]");
                        go.name = "[IAPManager]";
                        go.transform.SetSiblingIndex(1);
                        instance = go.AddComponent<IAPModule>();
                    }
                }
                return instance;
            }

        }
        

//        void Deb(string te)
//        {
//            if (debugText != null) debugText.text += ">" + te + "\r\n";
//        }


        public IStoreController StoreController
        {
            get { return storeController; }
        }

        public IExtensionProvider ExtensionProvider
        {
            get { return storeExtensionProvider; }
        }

        /// <summary>
        /// Тнформация о продукте
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product GetProduct(string productID)
        {
            if (storeController != null && storeController.products != null && !string.IsNullOrEmpty(productID))
            {
                return storeController.products.WithID(productID);
            }

            if (string.IsNullOrEmpty(productID)) Debug.LogError("IAPModule попытался получить продукт с пустым ID");
            else Debug.LogError("IAPModule попытался получить неизвестный продукт " + productID);  //IAPModule attempted to get unknown product
            return null;
        }

        public bool HasProductInCatalog(string productID)
        {
            foreach (var product in allProducts)
            {
                if (product.id == productID)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddButton(IAPButton button)
        {
            activeButtons.Add(button);
        }

        public void RemoveButton(IAPButton button)
        {
            activeButtons.Remove(button);
        }

        // ===========================


        private static IStoreController storeController;          // The Unity Purchasing system.
        private static IExtensionProvider storeExtensionProvider; // The store-specific Purchasing subsystems.

        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values 
        // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
        // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
        // specific mapping to Unity Purchasing's AddProduct, below.
//        public static string kProductIDConsumable = "consumable";
//        public static string kProductIDNonConsumable = "nonconsumable";
//        public static string kProductIDSubscription = "subscription";

        // Apple App Store-specific product identifier for the subscription product.
//        private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

        // Google Play Store-specific product identifier subscription product.
//        private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        void Awake()
        {
            //  instance = this;
            //if (instance == null)
            //{ // Экземпляр менеджера был найден
            instance = this; // Задаем ссылку на экземпляр объекта
            //}
            //else if (instance == this)
            //{ // Экземпляр объекта уже существует на сцене
            //    Destroy(gameObject); // Удаляем объект
            //}


            if (removeAppCaption == "Game Name") removeAppCaption = Application.productName;
            removeAppCaption = "(" + removeAppCaption + ")";

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            // If we haven't set up the Unity Purchasing reference
            if (storeController == null)
            {
                // Begin to configure our connection to Purchasing
                // Начинаем настраивать наше подключение к покупкам
                InitializePurchasing();
            }
        }


        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            StandardPurchasingModule module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            // Create a builder, first passing in a suite of Unity provided stores.
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

            foreach (var prod in allProducts)
            {
                builder.AddProduct(prod.id, prod.productType);

                /*
                if (prod.productType != ProductType.Subscription)
                    builder.AddProduct(prod.id, prod.productType);
                else
                    builder.AddProduct(prod.id, ProductType.Subscription, new IDs(){
                    { kProductNameAppleSubscription, AppleAppStore.Name },
                    { kProductNameGooglePlaySubscription, GooglePlay.Name },
                });
                */
            }


            /*
            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            // Continue adding the non-consumable product.
            builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
            // must only be referenced here. 
            builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
                { kProductNameAppleSubscription, AppleAppStore.Name },
                { kProductNameGooglePlaySubscription, GooglePlay.Name },
            });

            */
            
            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return storeController != null && storeExtensionProvider != null;
        }

        public void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = storeController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
//Deb("BuyProductID " + string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    if (debugMode) Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    storeController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
//Deb("BuyProductID #" + productId + " FAIL. Not purchasing product, either is not found");
                    //if (debugMode) Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    if (debugMode) Debug.Log("BuyProductID: НЕУДАЧА. Не покупаемый товар, либо не найден, либо недоступен для покупки");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
//Deb("Purchase failed because Purchasing was not initialized correctly");
                //Debug.LogError("Purchase failed because Purchasing was not initialized correctly");
                Debug.LogError("Покупка не удалась, потому что Покупка не была правильно инициализирована");

                foreach (var button in activeButtons)
                {
                    if (button.productId == productId)
                    {
                        button.OnPurchaseFailedCallback(null, UnityEngine.Purchasing.PurchaseFailureReason.PurchasingUnavailable);
                    }
                }
            }
        }


        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                if (debugMode) Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                if (debugMode) Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                    // no purchases are available to be restored.
                    if (debugMode) Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                if (debugMode) Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
//Deb("OnInitialized: PASS");
            if (debugMode)Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            // Общая система закупок, настроенная с продуктами для этого приложения.
            storeController = controller;
            
            // Store specific subsystem, for accessing device-specific store features.
            // Отдельная подсистема хранилища для доступа к функциям хранилища для конкретного устройства.
            storeExtensionProvider = extensions;

            initializationComplete = true;
            
            // == для IAP Buttons ===
            foreach (var button in activeButtons)
            {
                button.UpdateText();
            }
            // === end ===
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
//Deb("OnInitialized: FAILED");
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            // Настройка покупки не удалась. Проверьте причину ошибки. Рассмотрите эту причину для пользователя.
            if (debugMode) Debug.Log("OnInitializeFailed Причина ошибки инициализации:" + error);  //InitializationFailureReason
        }


        // Вызывается интерфейсом
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // == для IAP Buttons ===

            PurchaseProcessingResult result;

            // if any receiver consumed this purchase we return the status
            bool consumePurchase = false;
            bool resultProcessed = false;

            foreach (IAPButton button in activeButtons)
            {
                if (button.productId == args.purchasedProduct.definition.id)
                {
                    var pr = allProducts.Where((x) => x.id == button.productId).SingleOrDefault();
                    bool consumable = false;
                    if (pr != null) if (pr.productType == ProductType.Consumable) consumable = true; else consumable = false;

                    result = button.OnProcessPurchaseCallback(args, consumable);  //, consumable

                    if (result == PurchaseProcessingResult.Complete)
                    {
                        consumePurchase = true;
                    }
                    resultProcessed = true;
                }
            }

            if (!resultProcessed)
            {

                //Debug.LogError("Purchase not correctly processed for product \"" +
                //                 args.purchasedProduct.definition.id +
                //                 "\". Add an active IAPButton to process this purchase, or add an IAPListener to receive any unhandled purchase events.");

                Debug.LogError("Покупка не правильно обработана для продукта \"" +
                                     args.purchasedProduct.definition.id +
                                     "\". Добавьте активную кнопку IAP для обработки этой покупки, или подпишитесь на события IAPModule для получения любых необработанные событий покупки.");
            }

            var r = (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;

            OnPurchaseCompleteListener?.Invoke(args);

            return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;

            // === end ===

            /*
                        // A consumable product has been purchased by this user.
                        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
                        {
                            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                            //1            ScoreManager.score += 100;
                        }
                        // Or ... a non-consumable product has been purchased by this user.
                        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
                        {
                            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                        }
                        // Or ... a subscription product has been purchased by this user.
                        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
                        {
                            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                            // TODO: The subscription item has been successfully purchased, grant this to the player.
                        }
                        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
                        else
                        {
                            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
                        }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
            */
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // == для IAP Buttons ===

            //bool resultProcessed = false;
            foreach (IAPButton button in activeButtons)
            {
                if (button.productId == product.definition.id)
                {
                    button.OnPurchaseFailedCallback(product, failureReason);
                    //resultProcessed = true;
                }
            }
            // === end ===

            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            if (debugMode) Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

            OnPurchaseFailedListener?.Invoke(product, failureReason);
        }
    }
}

#endif