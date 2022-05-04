namespace VotingSystem.Controller
{
    public class ResultsManager
    {
        public static SharedResultViewer ResultViewer = new SharedResultViewer();
        private static Dictionary<string, UserResultsViewer> userViewers = new();

        public static void SpawnViewer(string voterSerial)
        {
            UserResultsViewer userViewer = new(voterSerial);
            userViewers.Add(voterSerial, userViewer);
        }


    }
}
