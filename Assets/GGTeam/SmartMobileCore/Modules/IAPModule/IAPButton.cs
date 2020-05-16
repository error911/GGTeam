#if UNITY_PURCHASING

using System.Collections;
using System.Collections.Generic;
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

        // The type of this button, can be either a purchase or a restore button
        [Tooltip("Тип этой кнопки может быть кнопкой покупки или восстановления.")]
        public ButtonType buttonType = ButtonType.Purchase;

        //Consume the product immediately after a successful purchase
        [Tooltip("Израсходовать продукт сразу после успешной покупки")]
        public bool consumePurchase = true;

        // Event fired after a successful purchase of this product
        [Tooltip("Событие, срабатываемое после успешной покупки данного товара")]
        public OnPurchaseCompletedEvent onPurchaseComplete;

        // Event fired after a failed purchase of this product
        [Tooltip("Событие, срабатываемое после неудачной покупки этого продукта")]
        public OnPurchaseFailedEvent onPurchaseFailed;

        // [Optional] Displays the localized title from the app store
        [Tooltip("[Необязательно] Отображает локализованный заголовок из магазина приложений.")]
        public Text titleText;

        // [Optional] Displays the localized price from the app store
        [Tooltip("[Необязательно] Отображение локализованной цены из магазина приложений.")]
        public Text priceText;

        // [Optional] Displays the localized description from the app store
        [Tooltip("[Необязательно] Отображает локализованное описание из магазина приложений.")]
        public Text descriptionText;

        void Start()
        {
            Button button = GetComponent<Button>();

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
                }

                if (!CodelessIAPStoreListener.Instance.HasProductInCatalog(productId))
                {
                    // The product catalog has no product with the ID
                    Debug.LogWarning("В каталоге товаров нет товара с ID \"" + productId + "\"");
                }
            }
            else if (buttonType == ButtonType.Restore)
            {
                if (button)
                {
                    button.onClick.AddListener(Restore);
                }
            }
        }

        void OnEnable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                IAPModule.Instance.AddButton(this);
                if (IAPModule.initializationComplete)
                {
                    UpdateText();
                }
            }
        }

        void OnDisable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                IAPModule.Instance.RemoveButton(this);
            }
        }

        void PurchaseProduct()
        {
            if (buttonType == ButtonType.Purchase)
            {
                Debug.Log("IAPButton.PurchaseProduct() with product ID: " + productId);

                IAPModule.Instance.InitiatePurchase(productId);
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
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<IMicrosoftExtensions>()
                        .RestoreTransactions();
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                         Application.platform == RuntimePlatform.OSXPlayer ||
                         Application.platform == RuntimePlatform.tvOS)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<IAppleExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                         StandardPurchasingModule.Instance().appStore == AppStore.SamsungApps)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<ISamsungAppsExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                         StandardPurchasingModule.Instance().appStore == AppStore.CloudMoolah)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<IMoolahExtension>()
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
                    Debug.LogWarning(Application.platform.ToString() +
                                     " is not a supported platform for the Codeless IAP restore button");
                }
            }
        }

        void OnTransactionsRestored(bool success)
        {
            Debug.Log("Transactions restored: " + success);
        }

        /**
         *  Invoked to process a purchase of the product associated with this button
         *  Вызывается для обработки покупки продукта, связанного с этой кнопкой
         */
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            Debug.Log(string.Format("IAPButton.ProcessPurchase(PurchaseEventArgs {0} - {1})", e,
                e.purchasedProduct.definition.id));

            onPurchaseComplete.Invoke(e.purchasedProduct);

            return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
        }

        /**
         *  Invoked on a failed purchase of the product associated with this button
         *  Вызывается при неудачной покупке продукта, связанного с этой кнопкой
         */
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            Debug.Log(string.Format("IAPButton.OnPurchaseFailed(Product {0}, PurchaseFailureReason {1})", product,
                reason));

            onPurchaseFailed.Invoke(product, reason);
        }

        internal void UpdateText()
        {
            var product = CodelessIAPStoreListener.Instance.GetProduct(productId);
            if (product != null)
            {
                if (titleText != null)
                {
                    titleText.text = product.metadata.localizedTitle;
                }

                if (descriptionText != null)
                {
                    descriptionText.text = product.metadata.localizedDescription;
                }

                if (priceText != null)
                {
                    priceText.text = product.metadata.localizedPriceString;
                }
            }
        }
    }
}
#endif