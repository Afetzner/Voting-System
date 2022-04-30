using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    [ApiController]
    public class BallotIssueController : ControllerBase
    {        
        [HttpGet]
        [Route("api/polls")]
        public List<BallotIssue> GetBallotIssues()
        {
            List<BallotIssue> list = new List<BallotIssue>();
            BallotIssue issue1 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("A12345678")
                .WithStartDate(new DateTime(2022, 3, 12))
                .WithEndDate(new DateTime(2022, 8, 5))
                .WithTitle("Test v. Test")
                .WithDescription("test")
                .WithOptions("option1", "option2")
                .Build();
            list.Add(issue1);
            BallotIssue issue2 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("B12345678")
                .WithStartDate(new DateTime(2022, 3, 12))
                .WithEndDate(new DateTime(2022, 7, 5))
                .WithTitle("Test v. Test")
                .WithDescription("test")
                .WithOptions("option1", "option2")
                .Build();
            list.Add(issue2);
            BallotIssue issue3 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("C12345678")
                .WithStartDate(new DateTime(2022, 3, 12))
                .WithEndDate(new DateTime(2022, 7, 5))
                .WithTitle("Test v. Test")
                .WithDescription("test")
                .WithOptions("option1", "option2")
                .Build();
            list.Add(issue3);
            BallotIssue issue4 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("D12345678")
                .WithStartDate(new DateTime(2022, 3, 12))
                .WithEndDate(new DateTime(2022, 8, 5))
                .WithTitle("Test v. Test")
                .WithDescription("test")
                .WithOptions("option1", "option2")
                .Build();
            list.Add(issue4);
            BallotIssue issue5 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("E12345678")
                .WithStartDate(new DateTime(2022, 3, 12))
                .WithEndDate(new DateTime(2022, 8, 5))
                .WithTitle("Test v. Test")
                .WithDescription("test")
                .WithOptions("option1", "option2")
                .Build();
            list.Add(issue5);
            return list;
        }
    }
}