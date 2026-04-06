# Database design and SQL (DDL)

## Task  

Create a database scheme for the domain (see description). The BD scheme must correspond to at least the third normal form (3NF).

DB should contain at least 7 tables with primary and foreign keys, unique, not null attributes where it is suitable.

### Restriction for create table statements
- Do not use ALTER operations 
- Don’t use cascading deletion from tables (is forbitten).
- Please use only simple primary and foreign keys (single column/attribute).
- Do not use IF NOT EXISTS statement.
- Do not use `` to escape a table name or column name.
- Don`t use digits in table names or column names.
- Use CHECK statement only in next format ColumnName ColumnType ColumnSpecification CHECK(CheckExpression). CheckExpression must contains only: column name, <,<=,>,>= , <>, BETWEEN (for example Age INTEGER NOT NULL CHECK (Age>0 and Age < 200))
- Use only  CREATE TABLE statements (CREATE index, etc. are forbitten) 



### Domain description   

- The company employees carry out projects (project data: name, date of creation, status open / closed, date of closure).   
- An employee can carry out several projects, on different projects his position may differ.  
- Tasks for the project are issued to the employee with a deadline. The task can be in the status: open, completed, requires revision, accepted (closed), indicating the date of setting the status and the employee setting the status. 

### How to complete task solution

1. Design DB scheme and save it’s screenshot in file **DBScheme.jpg**. Upload it to folder **sql_queries**. 
1. Save script with queries to file  **sql_queries** / **create.sql**. Separate queries with “;”.
______
