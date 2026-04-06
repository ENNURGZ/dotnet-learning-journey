# Database design and SQL (DQL). Inner join

## Task  

1. Select products (id, title, price), sorted ascending by title. Scheme of result data set: **id | title | price**

1. Select products (id, title, category name, price), sorted ascending by category name and product title. Scheme of result data set: **id | title | category | price**
 
1. Select products (title, price) which were purchased by customers with birthday date range inclusive 1 Jan 2000 – 31 Dec 2010 (date format is YYYY-MM-DD), sorted ascending by a title and price. Scheme of result data set: **title | price** 



### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/InnerJoin/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
