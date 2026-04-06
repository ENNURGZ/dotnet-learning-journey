# Database design and SQL (DML)

## Task  

Write INSERT statements to fill all tables (see DB scheme in file **sql_queries** / **DBScheme.jpg** ) with following minimum row count: 

| table | row count | comment
| ------ | ------ | ------ |
| city | 3 |
| street | 5 |
| supermarket | 7 |
| person | 10 |
| contact_type | 2 |
| person_contact | 20 |
| customer | 10 |
| product_category | 3 |
| product_title | 10 | min 3 per category |
| manufacturer | 4 |
| product | 20 |
| customer_order | 20 |
| order_details | 20 | min 1 row  per customer_order |

### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/SqlDataInsert/sql_queries/DBSchema.jpg)

### How to complete task solution

Save script with queries to file  **sql_queries** / **insert.sql**. Separate queries with “;”.
______
