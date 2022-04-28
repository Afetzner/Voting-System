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

SELECT (IF(EXISTS (SELECT 1 FROM afetzner.user WHERE username = 'jdoe16'), true, false)) INTO @collision;
SELECT @collision;

select * from afetzner.ballot;

	SELECT EXISTS (SELECT 1 FROM afetzner.ballot WHERE voter_serial_number = 'V12399874' AND issue_serial_number = 'I78955500' LIMIT 1) INTO @collision;
    SELECT @collision;
    
    INSERT INTO afetzner.ballot 
		(ballot_serial_number, 
		voter_serial_number,
		issue_serial_number,
		choice_number,
		voter_id,
		issue_id,
		choice_id)
	SELECT
		'B11240001',
        'V12399874',
        'I78955500',
        1,
		(SELECT voter_id FROM afetzner.voter WHERE voter.serial_number = 'V12399874' LIMIT 1),
        (SELECT issue_id FROM afetzner.issue WHERE issue.serial_number = 'I78955500' LIMIT 1),
        (SELECT option_id FROM afetzner.issue_option WHERE issue_option.option_number = 1 LIMIT 1)
	-- Protects against multiple ballots from one voter being entered on any issue? Needs testing
    WHERE NOT @collision;
    
select * from afetzner.user;

select * from afetzner.voter;

	DELETE voter, user, ballot
    FROM voter 
    LEFT JOIN user ON voter.user_id = user.user_id
    LEFT JOIN ballot ON voter.voter_id = ballot.voter_id
    WHERE voter.serial_number = 'V77124460';
    
    SELECT * FROM afetzner.voter where voter.serial_number = 'V77124460';
    
    	SELECT *
    FROM voter 
    LEFT JOIN user ON voter.user_id = user.user_id
    LEFT JOIN ballot ON voter.voter_id = ballot.voter_id
    WHERE voter.serial_number = 'V77124460';
    