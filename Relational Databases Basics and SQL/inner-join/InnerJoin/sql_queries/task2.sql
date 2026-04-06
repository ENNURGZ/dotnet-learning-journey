SELECT p.id, pt.title, pc.name AS category, p.price
FROM product p
INNER JOIN product_title pt ON p.product_title_id = pt.id
INNER JOIN product_category pc ON pt.product_category_id = pc.id
ORDER BY pc.name ASC, pt.title ASC;
