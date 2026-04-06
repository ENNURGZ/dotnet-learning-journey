-- city (3)
INSERT INTO city (name) VALUES ('Minsk');
INSERT INTO city (name) VALUES ('Gomel');
INSERT INTO city (name) VALUES ('Brest');

-- street (5)
INSERT INTO street (name, city_id) VALUES ('Lenina', 1);
INSERT INTO street (name, city_id) VALUES ('Pobedy', 1);
INSERT INTO street (name, city_id) VALUES ('Sovetskaya', 2);
INSERT INTO street (name, city_id) VALUES ('Mira', 3);
INSERT INTO street (name, city_id) VALUES ('Pushkina', 3);

-- supermarket (7)
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Euroopt', 1, '10');
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Gippo', 2, '5A');
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Korona', 3, '20');
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Almi', 4, '15');
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Dionis', 5, '3');
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Vitalur', 1, '22');
INSERT INTO supermarket (name, street_id, house_number) VALUES ('Santa', 3, '12');

-- person (10)
INSERT INTO person (name, surname, birth_date) VALUES ('Ivan', 'Ivanov', '1990-01-01');
INSERT INTO person (name, surname, birth_date) VALUES ('Petr', 'Petrov', '1985-05-15');
INSERT INTO person (name, surname, birth_date) VALUES ('Sidor', 'Sidorov', '1995-10-20');
INSERT INTO person (name, surname, birth_date) VALUES ('Alexey', 'Alexeev', '1988-03-12');
INSERT INTO person (name, surname, birth_date) VALUES ('Dmitry', 'Dmitriev', '1992-07-07');
INSERT INTO person (name, surname, birth_date) VALUES ('Sergey', 'Sergeev', '1980-12-31');
INSERT INTO person (name, surname, birth_date) VALUES ('Andrey', 'Andreev', '1998-06-25');
INSERT INTO person (name, surname, birth_date) VALUES ('Mikhail', 'Mikhailov', '1982-11-11');
INSERT INTO person (name, surname, birth_date) VALUES ('Nikolay', 'Nikolaev', '1993-04-04');
INSERT INTO person (name, surname, birth_date) VALUES ('Viktor', 'Viktorov', '1987-09-09');

-- contact_type (2)
INSERT INTO contact_type (name) VALUES ('Phone');
INSERT INTO contact_type (name) VALUES ('Email');

