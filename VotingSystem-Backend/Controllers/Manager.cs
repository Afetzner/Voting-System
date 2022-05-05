using VotingSystem.Model;

namespace VotingSystem.Controller
{
	public class Manager
	{
		// Dic<userResultViewers>
		private static Dictionary<string, UserResultsViewer> userResultViewers = new();
		//single static sharedREsultViewer -> functions (getPartition, getResult, getIssue) -> sharedResultViewer(universal) and  (getBallot) -> 

		public static Manager manager = new Manager();

		private static SharedResultViewer sharedResultViewer = new();

		public static void SpawnViewer(string voterSerial)
		{
			UserResultsViewer userViewer = new(voterSerial);
			userResultViewers.Add(voterSerial, userViewer);
		}

		public Dictionary<string, List<Voter>> GetVoterParticipation()
		{
			return sharedResultViewer.GetVoterParticipation();
		}

		public Dictionary<string, Dictionary<int, int>> GetResults()
		{
			return sharedResultViewer.GetResults();
		}

		public List<BallotIssue> GetBallotIssues()
		{
			return sharedResultViewer.GetBallotIssues();
		}

		public Dictionary<string, Ballot?> GetBallots(string voterSerial)
		{
			UserResultsViewer viewer = userResultViewers[voterSerial];
			return viewer.GetBallots(GetBallotIssues());
		}
	}
}

