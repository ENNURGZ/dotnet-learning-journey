CREATE TABLE categories
(
id INTEGER PRIMARY KEY,
category_name TEXT NOT NULL
);
CREATE TABLE manufacturers
(
id INTEGER PRIMARY KEY,
manufacturer_name TEXT NOT NULL
);
CREATE TABLE user_roles
(
id INTEGER PRIMARY KEY,
user_role_name TEXT NOT NULL
);
CREATE TABLE users
(
id INTEGER PRIMARY KEY,
first_name TEXT NOT NULL,
last_name TEXT NOT NULL,
Password TEXT NOT NULL,
user_role_id INTEGER NOT NULL,
FOREIGN KEY (user_role_id) REFERENCES user_roles (id) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE TABLE product_titles 
(
id INTEGER PRIMARY KEY,
product_title TEXT NOT NULL,
product_category_id INTEGER NOT NULL,
FOREIGN KEY (product_category_id) REFERENCES product_categories (id) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE TABLE products
(
id INTEGER PRIMARY KEY,
product_title_id INTEGER NOT NULL,
product_manufacturer_id INTEGER NOT NULL,
unit_price REAL NOT NULL,
comment TEXT NOT NULL,
FOREIGN KEY (product_title_id) REFERENCES product_titles (id) ON DELETE CASCADE ON UPDATE NO ACTION,
FOREIGN KEY (product_manufacturer_id) REFERENCES product_manufacturers (id) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE TABLE order_states
(
id INTEGER PRIMARY KEY,
state_name TEXT NOT NULL
);
CREATE TABLE customer_orders 
(
id INTEGER PRIMARY KEY AUTOINCREMENT,
operation_time TEXT NOT NULL,
customer_id INTEGER NOT NULL,
order_state_id INTEGER NOT NULL, 
FOREIGN KEY (customer_id) REFERENCES users (id) ON DELETE CASCADE ON UPDATE CASCADE,
FOREIGN KEY (order_state_id) REFERENCES order_states (id) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE TABLE sqlite_sequence(name,seq);
CREATE TABLE customer_order_details
(
id INTEGER PRIMARY KEY,
customer_order_id INTEGER NOT NULL,
product_id INTEGER NOT NULL,
price REAL NOT NULL,
product_amount INTEGER NOT NULL,
FOREIGN KEY (product_id) REFERENCES shop_products (product_id) ON DELETE CASCADE ON UPDATE CASCADE,
FOREIGN KEY (customer_order_id) REFERENCES customer_orders (customer_order_id) ON DELETE CASCADE ON UPDATE CASCADE
);
