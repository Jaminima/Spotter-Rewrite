using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace Spotter_Service
{
    public static class StateChecker
    {
        public static List<Objects.UserState> watchedUsers = new List<Objects.UserState>();

        public static async void CheckState()
        {
            foreach (Objects.UserState userState in watchedUsers)
            {
                CurrentlyPlayingContext currentlyPlaying = await userState.user.GetSpotifyClient().Player.GetCurrentPlayback();

                switch (currentlyPlaying.Item.Type)
                {
                    case ItemType.Track:
                        FullTrack playingTrack = (FullTrack)currentlyPlaying.Item;

                        if (playingTrack.Id != userState.playingTrack.Id)
                        {
                            Console.WriteLine($"Playing {playingTrack.Name}, Was Hearing {userState.playingTrack.Name}");
                            userState.playingContext = currentlyPlaying;
                        }

                        break;

                    default:
                        throw new Exception("Unkown Item Type");
                }
            }
        }
    }
}
