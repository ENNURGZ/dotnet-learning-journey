SELECT p.surname, p.name, IFNULL(SUM(od.price_with_discount * od.product_amount), 0) AS sum
FROM person p
INNER JOIN customer c ON p.id = c.person_id
LEFT JOIN customer_order co ON c.person_id = co.customer_id
LEFT JOIN order_details od ON co.id = od.customer_order_id
GROUP BY p.id, p.surname, p.name
UNION ALL
SELECT NULL, NULL, IFNULL(SUM(od.price_with_discount * od.product_amount), 0)
FROM customer_order co
INNER JOIN order_details od ON co.id = od.customer_order_id
WHERE co.customer_id IS NULL
ORDER BY sum, surname;
