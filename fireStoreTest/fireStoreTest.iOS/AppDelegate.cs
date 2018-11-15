using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Foundation;
using UIKit;

namespace fireStoreTest.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            Firebase.Core.App.Configure();

            // This token was generated from TIO.Web.Firestore > HomeController > CreateToken 
            var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiIwNTk0MzdlYi0wNTBmLTQyMTctOGRkOC1lNjU5MzQ3ZGI3ZjIiLCJpc3MiOiJhaW5maXJlc3RvcmVwcm9qZWN0QGFwcHNwb3QuZ3NlcnZpY2VhY2NvdW50LmNvbSIsInN1YiI6ImFpbmZpcmVzdG9yZXByb2plY3RAYXBwc3BvdC5nc2VydmljZWFjY291bnQuY29tIiwiYXVkIjoiaHR0cHM6Ly9pZGVudGl0eXRvb2xraXQuZ29vZ2xlYXBpcy5jb20vZ29vZ2xlLmlkZW50aXR5LmlkZW50aXR5dG9vbGtpdC52MS5JZGVudGl0eVRvb2xraXQiLCJleHAiOjE1NDIyNjkwMjcsImlhdCI6MTU0MjI2NTQyN30.fhJhjPXJllaTcUbSE5-K7YltrqlIPvreJeS8GN3hv6O6cWaBdp7uU_ld6IE08P4RnYtRaEOjtO6DGkVlEcnln_qJTp7ZTS2Oor8hG5ebWZoGWI3GD7AR2e6kTA5sog2lDFaIbzXUy7aAGbYx4YgAk3p9eLDOp8J8h8uBorkNgsbAEwXx9hF-p355NA83jLxpg65p4Rw1wvuU8B_2oSefc6WffDkmm-stHXZSE5PT4eNUSqKx37tLTCh_lalUcox6B-KpaPz8whOWo_h_bFOztbBoCch71Fy0l_fhqI-YbvZwYn_dTyH5CZn9dlJH2excgXbIHMLUTCClV4fMUuzHZA";

            // Check if user is already logged in
            if (Auth.DefaultInstance.CurrentUser != null)
            {
                // Force refresh firebase custom token
                Auth.DefaultInstance.CurrentUser.GetIdToken(true, (rToken, error) =>
                {
                    if (rToken != null)
                    {
                        // Initialize document fetch and listener
                        InitDocumentQuery();
                    }
                });
            }
            else
            {
                Auth.DefaultInstance.SignIn(token, (user, error) =>
                {
                    if (user != null)
                    {
                        // Initialize document fetch and listener
                        InitDocumentQuery();
                    }
                    else
                    {

                    }
                });
            }

            // To sign in anonymously
            //Auth.DefaultInstance.SignInAnonymously((authResult, error) =>
            //{

            //});

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Initialize document fetching and listner on a document query
        /// </summary>
        private static void InitDocumentQuery()
        {
            //FirestoreService.Init();
            var instance = FirestoreService.Instance;
            var collection = instance.GetCollection("tests");
            collection.GetDocuments((snapshot, e) =>
            {
                if (snapshot != null)
                {
                    if (snapshot.Documents != null)
                    {
                        foreach (var document in snapshot.Documents)
                        {
                            var dic = document.Data;
                        }
                    }
                }

            });

            collection.AddSnapshotListener((snapshot, e) =>
            {
                if (snapshot != null && !snapshot.Metadata.IsFromCache && snapshot.DocumentChanges != null && snapshot.DocumentChanges.Count() > 0)
                {
                    var changes = snapshot.DocumentChanges.ToList();
                    foreach (var change in changes)
                    {
                        var changedDocumentData = change.Document.Data;
                    }
                }
            });
        }
    }
}
