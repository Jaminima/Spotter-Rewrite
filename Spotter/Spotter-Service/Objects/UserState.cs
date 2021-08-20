using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace Spotter_Service.Objects
{
    public class UserState
    {
        public bool IsFirstPass = true;

        public User user;

        public CurrentlyPlayingContext playingContext;

        public DateTime lastUpdate;

        public async void UpdatePlaying()
        {
            playingContext = await user.GetSpotifyClient().Player.GetCurrentPlayback();
            lastUpdate = DateTime.Now;
        }

        public FullTrack playingTrack
        {
            get { return (FullTrack)playingContext.Item; }
        }

        public UserState(string authToken)
        {
            user = new User(authToken);

            UpdatePlaying();
        }
    }
}
