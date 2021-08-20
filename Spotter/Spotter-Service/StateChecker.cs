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

        public static Action<Objects.UserState, CurrentlyPlayingContext> OnResume, OnPause, OnTrackChange, OnTrackSkip;
        public static Action<Objects.UserState, CurrentlyPlayingContext, int, int> OnTrackRewind, OnTrackFastForward;

        private static async void CheckUser(Objects.UserState userState)
        {
            CurrentlyPlayingContext currentlyPlaying = await userState.user.GetSpotifyClient().Player.GetCurrentPlayback();

            if (currentlyPlaying.IsPlaying != userState.playingContext.IsPlaying)
            {
                if (currentlyPlaying.IsPlaying) { if (OnResume != null) OnResume(userState, currentlyPlaying); }
                else { if (OnPause != null) OnPause(userState, currentlyPlaying); }
            }

            if (currentlyPlaying.Item == null)
            {
                Console.WriteLine("Unkown Item");
                return;
            }

            switch (currentlyPlaying.Item.Type)
            {
                case ItemType.Track:
                    FullTrack playingTrack = (FullTrack)currentlyPlaying.Item;

                    if (playingTrack.Id != userState.playingTrack.Id)
                    {
                        if (OnTrackChange != null) OnTrackChange(userState, currentlyPlaying);

                        if (userState.playingContext.ProgressMs < userState.playingTrack.DurationMs * 0.9f)
                        {
                            if (OnTrackSkip != null) OnTrackSkip(userState, currentlyPlaying);
                        }
                    }
                    else
                    {
                        if (userState.playingContext.ProgressMs > currentlyPlaying.ProgressMs)
                        {
                            if (OnTrackRewind != null) OnTrackRewind(userState, currentlyPlaying, userState.playingContext.ProgressMs, currentlyPlaying.ProgressMs);
                        }
                        else if (currentlyPlaying.ProgressMs - userState.playingContext.ProgressMs > (DateTime.Now - userState.lastUpdate).TotalMilliseconds * 1.1f)
                        {
                            if (OnTrackFastForward != null) OnTrackFastForward(userState, currentlyPlaying, userState.playingContext.ProgressMs, currentlyPlaying.ProgressMs);
                        }
                    }

                    break;

                default:
                    throw new Exception("Unkown Item Type");
            }

            userState.UpdatePlaying(currentlyPlaying);
        }

        public static void CheckState()
        {
            foreach (Objects.UserState userState in watchedUsers)
            {
                if (userState.IsFirstPass)
                {
                    userState.IsFirstPass = false;
                    userState.UpdatePlaying();
                }
                else
                {
                    CheckUser(userState);
                }

            }
        }
    }
}
