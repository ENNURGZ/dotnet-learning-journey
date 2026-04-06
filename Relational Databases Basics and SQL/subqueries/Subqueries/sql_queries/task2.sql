SELECT p.surname, p.name, SUM(od.price_with_discount * od.product_amount) as total_expenses
FROM person p
INNER JOIN customer c ON p.id = c.person_id
INNER JOIN customer_order co ON c.person_id = co.customer_id
INNER JOIN order_details od ON co.id = od.customer_order_id
WHERE p.birth_date BETWEEN '2000-01-01' AND '2010-12-31'
GROUP BY p.surname, p.name
HAVING total_expenses > (
    SELECT AVG(total) FROM (
        SELECT SUM(od2.price_with_discount * od2.product_amount) as total
        FROM customer_order co2
        INNER JOIN order_details od2 ON co2.id = od2.customer_order_id
        WHERE co2.customer_id IS NOT NULL
        GROUP BY co2.customer_id
    )
)
ORDER BY total_expenses, p.surname;
