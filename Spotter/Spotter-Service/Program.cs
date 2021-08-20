using System.Threading;

namespace Spotter_Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Objects.UserState us = new Objects.UserState("BQDPBsr5ZahdFcaogV9F7wui-YmRQJbeWIYK1xDl4SmNEy1YSZFaN7bY6Pr2_I0qqM1y2JJbYd3irpd7REOSYnuc2tPeQsNSt07f-4vH3Z92tY-ARYBkZUnKghLcXmQl-05aJ_QOMZSaFy8cn0vucJB057NmY80tQM5AZuDmP-gl17qx3MOlVBoJegVySZYMPCn9g95BFjsHaRayCZ25fGZP729cOEbuc6l5DSU17UJs_D3KVa5iObHuXvuyjEH_oXA");

            StateChecker.watchedUsers.Add(us);

            while (true)
            {
                StateChecker.CheckState();
                Thread.Sleep(5000);
            }
        }
    }
}
