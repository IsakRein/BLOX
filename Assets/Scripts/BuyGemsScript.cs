using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VoxelBusters.NativePlugins;

public class BuyGemsScript : MonoBehaviour {

    public TextMeshProUGUI gemText;

    public List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        RequestBillingProducts();
    }

    public void BuyGems(int productID) 
    {
        if (productID < 4) {
            int gems = 0;

            if (productID == 0) 
            {
                gems = 600;    
            }

            else if (productID == 1) {
                gems = 1500;
            }

            else if (productID == 2)
            {
                gems = 4500;
            }
           
            else if (productID == 3)
            {
                gems = 11550;
            }

            Manager.gemCount += gems;
            Manager.SaveGemCount();

            gemText.SetText(Manager.gemCount.ToString());
        }
    }

    public void RequestBillingProducts()
    {
        NPBinding.Billing.RequestForBillingProducts(NPSettings.Billing.Products);

        // At this point you can display an activity indicator to inform user that task is in progress
    }

    private void OnEnable()
    {
        // Register for callbacks
        Billing.DidFinishRequestForBillingProductsEvent += OnDidFinishProductsRequest;
    }
        
    private void OnDisable()
    {
        // Deregister for callbacks
        Billing.DidFinishRequestForBillingProductsEvent -= OnDidFinishProductsRequest;
    }

    private void OnDidFinishProductsRequest(BillingProduct[] _regProductsList, string _error)
    {
        // Hide activity indicator

        // Handle response
        if (_error != null)
        {
            // Something went wrong
        }
        else
        {
            // Inject code to display received products

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = _regProductsList[i].LocalizedPrice + "";
            } 
        }
    }

    public void BuyItem(BillingProduct _product)
    {
        if (NPBinding.Billing.IsProductPurchased(_product.ProductIdentifier))
        {   
            // Show alert message that item is already purchased

            return;
        }

        // Call method to make purchase
        NPBinding.Billing.BuyProduct(_product);

        // At this point you can display an activity indicator to inform user that task is in progress
    }

    private void OnDidFinishTransaction(BillingTransaction _transaction)
    {
        if (_transaction != null)
        {

            if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
            {
                if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
                {
                    // Your code to handle purchased products

                }   
            }
        }
    }
}