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

        private static async void CheckUser(Objects.UserState userState)
        {
            CurrentlyPlayingContext currentlyPlaying = await userState.user.GetSpotifyClient().Player.GetCurrentPlayback();

            if (currentlyPlaying.IsPlaying != userState.playingContext.IsPlaying)
            {
                if (currentlyPlaying.IsPlaying) Console.WriteLine("Resumed Playback");
                else Console.WriteLine("Paused Playback");
            }

            switch (currentlyPlaying.Item.Type)
            {
                case ItemType.Track:
                    FullTrack playingTrack = (FullTrack)currentlyPlaying.Item;

                    if (playingTrack.Id != userState.playingTrack.Id)
                    {
                        Console.WriteLine($"Playing {playingTrack.Name}, Was Hearing {userState.playingTrack.Name}");

                        if (userState.playingContext.ProgressMs < userState.playingTrack.DurationMs * 0.9f)
                        {
                            Console.WriteLine($"{userState.playingTrack.Name} Was Skipped");
                        }
                    }
                    else
                    {
                        if (userState.playingContext.ProgressMs > currentlyPlaying.ProgressMs)
                        {
                            Console.WriteLine($"Rewound From {userState.playingContext.ProgressMs / 1000}s to {currentlyPlaying.ProgressMs / 1000}s");
                        }
                        else if (currentlyPlaying.ProgressMs - userState.playingContext.ProgressMs > (DateTime.Now - userState.lastUpdate).TotalMilliseconds * 1.1f)
                        {
                            Console.WriteLine($"Jumped From {userState.playingContext.ProgressMs / 1000}s to {currentlyPlaying.ProgressMs / 1000}s");
                        }
                    }

                    break;

                default:
                    throw new Exception("Unkown Item Type");
            }
        }

        public static void CheckState()
        {
            foreach (Objects.UserState userState in watchedUsers)
            {
                if (userState.IsFirstPass)
                {
                    userState.IsFirstPass = false;
                }
                else
                {
                    CheckUser(userState);
                }

                userState.UpdatePlaying();
            }
        }
    }
}
