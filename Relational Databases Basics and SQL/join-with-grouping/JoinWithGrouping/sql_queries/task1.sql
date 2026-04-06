SELECT pc.name AS category, COUNT(p.id) AS count
FROM product_category pc
INNER JOIN product_title pt ON pc.id = pt.product_category_id
INNER JOIN product p ON pt.id = p.product_title_id
GROUP BY pc.name
ORDER BY pc.name ASC;
