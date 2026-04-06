# Database design and SQL (DQL). Join with grouping

## Task  

1. Select non-empty categories with the total product number for each one (category name, count), sorted ascending by name. Scheme of result data set: **category | count**

2. Select sales revenue for each city (city, income) on 1 Nov 2020 – 30 Nov 2020 inclusive (the date format is YYYY-MM-DD), sorted ascending by revenue and city. Scheme of result data set: **city | income** 

3. Select a list of customers without discount and their summary purchase (surname, name, birth_date, sum)  in the 2021 year (the date format is YYYY-MM-DD), sorted ascending by purchase and surname. Scheme of result data set: **surname | name | birth_date | sum**.

 


### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBScheme](/JoinWithGrouping/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
