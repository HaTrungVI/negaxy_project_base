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

    /// <summary>
    /// function to load consent form
    /// </summary>
    /// <param name="consentForm"> consent form </param>
    /// <param name="error"> form error </param>
    private static void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        //consent form
        ConsentForm _consentForm;

        //if there is an error while getting a form
        if (error != null)
        {
            //log it out, use firebase
            Debug.LogError(error);
            //stop execute
            return;
        }

        //success

        //get consent form
        _consentForm = consentForm;

        //make sure if it is required to get consent
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            //show the form with error check function
            _consentForm.Show(OnShowForm);
        }
    }


    /// <summary>
    /// function to show form
    /// </summary>
    /// <param name="error"> error from getting form </param>
    private static void OnShowForm(FormError error)
    {
        //if  there is tourble getting form
        if (error != null)
        {
            //log our error use firebase
            Debug.LogError(error);
            //stop execute
            return;
        }
    }

}
