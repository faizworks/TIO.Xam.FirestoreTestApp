﻿using System;
using Firebase.Auth;
using Firebase.Firestore;

namespace fireStoreTest.Droid
{
    public static class FirestoreService
    {
        private static Firebase.FirebaseApp app;
        public static FirebaseFirestore Instance
        {
            get
            {
                return FirebaseFirestore.GetInstance(app);
            }
        }

        public static FirebaseAuth AuthInstance
        {
            get
            {
                return FirebaseAuth.GetInstance(app);
            }
        }



        public static string AppName { get; } = "firestoreTest1";

        public static void Init(Android.Content.Context context)
        {
            var baseOptions = Firebase.FirebaseOptions.FromResource(context);
            //// This HACK will be not needed, fixed in https://github.com/xamarin/GooglePlayServicesComponents/commit/723ebdc00867a4c70c51ad2d0dcbd36474ce8ff1
            var options = new Firebase.FirebaseOptions.Builder(baseOptions).SetProjectId(baseOptions.StorageBucket.Split('.')[0]).Build();
            app = Firebase.FirebaseApp.InitializeApp(context, options, AppName);
            //app = Firebase.FirebaseApp.InitializeApp(context, options, AppName);
        }
    }

}
