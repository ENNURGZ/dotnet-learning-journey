# Database design and SQL (DDL)

## Task  

Create a database scheme for the domain (see description). The BD scheme must correspond to at least the third normal form (3NF).

DB should contain at least 15 tables with primary and foreign keys, unique, not null attributes where it is suitable.

### Restriction for create table statements
- Do not use ALTER operations 
- Don’t use cascading deletion from tables (is forbitten).
- Please use only simple primary and foreign keys (single column/attribute).
- Do not use IF NOT EXISTS statement.
- Do not use `` to escape a table name or column name.
- Don`t use digits in table names or column names.
- Use CHECK statement only in next format ColumnName ColumnType ColumnSpecification CHECK(CheckExpression). CheckExpression must contains only: column name, <,<=,>,>= , <>, BETWEEN (for example Age INTEGER NOT NULL CHECK (Age>0 and Age < 200))
- Use only  CREATE TABLE statements (CREATE index, etc. are forbitten) 



### Domain area description   
It is planned to automate the work of a smartphone repair workshop
We assume that the following information will be stored in the database:
- Smartphone: Brand, model, manufacturer, screen resolution, matrix type, RAM, ROM, year of manufacture, IMEI 1, IMEI 2 (Optional); last name and first name of the owner, his phone number, description of the malfunction;  position, last name and first name of the employee who accepted the device for repair; Receipt No.   
- Repair: receipt number;  position, surname and name of the employee performing diagnostics and repairs; description of the malfunction; order state ("Repairable","Unrepairable","Accepted for diagnostics", "On diagnostics", "Under repair", "Ready to be returned to owner", "Returned to owner"; amount to pay for repairs.  


### How to complete task solution

1. Save script with queries to file  **sql_queries** / **create.sql**. Separate queries with “;”.
______
