using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace Spotter_Service.Objects
{
    public class User
    {
        public string authToken;

        private SpotifyClient client;

        public SpotifyClient GetSpotifyClient()
        {
            if (client == null)
            {
                client = new SpotifyClient(authToken);
            }
            return client;
        }

        public User(string auth)
        {
            authToken = auth;
        }
    }
}
