CREATE TABLE account_owner
(
    account_owner_id INTEGER  PRIMARY KEY AUTOINCREMENT,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    email TEXT NOT NULL
);


CREATE TABLE currency_code
(
    currency_code_id INTEGER  PRIMARY KEY AUTOINCREMENT,
    currency_code CHAR(5) NOT NULL
);

CREATE TABLE bank_account
(
    bank_account_id INTEGER PRIMARY KEY AUTOINCREMENT,
    account_owner_id INTEGER  NOT NULL,
    account_number CHAR(20),
    balance REAL NOT NULL,
    currency_code_id INTEGER  NOT NULL,
    bonus_points INTEGER  NOT NULL,
    overdraft REAL NOT NULL,
	FOREIGN KEY (account_owner_id) REFERENCES account_owner (account_owner_id) ON DELETE NO ACTION ON UPDATE CASCADE,
	FOREIGN KEY (currency_code_id) REFERENCES currency_code (currency_code_id) ON DELETE NO ACTION ON UPDATE CASCADE
);

CREATE TABLE account_cash_operation
(
    account_cash_operation_id INTEGER  PRIMARY KEY AUTOINCREMENT,
    bank_account_id INTEGER NOT NULL,
    amount REAL NOT NULL,
    operation_date_time DATETIME NOT NULL,
    note VARCHAR(160) NULL,
    FOREIGN KEY (bank_account_id) REFERENCES bank_account(bank_account_id) ON DELETE NO ACTION ON UPDATE CASCADE
);


