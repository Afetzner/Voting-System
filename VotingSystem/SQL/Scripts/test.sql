-- get all admins with username & password
select admin_id, username, password 
from user join admin on user.user_id = admin.user_id;

-- get all voters with username & password
select serial_number, username, password, first_name, last_name 
from user join voter on user.user_id = voter.user_id;

-- See all issues with options
select serial_number, issue.title, start_date, end_date, description, issue.title, option_number, issue_option.title 
from issue join issue_option on issue.issue_id = issue_option.issue_id;

-- See all ballots
select ballot_serial_number, voter_serial_number, voter.first_name, voter.last_name, issue_serial_number, issue.title as "issue", issue_option.title as "choice"
from ballot 
	join voter on ballot.voter_id = voter.voter_id
    join issue on ballot.issue_id = issue.issue_id
    join issue_option on ballot.choice_id = issue_option.option_id;

-- Get all vote counts by issue and option
select issue.serial_number, issue.title, issue_option.title, count(ballot_id)
from issue
	right join issue_option on issue.issue_id = issue_option.issue_id
	left join ballot on ballot.choice_id = issue_option.option_id
group by issue_option.option_id;