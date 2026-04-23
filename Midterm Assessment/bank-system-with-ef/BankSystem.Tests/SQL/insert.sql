INSERT INTO account_owner(account_owner_id, first_name, last_name, email)
VALUES
(1, "Jim", "Qusak", "gq@hollywood.com"),
(2, "Live", "Tyler", "lv@hollywood.com"),
(3, "James", "Cameron", "jq@hollywood.com");


INSERT INTO currency_code(currency_code_id, currency_code)
VALUES
( 1, "USD"),
( 2, "AUD"),
( 3, "CAD"),
( 4, "CHF"),
( 5, "JPY"),
( 6, "EUR"),
( 7, "GBP"),
( 8, "SEK"),
( 9, "CZK"),
(10, "PLN"),
(11, "RUB"),
(12, "TRY"),
(13, "CNH"),
(14, "UAH"),
(15, "KRW"),
(16, "TWD"),
(17, "VND"),
(18, "BYN"),
(19, "INR"),
(20, "HUF");

INSERT INTO bank_account(bank_account_id, account_owner_id, account_number, balance, currency_code_id, bonus_points, overdraft)
VALUES
(1, 1, "5247987635241456", 250.5,   1,  0, 25),
(2, 1, "5248987835248456", 10.99,   1, 10, 10),
(3, 2, "5557987225241304", 2500.45, 1, 50, 150),
(4, 3, "1147987666247856", 10000,   1, 75, 500),
(5, 3, "4447987744243437", 4000.41, 1, 55, 300);

INSERT INTO account_cash_operation(account_cash_operation_id, bank_account_id, amount, operation_date_time)
VALUES
( 1, 1, 2000, "2024-01-01 18:24:57"),
( 2, 1,-1800, "2024-02-01 17:24:57"),
( 3, 1, -100, "2024-03-01 16:24:57"),
( 4, 2,-2000, "2023-01-01 18:24:57"),
( 5, 2,  200, "2023-02-02 18:24:57"),
( 6, 2,  600, "2023-03-15 18:24:57"),
( 7, 3, 1000, "2023-04-16 18:24:57"),
( 8, 3, 3000, "2023-05-17 18:24:57"),
( 9, 3, -600, "2023-06-18 18:24:57"),
(10, 4, 5000, "2023-07-19 18:24:57"),
(11, 4, 5000, "2023-08-20 18:24:57"),
(12, 5,-4000, "2023-09-24 18:24:57"),
(13, 5,-2000, "2023-10-26 18:24:57"),
(14, 5,-2200, "2023-11-28 18:24:57"),
(15, 5, 6000, "2023-12-29 18:24:57");