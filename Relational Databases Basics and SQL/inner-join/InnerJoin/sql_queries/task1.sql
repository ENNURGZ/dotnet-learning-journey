SELECT p.id, pt.title, p.price
FROM product p
INNER JOIN product_title pt ON p.product_title_id = pt.id
ORDER BY pt.title ASC;