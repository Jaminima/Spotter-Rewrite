using SpotifyAPI.Web;
using System;

namespace Spotter_Service.Objects
{
    public class UserState
    {
        #region Fields

        public bool IsFirstPass = true;

        public DateTime lastUpdate;
        public CurrentlyPlayingContext playingContext;
        public User user;

        #endregion Fields

        #region Constructors

        public UserState(string authToken)
        {
            user = new User(authToken);

            UpdatePlaying();
        }

        #endregion Constructors

        #region Properties

        public FullTrack playingTrack
        {
            get { return (FullTrack)playingContext.Item; }
        }

        #endregion Properties

        #region Methods

        public async void UpdatePlaying()
        {
            playingContext = await user.GetSpotifyClient().Player.GetCurrentPlayback();
            lastUpdate = DateTime.Now;
        }

        public async void UpdatePlaying(CurrentlyPlayingContext playContext)
        {
            playingContext = playContext;
            lastUpdate = DateTime.Now;
        }

        #endregion Methods
    }
}