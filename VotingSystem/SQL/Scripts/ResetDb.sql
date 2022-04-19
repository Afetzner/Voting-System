use afetzner;

DROP TABLE IF EXISTS tab_ballot;
DROP TABLE IF EXISTS tab_ballot_issue_option;
DROP TABLE IF EXISTS tab_ballot_issue;
DROP TABLE IF EXISTS tab_voter;
DROP TABLE IF EXISTS tab_admin;
DROP TABLE IF EXISTS tab_user;

CREATE TABLE tab_user (
	user_id int auto_increment,
    username varchar(31) NOT NULL,
    password varchar(31) NOT NULL,
    PRIMARY KEY (user_id)
);

CREATE TABLE tab_admin (
	admin_id int auto_increment,
    user_id int,
    serial_number varchar(9) NOT NULL UNIQUE,
    PRIMARY KEY (admin_id),
    FOREIGN KEY (user_id) REFERENCES tab_user(user_id)
);

CREATE TABLE tab_voter (
	voter_id int auto_increment,
    user_id int,
    serial_number varchar(9) NOT NULL UNIQUE,
    first_name varchar(31) NOT NULL,
    last_name varchar(31) NOT NULL,
    PRIMARY KEY (voter_id),
    FOREIGN KEY (user_id) REFERENCES tab_user(user_id)
);

CREATE TABLE tab_ballot_issue (
	issue_id int auto_increment,
    serial_number varchar(9) NOT NULL UNIQUE,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    title varchar(127) NOT NULL,
    description varchar(255) NOT NULL,
    PRIMARY KEY(issue_id)
);

CREATE TABLE tab_ballot_issue_option (
	option_id int auto_increment,
    option_number int NOT NULL,
    title varchar(127) NOT NULL,
    PRIMARY KEY (option_id)
);

CREATE TABLE tab_ballot (
	ballot_id int auto_increment,
    serial_number varchar(9) NOT NULL UNIQUE,
    voter int,
    issue int,
    choice int,
    PRIMARY KEY (ballot_id),
    FOREIGN KEY (voter) REFERENCES tab_voter(voter_id),
    FOREIGN KEY (issue) REFERENCES tab_ballot_issue(issue_id),
    FOREIGN KEY (choice) REFERENCES tab_ballot_issue_option(option_id)
);
