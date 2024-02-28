using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump;
using GoogleMobileAds.Ump.Api;

public static class UMP
{
    // Start is called before the first frame update
    public static void Init()
    {
        // Create a ConsentRequestParameters object.
        ConsentRequestParameters request = new ConsentRequestParameters();
        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);

    }

    private static void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            // Handle the error.
            Debug.LogError(consentError);
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        if (ConsentInformation.IsConsentFormAvailable())
        {
            ConsentForm.Load(OnLoadConsentForm);
        }


    }

    private static void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        ConsentForm _consentForm;

        if (error != null)
        {
            Debug.LogError(error);
            return;
        }
        _consentForm = consentForm;
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
    }

    private static void OnShowForm(FormError error)
    {
        if (error != null)
        {
            Debug.LogError(error);
            return;
        }
    }

}