-- person_contact (20)
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (1, 1, '+375291111111');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (1, 2, 'ivan@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (2, 1, '+375292222222');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (2, 2, 'petr@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (3, 1, '+375293333333');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (3, 2, 'sidor@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (4, 1, '+375294444444');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (4, 2, 'alex@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (5, 1, '+375295555555');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (5, 2, 'dmitry@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (6, 1, '+375296666666');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (6, 2, 'sergey@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (7, 1, '+375297777777');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (7, 2, 'andrey@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (8, 1, '+375298888888');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (8, 2, 'mikhail@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (9, 1, '+375299999999');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (9, 2, 'nikolay@mail.com');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (10, 1, '+375290000000');
INSERT INTO person_contact (person_id, contact_type_id, contact_value) VALUES (10, 2, 'viktor@mail.com');

-- customer (10)
INSERT INTO customer (person_id, card_number, discount) VALUES (1, 1001, 5.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (2, 1002, 3.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (3, 1003, 7.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (4, 1004, 0.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (5, 1005, 10.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (6, 1006, 2.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (7, 1007, 4.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (8, 1008, 6.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (9, 1009, 1.0);
INSERT INTO customer (person_id, card_number, discount) VALUES (10, 1010, 8.0);

-- product_category (3)
INSERT INTO product_category (name) VALUES ('Dairy');
INSERT INTO product_category (name) VALUES ('Grocery');
INSERT INTO product_category (name) VALUES ('Drinks');

-- product_title (10, min 3 per category)
INSERT INTO product_title (title, product_category_id) VALUES ('Milk', 1);
INSERT INTO product_title (title, product_category_id) VALUES ('Cheese', 1);
INSERT INTO product_title (title, product_category_id) VALUES ('Yogurt', 1);
INSERT INTO product_title (title, product_category_id) VALUES ('Butter', 1);
INSERT INTO product_title (title, product_category_id) VALUES ('Bread', 2);
INSERT INTO product_title (title, product_category_id) VALUES ('Sugar', 2);
INSERT INTO product_title (title, product_category_id) VALUES ('Salt', 2);
INSERT INTO product_title (title, product_category_id) VALUES ('Water', 3);
INSERT INTO product_title (title, product_category_id) VALUES ('Juice', 3);
INSERT INTO product_title (title, product_category_id) VALUES ('Coffee', 3);

-- manufacturer (4)
INSERT INTO manufacturer (name) VALUES ('Savushkin');
INSERT INTO manufacturer (name) VALUES ('Brest-Litovsk');
INSERT INTO manufacturer (name) VALUES ('Coca-Cola');
INSERT INTO manufacturer (name) VALUES ('Nestle');

-- product (20)
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (1, 1, 2.50, 'Fresh milk');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (2, 2, 5.00, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (3, 1, 1.20, 'Very sweet');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (4, 2, 4.50, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (5, 4, 1.00, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (6, 4, 3.00, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (7, 4, 0.80, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (8, 3, 1.50, 'Pure water');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (9, 3, 3.50, 'Orange juice');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (10, 4, 15.00, 'Premium coffee');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (1, 2, 2.60, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (2, 1, 5.20, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (3, 2, 1.30, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (4, 1, 4.40, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (5, 2, 1.10, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (6, 2, 2.90, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (8, 1, 1.40, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (9, 1, 3.30, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (10, 1, 12.00, '');
INSERT INTO product (product_title_id, manufacturer_id, price, comment) VALUES (8, 4, 1.60, '');

-- customer_order (20)
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-01 10:00:00', 1, 1);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-02 11:00:00', 2, 2);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-03 12:00:00', 3, 3);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-04 13:00:00', 4, 4);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-05 14:00:00', 5, 5);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-06 15:00:00', 1, 6);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-07 16:00:00', 2, 7);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-08 17:00:00', 3, 8);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-09 18:00:00', 4, 9);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-10 19:00:00', 5, 10);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-11 10:30:00', 6, 1);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-12 11:30:00', 7, 2);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-13 12:30:00', 1, 3);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-14 13:30:00', 2, 4);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-15 14:30:00', 3, 5);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-16 15:30:00', 4, 6);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-17 16:30:00', 5, 7);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-18 17:30:00', 6, 8);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-19 18:30:00', 7, 9);
INSERT INTO customer_order (operation_time, supermarket_id, customer_id) VALUES ('2024-01-20 19:30:00', 1, 10);

-- order_details (20, min 1 per order)
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (1, 1, 2.50, 2.37, 2);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (2, 2, 5.00, 4.85, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (3, 3, 1.20, 1.11, 3);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (4, 4, 4.50, 4.50, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (5, 5, 1.00, 0.90, 5);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (6, 6, 3.00, 2.94, 2);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (7, 7, 0.80, 0.77, 4);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (8, 8, 1.50, 1.41, 10);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (9, 9, 3.50, 3.46, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (10, 10, 15.00, 13.80, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (11, 1, 2.50, 2.37, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (12, 2, 5.00, 4.85, 2);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (13, 3, 1.20, 1.11, 2);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (14, 4, 4.50, 4.50, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (15, 5, 1.00, 0.90, 3);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (16, 6, 3.00, 2.94, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (17, 7, 0.80, 0.77, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (18, 8, 1.50, 1.41, 2);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (19, 9, 3.50, 3.46, 1);
INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount) VALUES (20, 10, 15.00, 13.80, 1);
