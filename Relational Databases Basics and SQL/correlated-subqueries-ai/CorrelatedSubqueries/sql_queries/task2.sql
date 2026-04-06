SELECT id, name
FROM product_category pc
WHERE EXISTS (
    SELECT 1
    FROM product p
    JOIN product_title pt ON p.product_title_id = pt.id
    WHERE pt.product_category_id = pc.id
)
AND NOT EXISTS (
    SELECT 1
    FROM product p
    JOIN product_title pt ON p.product_title_id = pt.id
    WHERE pt.product_category_id = pc.id
    AND p.id NOT IN (SELECT product_id FROM order_details)
)
ORDER BY id;
