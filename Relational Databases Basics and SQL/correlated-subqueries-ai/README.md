# Database design and SQL (DQL). Correlated subqueries or simple subqueries

## Introduction

1. In these tasks you should use ChatGPT. Ask ChatGPT to generate SQL Select code.  

2. Use your local Unit tests to check generated Select Query.  

3. Save the result of communication with ChatGPT to the file gpt-stage-X.txt  


## Task  

1. Get cities, for which there are no streets, sorted ascending by name. Scheme of result data set: **id | name**
 
2. Get a list of categories, where each product has at least one purchase. Sort ascending by id. Scheme of result data set: **id| name**
 
3. Get list of products which price are higher than the average price of producs in this category. Sort ascendidng by product id. Scheme of result data set: **id | title | price**



### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/CorrelatedSubqueries/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
