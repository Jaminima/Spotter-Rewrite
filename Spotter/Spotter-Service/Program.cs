using System;
using System.Threading;

namespace Spotter_Service
{
    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            Objects.UserState us = new Objects.UserState("BQBKiUMCFWW5dtBsVkyHH9CqVBFjGXMXQHcJczd6-9f8PowLZPoWv9eIVb-5-7i7mKsWLNul4Pn5058IKzo_7hz5W01Eqbq1F5J4Tj3buX5nwYUHM6fdzJDfejJQtdoHBo1zEjJmR4_vgTdU_jjEPt3vdh4u2NwLO66xbAA8vTEgh-pnuX0vVZ9V2PkjWnW6xSN1iCAYnSm5a8GGneuURHJrL5xsRjK8cypOWXSRe9nV6v-hDvOI-14CrSy_oxa_K_E");

            StateChecker.OnTrackSkip = (userState, currentPlaying) => { Console.WriteLine($"Skipped {userState.playingTrack.Name}"); };

            StateChecker.OnNowPlayingLikedSongs = (userState, currentPlaying) => { Console.WriteLine($"Playing From Liked Songs"); };
            StateChecker.OnListenContextChange = (userState, currentPlaying) => { Console.WriteLine($"Playing From A {currentPlaying.Context.Type}"); };

            StateChecker.watchedUsers.Add(us);

            while (true)
            {
                StateChecker.CheckState();
                Thread.Sleep(5000);
            }
        }

        #endregion Methods
    }
}