using SpotifyAPI.Web;

namespace Spotter_Service.Objects
{
    public class User
    {
        #region Fields

        private SpotifyClient client;
        public string authToken;

        #endregion Fields

        #region Constructors

        public User(string auth)
        {
            authToken = auth;
        }

        #endregion Constructors

        #region Methods

        public SpotifyClient GetSpotifyClient()
        {
            if (client == null)
            {
                client = new SpotifyClient(authToken);
            }
            return client;
        }

        #endregion Methods
    }
}