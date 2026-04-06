# Database design and SQL (DQL). Simple select

## Task  

1. Select product categories (id, name), sorted ascending by name. Scheme of result data set: **id | name** 
 
2. Select customers (person_id, discount) with discount more then 0, sorted ascending by person_id. Scheme of result data set: **person_id | discount** 

3. Select persons list (surname, name, birth_date) with birthday date range inclusive 1 Jan 2000 – 31 Dec 2010 (date format is YYYY-MM-DD), sorted ascending by surname. Scheme of result data set: **surname | name | birth_date** 

4. Select persons (name, surname) whose last name begins with "Kra", sorted by surname  ascending. Customers with the same surnames should be sorted descending by birthday date. Scheme of result data set: **name | surname**

5. Select persons (surname), sorted ascending by surname. The surnames must be different (not duplicate). Scheme of result data set: **surname** 


### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/SimpleSelect/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
