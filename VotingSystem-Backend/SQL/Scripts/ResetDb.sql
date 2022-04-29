use afetzner;

DROP TABLE IF EXISTS ballot;
DROP TABLE IF EXISTS issue_option;
DROP TABLE IF EXISTS issue;
DROP TABLE IF EXISTS voter;
DROP TABLE IF EXISTS admin;
DROP TABLE IF EXISTS user;

CREATE TABLE user (
	user_id int auto_increment,
    username varchar(31) NOT NULL UNIQUE,
    password varchar(31) NOT NULL,
    email varchar(63) NOT NULL,
	first_name varchar(31) NOT NULL,
    last_name varchar(31) NOT NULL,
    serial_number varchar(9) NOT NULL UNIQUE,
    is_admin bool NOT NULL,
    
    PRIMARY KEY (user_id)
);

CREATE TABLE issue (
	issue_id int auto_increment,
    serial_number varchar(9) NOT NULL UNIQUE,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    title varchar(127) NOT NULL UNIQUE,
    description varchar(255) NOT NULL,
    
    PRIMARY KEY(issue_id)
);

CREATE TABLE issue_option (
	option_id int auto_increment,
    option_number int NOT NULL,
    title varchar(127) NOT NULL UNIQUE,
    issue_id int NOT NULL,
    issue_serial varchar(9) NOT NULL,
    
    PRIMARY KEY (option_id),
    FOREIGN KEY (issue_id) REFERENCES issue(issue_id)
);

CREATE TABLE ballot (
	ballot_id int auto_increment,
    ballot_serial_number varchar(9) NOT NULL UNIQUE,
    voter_serial_number varchar(9) NOT NULL,
    issue_serial_number varchar(9) NOT NULL,
    choice_number int NOT NULL,
    voter_id int NOT NULL,
    issue_id int NOT NULL,
    choice_id int NOT NULL,
    
    PRIMARY KEY (ballot_id),
    FOREIGN KEY (voter_id) REFERENCES user(user_id),
    FOREIGN KEY (issue_id) REFERENCES issue(issue_id),
    FOREIGN KEY (choice_id) REFERENCES issue_option(option_id)
);
