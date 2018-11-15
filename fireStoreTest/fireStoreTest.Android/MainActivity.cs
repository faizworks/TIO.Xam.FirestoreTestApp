using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Auth;
using Android.Gms.Tasks;
using Java.Lang;
using Firebase.Firestore;
using System.Linq;
using Firebase;

namespace fireStoreTest.Droid
{
    [Activity(Label = "fireStoreTest", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnCompleteListener, IOnSuccessListener, IOnFailureListener, IEventListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            FirestoreService.Init(ApplicationContext);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // This token was generated from TIO.Web.Firestore > HomeController > CreateToken 
            var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI1NzFmODViZS1kOGI0LTQ5MTYtOTVlYy0zNjBmOTk5OGE5MDEiLCJpc3MiOiJhaW5maXJlc3RvcmVwcm9qZWN0QGFwcHNwb3QuZ3NlcnZpY2VhY2NvdW50LmNvbSIsInN1YiI6ImFpbmZpcmVzdG9yZXByb2plY3RAYXBwc3BvdC5nc2VydmljZWFjY291bnQuY29tIiwiYXVkIjoiaHR0cHM6Ly9pZGVudGl0eXRvb2xraXQuZ29vZ2xlYXBpcy5jb20vZ29vZ2xlLmlkZW50aXR5LmlkZW50aXR5dG9vbGtpdC52MS5JZGVudGl0eVRvb2xraXQiLCJleHAiOjE1NDIyODAxODQsImlhdCI6MTU0MjI3NjU4NH0.fcR9tmsaspdGk7W1W_UWtxuNX-HF7e9-bY85euFpIEAQu0gO1pjF4CDyfC14Pn7hvo0aUQLxQLXs5EnhmHYvdHPdiKhQ_Ma2jpC_rim3wdnkE0W5shJha59pA0lmGDS9tFt2yHCfu24RoBBrduR0VC2oNUsIYeiYqXkLlbE2SXe3U_MOUbafYameox5XilrdxUrPsDUGEjolTPrewA_trGOHRSRs5obyrvXyq8SSNnLE0aTY-dILJ-68ULtIGtNzX6LDjLAt1-b-9vd6gZNFvFmCCBlfK4X1rxkTBPb6oVWN9LIG6fTn_WUtVT1aglb9YVe2bRZAIXej56KW5RoniA";

            var currentUser = FirebaseAuth.Instance.CurrentUser;

            // Check if user is already logged in
            if (currentUser != null)
            {
                // Force refresh firebase token
                currentUser.GetIdToken(true).AddOnCompleteListener(this);
            }
            else
            {
                FirestoreService.AuthInstance.SignInWithCustomToken(token).AddOnCompleteListener(this).AddOnFailureListener(this);
            }

            //FirestoreService.AuthInstance.SignInAnonymously();
        }

        public void OnComplete(Task task)
        {
            //throw new NotImplementedException();
            if (task.IsSuccessful)
            {
                if (task.Result != null)
                {
                    if (task.Result.GetType() == typeof(QuerySnapshot))
                    {
                        var snapshot = (QuerySnapshot)task.Result;
                        if (snapshot != null)
                        {
                            if (snapshot.Documents != null)
                            {
                                foreach (var document in snapshot.Documents)
                                {
                                    System.Diagnostics.Debug.WriteLine("Fetched DocumentId: " + document.Id);
                                    var dic = document.Data;
                                }
                            }
                        }
                    }
                    else
                    {
                        GetDocuments();
                    }
                }
                var result = task.Result;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("signInWithCustomToken:failure" + task.Exception);
                Toast.MakeText(this, "Authentication failed.",
                               ToastLength.Long).Show();
            }
        }

        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            if (error != null)
            {
                Toast.MakeText(this, error.ToString(),
                               ToastLength.Long).Show();
            }
            else
            {
                if (value != null)
                {
                    if (value.GetType() == typeof(QuerySnapshot))
                    {
                        var snapshot = (QuerySnapshot)value;
                        if (snapshot != null && !snapshot.Metadata.IsFromCache && snapshot.DocumentChanges != null && snapshot.DocumentChanges.Count() > 0)
                        {
                            var changes = snapshot.DocumentChanges.ToList();
                            foreach (var change in changes)
                            {
                                System.Diagnostics.Debug.WriteLine("Changed DocumentId: " + change.Document.Id);
                                var changedDocumentData = change.Document.Data;
                            }
                        }
                    }
                }
            }
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            //throw new NotImplementedException();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize document fetching and listner on a document query
        /// </summary>
        private void GetDocuments()
        {
            var instance = FirestoreService.Instance;
            var collection = instance.Collection("tests");
            collection.Get().AddOnCompleteListener(this);
            collection.AddSnapshotListener(this);
        }
    }
}