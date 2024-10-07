using Firebase.Database;

namespace Server
{
    class Connection_Server
    {
        private static string firebaseURL = "https://nt106-cce90-default-rtdb.firebaseio.com/";
        public static FirebaseClient GetFirebaseClient()
        {
            return new FirebaseClient(firebaseURL);
        }
    }
}
