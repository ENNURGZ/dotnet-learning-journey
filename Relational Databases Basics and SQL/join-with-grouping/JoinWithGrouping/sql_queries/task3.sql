SELECT p.surname, p.name, p.birth_date, SUM(od.price_with_discount * od.product_amount) AS sum
FROM person p
INNER JOIN customer c ON p.id = c.person_id
INNER JOIN customer_order co ON c.person_id = co.customer_id
INNER JOIN order_details od ON co.id = od.customer_order_id
WHERE c.discount = 0 AND co.operation_time BETWEEN '2021-01-01' AND '2021-12-31'
GROUP BY p.surname, p.name, p.birth_date
ORDER BY sum ASC, p.surname ASC;
