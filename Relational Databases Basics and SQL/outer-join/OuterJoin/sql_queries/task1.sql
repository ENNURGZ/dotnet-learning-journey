SELECT pc.name AS category, pt.title AS product
FROM product p
INNER JOIN product_title pt ON p.product_title_id = pt.id
INNER JOIN product_category pc ON pt.product_category_id = pc.id
LEFT JOIN order_details od ON p.id = od.product_id
WHERE od.product_id IS NULL
ORDER BY p.id;
