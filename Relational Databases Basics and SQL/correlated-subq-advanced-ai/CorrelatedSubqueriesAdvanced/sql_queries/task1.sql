SELECT 
    p.name, 
    p.surname, 
    ROUND(groups.avg_purchase, 2) AS avg_purchase,
    groups.sum_purchase
FROM (
    SELECT 
        co.customer_id,
        SUM(od.price_with_discount * od.product_amount) / CAST(COUNT(*) AS FLOAT) AS avg_purchase,
        SUM(od.price_with_discount * od.product_amount) AS sum_purchase
    FROM customer_order co
    JOIN order_details od ON co.id = od.customer_order_id
    GROUP BY co.customer_id
) AS groups
LEFT JOIN person p ON groups.customer_id = p.id
WHERE groups.avg_purchase > 70
ORDER BY sum_purchase ASC, p.surname ASC;
