SELECT 
    p.id, 
    p.comment AS title,
    CAST(IFNULL((SELECT SUM(od.product_amount) FROM order_details od WHERE od.product_id = p.id AND (od.price - od.price_with_discount) / od.price > 0.05), 0) AS INTEGER) AS count_with_discount_5,
    CAST(IFNULL((SELECT SUM(od.product_amount) FROM order_details od WHERE od.product_id = p.id AND (od.price - od.price_with_discount) / od.price <= 0.05), 0) AS INTEGER) AS count_without_discount_5,
    ROUND(IFNULL(CAST((
        IFNULL((SELECT SUM(od.product_amount) FROM order_details od WHERE od.product_id = p.id AND (od.price - od.price_with_discount) / od.price > 0.05), 0) - 
        IFNULL((SELECT SUM(od.product_amount) FROM order_details od WHERE od.product_id = p.id AND (od.price - od.price_with_discount) / od.price <= 0.05), 0)
    ) AS FLOAT) * 100.0 / NULLIF(CAST((
        IFNULL((SELECT SUM(od.product_amount) FROM order_details od WHERE od.product_id = p.id AND (od.price - od.price_with_discount) / od.price > 0.05), 0) + 
        IFNULL((SELECT SUM(od.product_amount) FROM order_details od WHERE od.product_id = p.id AND (od.price - od.price_with_discount) / od.price <= 0.05), 0)
    ) AS FLOAT), 0), 0.0), 2) AS difference
FROM product p
WHERE p.id > 0
ORDER BY p.id ASC;
