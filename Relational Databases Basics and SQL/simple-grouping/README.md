# Database design and SQL (DQL). Simple grouping

## Task  

1. Find the number of customers for each discount (customer_count) and their discount (discount), sorted ascending by discount . Scheme of result data set: **customer_count | discount**
 
2. Find for which orders (customer_order_id) the total price (total cost) of products (to_pay) is more than 100, sorted ascending by customer_order_id. Scheme of result data set: customer_order_id | to_pay
 
3. Find order with the highest total discount value among all orders and get it’s discount value (max_order_discount). Scheme of result data set: **max_order_discount**

### Restriction for create table statements
- Do not use Subqueries 



### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/SimpleGrouping/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
