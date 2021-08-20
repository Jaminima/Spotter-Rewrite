using SpotifyAPI.Web;
using System;
using System.Collections.Generic;

namespace Spotter_Service
{
    public static class StateChecker
    {
        #region Methods

        private static async void CheckUser(Objects.UserState lastUserState)
        {
            CurrentlyPlayingContext currentlyPlaying = await lastUserState.user.GetSpotifyClient().Player.GetCurrentPlayback();

            if (currentlyPlaying.IsPlaying != lastUserState.playingContext.IsPlaying)
            {
                if (currentlyPlaying.IsPlaying) { if (OnResume != null) OnResume(lastUserState, currentlyPlaying); }
                else { if (OnPause != null) OnPause(lastUserState, currentlyPlaying); }
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

                    if (playingTrack.Id != lastUserState.playingTrack.Id)
                    {
                        if (OnTrackChange != null) OnTrackChange(lastUserState, currentlyPlaying);

                        if (lastUserState.playingContext.ProgressMs < lastUserState.playingTrack.DurationMs * 0.9f)
                        {
                            if (OnTrackSkip != null) OnTrackSkip(lastUserState, currentlyPlaying);
                        }
                    }
                    else
                    {
                        if (lastUserState.playingContext.ProgressMs > currentlyPlaying.ProgressMs)
                        {
                            if (OnTrackRewind != null) OnTrackRewind(lastUserState, currentlyPlaying, lastUserState.playingContext.ProgressMs, currentlyPlaying.ProgressMs);
                        }
                        else if (currentlyPlaying.ProgressMs - lastUserState.playingContext.ProgressMs > (DateTime.Now - lastUserState.lastUpdate).TotalMilliseconds * 1.1f)
                        {
                            if (OnTrackFastForward != null) OnTrackFastForward(lastUserState, currentlyPlaying, lastUserState.playingContext.ProgressMs, currentlyPlaying.ProgressMs);
                        }
                    }

                    if (currentlyPlaying.Context == null && lastUserState.playingContext.Context != null)
                    {
                        if (OnNowPlayingLikedSongs != null) OnNowPlayingLikedSongs(lastUserState, currentlyPlaying);
                    }

                    if (currentlyPlaying.Context != null)
                    {
                        if (lastUserState.playingContext.Context == null || currentlyPlaying.Context.Uri != lastUserState.playingContext.Context.Uri)
                        {
                            if (OnListenContextChange != null) OnListenContextChange(lastUserState, currentlyPlaying);
                        }
                    }

                    break;

                default:
                    throw new Exception("Unkown Item Type");
            }

            lastUserState.UpdatePlaying(currentlyPlaying);
        }

        #endregion Methods

        #region Fields

        public static Action<Objects.UserState, CurrentlyPlayingContext> OnResume, OnPause, OnTrackChange, OnTrackSkip, OnNowPlayingLikedSongs, OnListenContextChange;
        public static Action<Objects.UserState, CurrentlyPlayingContext, int, int> OnTrackRewind, OnTrackFastForward;
        public static List<Objects.UserState> watchedUsers = new List<Objects.UserState>();

        #endregion Fields

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