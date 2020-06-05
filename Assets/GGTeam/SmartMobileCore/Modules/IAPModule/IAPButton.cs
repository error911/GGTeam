// ================================
// Free license: CC BY Murnik Roman
// Component Module
// ================================

#if UNITY_PURCHASING

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace GGTeam.SmartMobileCore.Modules.IAP
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("GGTeam/SmartMobileCore/Modules/IAP/IAP-Button")]

    public class IAPButton : MonoBehaviour
    {
        readonly string MES_PURCHASED = "purchased";

        bool aded = false;
        bool debugMode = true;

        string localizedTitle = "";
        string localizedDescription = "";
        string localizedPrice = "";
        bool localizedInter = true;
[HideInInspector] public bool clicked = false;

        public enum ButtonType
        {
            Purchase,
            Restore
        }

        [System.Serializable]
        public class OnPurchaseCompletedEvent : UnityEvent<Product>
        {
        };

        [System.Serializable]
        public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason>
        {
        };

        [HideInInspector]
        public string productId;

        //[HideInInspector]
        //public Image productImage;

        // The type of this button, can be either a purchase or a restore button
        [Tooltip("Тип этой кнопки может быть кнопкой покупки или восстановления.")]
        public ButtonType buttonType = ButtonType.Purchase;

        //Consume the product immediately after a successful purchase
        //        [Tooltip("Израсходовать продукт сразу после успешной покупки")]

        //        [Tooltip("Да: Расходуемый. Нет: Продукт можно купить один раз и навсегда (например NoAds)")]
        //        public bool consumePurchase = true;

        // Event fired after a successful purchase of this product
        [Tooltip("Событие, срабатываемое после успешной покупки данного товара")]
        public OnPurchaseCompletedEvent onPurchaseComplete;

        // Event fired after a failed purchase of this product
        [Tooltip("Событие, срабатываемое после неудачной покупки этого продукта")]
        public OnPurchaseFailedEvent onPurchaseFailed;

        [Tooltip("[Необязательно] Изображение продукта.")]
        public Image productImage;

        // [Optional] Displays the localized title from the app store
        [Tooltip("[Необязательно] Отображает локализованный заголовок из магазина приложений.")]
        public Text titleText;

        // [Optional] Displays the localized price from the app store
        [Tooltip("[Необязательно] Отображение локализованной цены из магазина приложений.")]
        public Text priceText;

        // [Optional] Displays the localized description from the app store
        [Tooltip("[Необязательно] Отображает локализованное описание из магазина приложений.")]
        public Text descriptionText;

        Button button;
        [HideInInspector] public ProductItem productItem
        {
            get
            {
                return _productItem;
            }
            set
            {
                _productItem = value;
                if (_productItem.productImage != null) productImage.sprite = _productItem.productImage;
            }
        }
        ProductItem _productItem;

        void RedrawProductInfo()
        {
            if (productItem == null) productItem = IAPModule.Instance.GetLocalProductInCatalogue(productId);
            if (productItem == null) return;
            if (productItem.productImage != null) productImage.sprite = _productItem.productImage;
            if (!string.IsNullOrEmpty(productItem.productTitle) && titleText != null) titleText.text = productItem.productTitle;
            if (!string.IsNullOrEmpty(productItem.productDescription) && descriptionText != null) descriptionText.text = productItem.productDescription;
        }

        void Start()
        {
            clicked = false;
            RedrawProductInfo();
            button = GetComponent<Button>();
            if (string.IsNullOrEmpty(productItem.productTitle)) if (titleText != null) titleText.text = "Loading..";
            if (priceText != null) priceText.text = "";
            if (string.IsNullOrEmpty(productItem.productDescription)) if (descriptionText != null) descriptionText.text = "";

            if (buttonType == ButtonType.Purchase)
            {
                if (button)
                {
                    button.onClick.AddListener(PurchaseProduct);
                }

                if (string.IsNullOrEmpty(productId))
                {
                    // IAPButton productId is empty
                    Debug.LogError("ID продукта IAPButton пуст");
                    if (titleText != null) titleText.text = "Empty";
                    if (button) button.interactable = false;
                    return;
                }


                if (!IAPModule.Instance.HasProductInCatalog(productId))
                {
                    // The product catalog has no product with the ID
                    Debug.LogWarning("В каталоге товаров нет товара с ID \"" + productId + "\"");
                    if (titleText != null) titleText.text = "Unknown";
                    if (button) button.interactable = false;
                    return;
                }


                // Проверка на купленность (если кнопка была скрыта при инициализации)
                if (IAPModule.Instance != null)
                {
                    StartCoroutine(WaitAndDo());
                    /*
                    if (IAPModule.initializationComplete)
                    {
                        if (IAPModule.Instance.CheckPurchasedProduct(productId))
                        {
                            if (button) { button.interactable = false; }
                            //UpdateText();
                            return;
                        }
                    }
                    */
                }


            }
            else if (buttonType == ButtonType.Restore)
            {
                if (button)
                {
                    button.onClick.AddListener(Restore);
                }
            }

            if (!aded) AddToList();
        }


        IEnumerator WaitAndDo()
        {
            //yield return new WaitForSeconds(waitTime);
            yield return new WaitForEndOfFrame();
            if (IAPModule.initializationComplete)
            {
                if (IAPModule.Instance.CheckPurchasedProduct(productId))
                {
                    if (button) { button.interactable = false; }
                    //UpdateText();
                    //return;
                }
            }
        }

        void AddToList()
        {
            if (buttonType == ButtonType.Purchase)
            {
                if (IAPModule.Instance == null) { aded = false; return; }

                IAPModule.Instance.AddButton(this);
                if (IAPModule.initializationComplete)
                {
                    UpdateText();
                }
            }
            aded = true;
        }

        void OnEnable()
        {
            if (!aded) AddToList();
        }

        void OnDisable()
        {
            if (IAPModule.Instance == null) return;
            if (buttonType == ButtonType.Purchase)
            {
                IAPModule.Instance.RemoveButton(this);
            }
            aded = false;
        }

        void PurchaseProduct()
        {
            if (buttonType == ButtonType.Purchase)
            {
                if (debugMode) Debug.Log("IAPButton.BuyProductID() with product ID: " + productId); //PurchaseProduct
clicked = true;
                IAPModule.Instance.BuyProductID(productId); // InitiatePurchase
            }
        }

        void Restore()
        {
            if (buttonType == ButtonType.Restore)
            {
                if (Application.platform == RuntimePlatform.WSAPlayerX86 ||
                    Application.platform == RuntimePlatform.WSAPlayerX64 ||
                    Application.platform == RuntimePlatform.WSAPlayerARM)
                {
                    IAPModule.Instance.ExtensionProvider.GetExtension<IMicrosoftExtensions>()   //CodelessIAPStoreListener
                        .RestoreTransactions();
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                         Application.platform == RuntimePlatform.OSXPlayer ||
                         Application.platform == RuntimePlatform.tvOS)
                {
                    IAPModule.Instance.ExtensionProvider.GetExtension<IAppleExtensions>()    //CodelessIAPStoreListener
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                         StandardPurchasingModule.Instance().appStore == AppStore.SamsungApps)
                {
                    IAPModule.Instance.ExtensionProvider.GetExtension<ISamsungAppsExtensions>() //CodelessIAPStoreListener
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                         StandardPurchasingModule.Instance().appStore == AppStore.CloudMoolah)
                {
                    IAPModule.Instance.ExtensionProvider.GetExtension<IMoolahExtension>()    //CodelessIAPStoreListener
                        .RestoreTransactionID((restoreTransactionIDState) =>
                        {
                            OnTransactionsRestored(
                                restoreTransactionIDState != RestoreTransactionIDState.RestoreFailed &&
                                restoreTransactionIDState != RestoreTransactionIDState.NotKnown);
                        });
                }
                else
                {
                    // is not a supported platform for the Codeless IAP restore button
                    Debug.LogWarning(Application.platform.ToString() + " is not a supported platform for the Codeless IAP restore button");
                }
            }
        }

        void OnTransactionsRestored(bool success)
        {
            if (debugMode) Debug.Log("Transactions restored: " + success);
        }


        /**
         *  Invoked to process a purchase of the product associated with this button
         *  Вызывается для обработки покупки продукта, связанного с этой кнопкой
         */
        public PurchaseProcessingResult OnProcessPurchaseCallback(PurchaseEventArgs e, bool consumePurchase)    //, bool consumePurchase
        {
            //Deb("Купили!! " + e.purchasedProduct.definition.id);
            //if (onPurchaseComplete == null) Deb("onPurchaseComplete == null");

            if (debugMode) Debug.Log(string.Format("IAPButton.ProcessPurchase(PurchaseEventArgs {0} - {1})", e, e.purchasedProduct.definition.id));
            onPurchaseComplete.Invoke(e.purchasedProduct);
            UpdateText();
            return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
        }

        /**
         *  Invoked on a failed purchase of the product associated with this button
         *  Вызывается при неудачной покупке продукта, связанного с этой кнопкой
         */
        public void OnPurchaseFailedCallback(Product product, PurchaseFailureReason reason)
        {
            //Deb("НЕ КУПИЛИ!! " + product.definition.id + ", " + reason.ToString());
            if (debugMode) Debug.Log(string.Format("IAPButton.OnPurchaseFailed(Product {0}, PurchaseFailureReason {1})", product, reason));
            onPurchaseFailed.Invoke(product, reason);
        }

        /*
        IEnumerator WaitUpdateText()
        {
            //yield return new WaitForSeconds(waitTime);
            yield return new WaitForEndOfFrame();
            var product = IAPModule.Instance.GetProduct(productId);  // CodelessIAPStoreListener
            if (product != null)
            {
                //product.hasReceipt - куплен purchased
                if (titleText != null)
                {
                    string str = product.metadata.localizedTitle;
                    str = str.Replace(IAPModule.Instance.removeAppCaption, "");
                    str = str.Trim();
                    titleText.text = str;
                }

                if (descriptionText != null)
                {
                    descriptionText.text = product.metadata.localizedDescription;
                }

                if (priceText != null)
                {
                    priceText.text = product.metadata.localizedPriceString;
                }

                if (product.hasReceipt)
                {
                    if (product.definition.type == ProductType.NonConsumable)
                    {
                        priceText.text = MES_PURCHASED;
                        button.interactable = false;
                    }
                }
            }
        }
        */


        private async void WaitForUpdateText()
        {
            UnityEngine.Purchasing.Product product;
            await Task.Run(() =>
            {
                product = IAPModule.Instance.GetProduct(productId);

                if (product != null)
                {
                    string str = product.metadata.localizedTitle;
                    str = str.Replace(IAPModule.Instance.removeAppCaption, "");
                    str = str.Trim();
                    localizedTitle = str;
                    localizedDescription = product.metadata.localizedDescription;
                    localizedPrice = product.metadata.localizedPriceString;
                    if (product.hasReceipt)
                    {
                        if (product.definition.type == ProductType.NonConsumable)
                        {
                            localizedPrice = MES_PURCHASED;
                            localizedInter = false;
                        }
                    }
                }
                return;
            });

            WaitForUpdateTextEnd();
        }

        void WaitForUpdateTextEnd()
        {
            if (titleText != null) titleText.text = localizedTitle;
            if (descriptionText != null) descriptionText.text = localizedDescription;
            if (priceText != null) priceText.text = localizedPrice;
            button.interactable = localizedInter;
        }


        internal void UpdateText()
        {
            //return;
            if (button == null) button = GetComponent<Button>();
            if (IAPModule.Instance == null) return;
            if (!IAPModule.initializationComplete) return;

            //StartCoroutine(WaitUpdateText());
            WaitForUpdateText();

            /*
            var product = IAPModule.Instance.GetProduct(productId);  // CodelessIAPStoreListener
            if (product != null)
            {
                //product.hasReceipt - куплен purchased
                if (titleText != null)
                {
                    string str = product.metadata.localizedTitle;
                    str = str.Replace(IAPModule.Instance.removeAppCaption, "");
                    str = str.Trim();
                    titleText.text = str;
                }

                if (descriptionText != null)
                {
                    descriptionText.text = product.metadata.localizedDescription;
                }

                if (priceText != null)
                {
                    priceText.text = product.metadata.localizedPriceString;
                }

                if (product.hasReceipt)
                {
                    if (product.definition.type == ProductType.NonConsumable)
                    {
                        priceText.text = MES_PURCHASED;
                        button.interactable = false;
                    }
                }
            }
            */
        }
    }
}
#endif