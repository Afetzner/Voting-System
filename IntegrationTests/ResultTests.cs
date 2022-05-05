using System;
using System.Collections.Generic;
using VotingSystem.Controller;
using VotingSystem.Accessor;
using VotingSystem.Model;
using IntegrationTests.Interactive;


namespace IntegrationTests
{
    internal static class ResultTests
    {
        public static Func<bool> ResultTestMenu()
        {
            Console.WriteLine("Select a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all results tests\n" +
                " (3) Get issues\n" + 
                " (4) Get ballots (voter 1)\n" +
                " (5) Get ballots (voter 2)\n" +
                " (6) Get issue results\n" +
                " (7) Get voter participation\n" + 
                " (8) Use shared cache\n" +
                " (*) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => TestDataLoader.UnloadTestData,
                    '1' => TestDataLoader.LoadIntTestData,
                    '2' => RunAllResultTests,
                    '3' => TestGetIssues,
                    '4' => TestGetBallotsVoter1,
                    '5' => TestGetBallotsVoter2,
                    '6' => TestGetIssueResults,
                    '7' => TestGetVoterParticipation,
                    '8' => TestSharedCache,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllResultTests = new()
        {
            TestGetIssues,
            TestGetBallotsVoter1,
            TestGetBallotsVoter2,
            TestGetIssueResults,
            TestGetVoterParticipation,
            TestSharedCache
        };

        public static bool RunAllResultTests()
        {
            TestDataLoader.UnloadTestData();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllResultTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail");
            Console.WriteLine("\n");
            return fail == 0;
        }

        public static bool TestGetIssues()
        {
            Console.WriteLine("    Testing results get issues");
            TestDataLoader.LoadIntTestDataForResultsViewer();
            SharedResultCache resultsCache = new();
            var issues = resultsCache.GetBallotIssues();
            var issue1 = new TestData().issue.Build();
            var issue2 = new TestData().issue2.Build();

            if (!issues.Exists(x => x.Title == issue1.Title
                                && x.SerialNumber == issue1.SerialNumber
                                && x.StartDate == issue1.StartDate
                                && x.EndDate == issue1.EndDate
                                && x.IsEnded == issue1.IsEnded) ||
                
                !issues.Exists(x => x.Title == issue2.Title
                                && x.SerialNumber == issue2.SerialNumber
                                && x.StartDate == issue2.StartDate
                                && x.EndDate == issue2.EndDate
                                && x.IsEnded == issue2.IsEnded))
            {
                Console.WriteLine("(F) Results get issues failed: no match in gotten issues");
                Console.WriteLine(@$"    issues to match: s:'{issue1.SerialNumber}', t:'{issue1.Title}'");
                Console.WriteLine(@$"                     s:'{issue2.SerialNumber}', t:'{issue2.Title}'");
                Console.WriteLine("    issues from db:");

                foreach (var gotten in issues)
                {
                    Console.WriteLine(@$"                     s:'{gotten.SerialNumber}', t:'{gotten.Title}'");
                }
                return false;
            }
            Console.WriteLine("(S) Results get issues success");
            return true;
        }
        
        /// <summary>
        /// In test data, voter 1 submitted ballots on issues 1 and 2, expect non-null, matching result for both
        /// </summary>
        public static bool TestGetBallotsVoter1()
        {
            Console.WriteLine("    Testing get ballots (voter 1)");
            //Arrange
            TestDataLoader.LoadIntTestDataForResultsViewer();
            var voter = new TestData().voter.Build();
            var issue1 = new TestData().issue.Build();
            var issue2 = new TestData().issue2.Build();
            var ballot1 = new TestData().ballot.Build(); //Voter 1's ballot on issue 1
            var ballot2 = new TestData().ballot2.Build(); //Voter 1's ballot on issue 2

            //Act
            var issues = BallotIssue.Accessor.GetBallotIssues();
            var ballots = new UserResultsCache(voter.SerialNumber).GetBallots(ref issues);
            var ballotFromDb1 = ballots[issue1.SerialNumber];
            var ballotFromDb2 = ballots[issue2.SerialNumber];

            //Assert
            if (ballotFromDb1 == null || ballotFromDb2 == null)
            {
                Console.WriteLine("(F) Get ballots voter 1 failure: Ballot from db null");
                return false;
            }
            
            //Assert match to ballot 1 (on issue 1)
            if (ballotFromDb1.SerialNumber != ballot1.SerialNumber ||
                ballotFromDb1.IssueSerial != issue1.SerialNumber ||
                ballotFromDb1.VoterSerial != voter.SerialNumber ||
                ballotFromDb1.Choice != ballot1.Choice)
            {
                Console.WriteLine("(F) Results get ballots (voter 1) failed: issue 1 ballot does not match");
                Console.WriteLine(@$"    expected: s:'{ballotFromDb1.SerialNumber}', i:'{ballotFromDb1.IssueSerial}', v:'{ballotFromDb1.VoterSerial}', c: '{ballotFromDb1.Choice}'");
                Console.WriteLine(@$"     actual : s:'{ballot1.SerialNumber}', i:'{ballot1.IssueSerial}', v:'{ballot1.VoterSerial}', c: '{ballot1.Choice}'");
                return false;
            }

            //Assert match to ballot 2 (on issue 2)
            if (ballotFromDb2.SerialNumber != ballot2.SerialNumber ||
                ballotFromDb2.IssueSerial != issue2.SerialNumber ||
                ballotFromDb2.VoterSerial != voter.SerialNumber ||
                ballotFromDb2.Choice != ballot2.Choice)
            {
                Console.WriteLine("(F) Results get ballots (voter 1) failed: issue 2 ballot does not match");
                Console.WriteLine(@$"    expected: s:'{ballotFromDb2.SerialNumber}', i:'{ballotFromDb2.IssueSerial}', v:'{ballotFromDb2.VoterSerial}', c: '{ballotFromDb2.Choice}'");
                Console.WriteLine(@$"     actual : s:'{ballot2.SerialNumber}', i:'{ballot2.IssueSerial}', v:'{ballot2.VoterSerial}', c: '{ballot2.Choice}'");
                return false;
            }

            Console.WriteLine("(S) Results get ballots (voter 1) success");
            return true;
        }

        /// <summary>
        /// In test data, voter 2 submitted ballots on only issues 1, expect matching for first, null for second
        /// </summary>
        public static bool TestGetBallotsVoter2()
        {
            Console.WriteLine("    Testing get ballots (voter 2)");
            //Arrange
            TestDataLoader.LoadIntTestDataForResultsViewer();
            var voter = new TestData().voter2.Build();
            var issue1 = new TestData().issue.Build();
            var issue2 = new TestData().issue2.Build();
            var ballot = new TestData().ballot3.Build(); //Voter 2's ballot on issue 1

            //Act
            var issues = BallotIssue.Accessor.GetBallotIssues();
            var ballots = new UserResultsCache(voter.SerialNumber).GetBallots(ref issues);
            var ballotFromDb1 = ballots[issue1.SerialNumber];
            var ballotFromDb2 = ballots[issue2.SerialNumber];

            //Assert
            if (ballotFromDb1 == null)
            {
                Console.WriteLine("(F) Get ballots voter 2 failure: Ballot from db null");
                return false;
            }

            //Assert match to ballot (on issue 1)
            if (ballotFromDb1.SerialNumber != ballot.SerialNumber ||
                ballotFromDb1.IssueSerial != issue1.SerialNumber ||
                ballotFromDb1.VoterSerial != voter.SerialNumber ||
                ballotFromDb1.Choice != ballot.Choice)
            {
                Console.WriteLine("(F) Results get ballots (voter 2) failed: issue 1 ballot does not match");
                Console.WriteLine(@$"    expected: s:'{ballotFromDb1.SerialNumber}', i:'{ballotFromDb1.IssueSerial}', v:'{ballotFromDb1.VoterSerial}', c: '{ballotFromDb1.Choice}'");
                Console.WriteLine(@$"     actual : s:'{ballot.SerialNumber}', i:'{ballot.IssueSerial}', v:'{ballot.VoterSerial}', c: '{ballot.Choice}'");
                return false;
            }

            //Assert match to ballot 2 (on issue 2)
            if (ballotFromDb2 != null)
            {
                Console.WriteLine("(F) Results get ballots (voter 2) failed: issue 2 not null");
                Console.WriteLine(@$"    from db: s:'{ballotFromDb2.SerialNumber}', i:'{ballotFromDb2.IssueSerial}', v:'{ballotFromDb2.VoterSerial}', c: '{ballotFromDb2.Choice}'");
                return false;
            }

            Console.WriteLine("(S) Results get ballots (voter 2) success");
            return true;
        }
    
        /// <summary>
        /// In test data --- issue 1: opt 0: 2, opt 1: 0 --- issue 2: opt 0: 0, opt 1: 1
        /// </summary>
        public static bool TestGetIssueResults()
        {
            Console.WriteLine("    Testing get issue results");
            //Arrange
            TestDataLoader.LoadIntTestDataForResultsViewer();
            var issue1 = new TestData().issue.Build();
            var issue2 = new TestData().issue2.Build();

            //Act 
            var issues = BallotIssue.Accessor.GetBallotIssues();
            var results = new ResultAccessor().GetAllResults(issues);

            //Assert issue 1 results (2 for opt 1, 0 for opt 0)
            if (results[issue1.SerialNumber][0] != 2)
            {
                int count = results[issue1.SerialNumber][0];
                Console.WriteLine(@$"(F) Get issue results failure: issue 1, option 0 expected 2, actual {count}");
                return false;
            }
            if (results[issue1.SerialNumber][1] != 0)
            {
                int count = results[issue1.SerialNumber][1];
                Console.WriteLine(@$"(F) Get issue results failure: issue 1, option 1 expected 1, actual {count}");
                return false;
            }

            //Assert issue 2 results (0 for opt 0, 1 for opt 1)
            if (results[issue2.SerialNumber][0] != 0)
            {
                int count = results[issue2.SerialNumber][0];
                Console.WriteLine(@$"(F) Get issue results failure: issue 2, option 0 expected 0, actual {count}");
                return false;
            }
            if (results[issue2.SerialNumber][1] != 1)
            {
                int count = results[issue2.SerialNumber][1];
                Console.WriteLine(@$"(F) Get issue results failure: issue 2, option 1 expected 0, actual {count}");
                return false;
            }

            Console.WriteLine("(S) Get issue results success");
            return true;
        }

        /// <summary>
        /// In test data: expecct both voters partcipate in issue 1, netiher in issue 2
        /// </summary>
        public static bool TestGetVoterParticipation()
        {
            Console.WriteLine("    Testing get voter participation");
            //Arrange
            TestDataLoader.LoadIntTestDataForResultsViewer();
            var voter1 = new TestData().voter.Build();
            var voter2 = new TestData().voter2.Build();
            var issue1 = new TestData().issue.Build();
            var issue2 = new TestData().issue2.Build();
            var ballot = new TestData().ballot2.Build();
            //Remove voter 1's participation from issue 2
            Ballot.Accessor.RemoveBallot(ballot.SerialNumber);

            //Act
            var issues = BallotIssue.Accessor.GetBallotIssues();
            var participation = new SharedResultCache().GetVoterParticipation();
            var partIssue1 = participation[issue1.SerialNumber];
            var partIssue2 = participation[issue2.SerialNumber];

            //Assert
            //Assert issue 1 participation
            if (!partIssue1.Exists(x => x.SerialNumber == voter1.SerialNumber) ||
                !partIssue1.Exists(x => x.SerialNumber == voter2.SerialNumber))
            {
                Console.WriteLine("(F) Get voter participation failure: issue 1 voter missing");
                Console.WriteLine("    Actual:");
                foreach (var voter in partIssue1)
                {
                    Console.WriteLine(@$"    voter: {voter.SerialNumber}");
                }
                Console.WriteLine("    Expected:");
                Console.WriteLine(@$"    {voter1.SerialNumber}");
                Console.WriteLine(@$"    {voter2.SerialNumber}");
                return false;
            }

            //Assert issue 2 participation
            if (partIssue2.Count != 0)
            {
                Console.WriteLine("(F) Get voter participation failure: issue 2 not empty");
                Console.WriteLine("    Actual:");
                foreach (var voter in partIssue2)
                {
                    Console.WriteLine(@$"    voter: {voter.SerialNumber}");
                }
                Console.WriteLine("    Expected nothing");
                return false;
            }

            Console.WriteLine("(S) Get voter participation success");
            return true;
        }

        /// <summary>
        /// One big test to make sure the values are cached correctly
        /// </summary>
        public static bool TestSharedCache()
        {
            Console.WriteLine("    Testing use shared viewer");
            //Arrange
            TestDataLoader.LoadIntTestDataForResultsViewer();
            var voter1 = new TestData().voter.Build();
            var voter2 = new TestData().voter2.Build();
            var issue1 = new TestData().issue.Build();
            var issue2 = new TestData().issue2.Build();
            var ballot1 = new TestData().ballot.Build();
            var ballot2 = new TestData().ballot2.Build();
            var ballot3 = new TestData().ballot3.Build();

            //Act 
            CacheManager cache = new ();
            var issues = cache.GetBallotIssues();
            var results = cache.GetResults();
            var ballots = cache.GetBallots(voter1.SerialNumber);
            var participation = cache.GetVoterParticipation();

            //Assert
            //Assert get issues is correct
            if (!issues.Exists(x => x.SerialNumber == issue1.SerialNumber) ||
                !issues.Exists(x => x.SerialNumber == issue2.SerialNumber))
            {
                Console.WriteLine("(F) use shared viewer failed: issues not contained");
                return false;
            }

            //Assert results are correct
            if (results[issue1.SerialNumber][0] != 2 ||
                results[issue1.SerialNumber][1] != 0 ||
                results[issue1.SerialNumber][2] != 0 ||
                results[issue2.SerialNumber][0] != 0 ||
                results[issue2.SerialNumber][1] != 1 ||
                results[issue2.SerialNumber][2] != 0)
            {
                Console.WriteLine("(F) use shared viewer failed: results not correct");
                return false;
            }

            //Assert ballots are correct
            if (ballots[issue1.SerialNumber].SerialNumber != ballot1.SerialNumber ||
                ballots[issue2.SerialNumber].SerialNumber != ballot2.SerialNumber)
            {
                Console.WriteLine("(F) use shared viewer failed: voter 1's ballots are incorrect");
                return false;
            }

            //Assert voter participation is correct
            if (!participation[issue1.SerialNumber].Exists(x => x.SerialNumber == voter1.SerialNumber) ||
                !participation[issue1.SerialNumber].Exists(x => x.SerialNumber == voter2.SerialNumber))
            {
                Console.WriteLine("(F) use shared viewer failed: voters not contained in participation");
                return false;
            }

            Console.WriteLine("(S) Use shared viewer success");
            return true;
        }
    }
}
