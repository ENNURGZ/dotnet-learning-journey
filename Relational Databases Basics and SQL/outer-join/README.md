# Database design and SQL (DQL). Outer join

## Task  

1. Get all products (category name, product name) that are not presented in orders, sorted ascending by product id. Scheme of result data set:  **category | product**
 
2. Get a list of authorized customers (last name, first name, date of birth) who has no orders in retailer stores. Sort ascending by last name, birth date. Scheme of result data set: **surname | name | birth_date**

3. Get a list of customers with their total amount of purchases (last name, first name, total expenses). The list includes buyers with and without purchases in retailer stores. The list also includes one anonymous buyer. Sort ascending by customer expenses and surname. Scheme of result data set: **surname | name | sum**



### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/OuterJoin/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
