use afetzner;

-- get all admins with username & password
select admin_id, username, password 
from user join admin on user.user_id = admin.user_id;

-- get all voters with username & password
select serial_number, username, password, first_name, last_name 
from user;

-- See all issues with options
select serial_number, issue.title, start_date, end_date, description, issue.title, option_number, issue_option.title 
from issue join issue_option on issue.issue_id = issue_option.issue_id;

-- See all ballots
select ballot_serial_number, voter_serial_number, user.first_name, user.last_name, issue_serial_number, issue.title as "issue", issue_option.title as "choice", ballot.choice_number
from ballot 
	join user on ballot.voter_id = user.user_id
    join issue on ballot.issue_id = issue.issue_id
    join issue_option on ballot.choice_id = issue_option.option_id;

-- Get all vote counts by issue and option
select issue.serial_number, issue.title, issue_option.title, count(ballot_id)
from issue
	right join issue_option on issue.issue_id = issue_option.issue_id
	left join ballot on ballot.choice_id = issue_option.option_id
group by issue_option.option_id;

select * from afetzner.ballot;

select * from afetzner.user;


use afetzner;

SELECT * from issue JOIN issue_option ON issue.issue_id = issue_option.issue_id;

SELECT ops.issue, ops.option, IF(ISNULL(votes.count), 0, votes.count) as 'count'
	FROM (SELECT issue.serial_number as 'issue', issue.issue_id, issue_option.option_number as 'option', issue_option.option_id
		FROM issue RIGHT JOIN issue_option ON issue.issue_id = issue_option.issue_id) ops
	LEFT JOIN (SELECT ballot.choice_id, count(*) AS 'count'
		FROM ballot GROUP BY (ballot.choice_id)) votes
	ON votes.choice_id = ops.option_id;

