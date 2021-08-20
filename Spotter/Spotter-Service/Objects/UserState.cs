using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace Spotter_Service.Objects
{
    public class UserState
    {
        public User user;

        public CurrentlyPlayingContext playingContext;

        public FullTrack playingTrack
        {
            get { return (FullTrack)playingContext.Item; }
        }

        public UserState(string authToken)
        {
            user = new User(authToken);

            var p = user.GetSpotifyClient().Player.GetCurrentPlayback();
            p.Wait();
            playingContext = p.Result;
        }
    }
}
