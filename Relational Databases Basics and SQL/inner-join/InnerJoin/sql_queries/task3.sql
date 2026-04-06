SELECT DISTINCT pt.title, p.price
FROM product p
INNER JOIN product_title pt ON p.product_title_id = pt.id
INNER JOIN order_details od ON p.id = od.product_id
INNER JOIN customer_order co ON od.customer_order_id = co.id
INNER JOIN customer c ON co.customer_id = c.person_id
INNER JOIN person ps ON c.person_id = ps.id
WHERE ps.birth_date BETWEEN '2000-01-01' AND '2010-12-31'
ORDER BY pt.title ASC, p.price ASC;
