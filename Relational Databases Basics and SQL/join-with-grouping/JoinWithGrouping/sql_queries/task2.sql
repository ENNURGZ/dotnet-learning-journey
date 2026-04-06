SELECT c.name AS city, SUM(od.price_with_discount * od.product_amount) AS income
FROM city c
INNER JOIN street s ON c.id = s.city_id
INNER JOIN supermarket sm ON s.id = sm.street_id
INNER JOIN customer_order co ON sm.id = co.supermarket_id
INNER JOIN order_details od ON co.id = od.customer_order_id
WHERE co.operation_time BETWEEN '2020-11-01' AND '2020-11-30'
GROUP BY c.name
ORDER BY income ASC, c.name ASC;
