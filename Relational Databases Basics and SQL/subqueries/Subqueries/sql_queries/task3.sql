SELECT co.id as order_id, COUNT(od.id) as items_count
FROM customer_order co
INNER JOIN order_details od ON co.id = od.customer_order_id
WHERE co.operation_time BETWEEN '2021-01-01' AND '2021-12-31'
GROUP BY co.id
HAVING items_count > (
    SELECT AVG(cnt) FROM (
        SELECT COUNT(*) as cnt
        FROM order_details
        GROUP BY customer_order_id
    )
)
ORDER BY items_count, order_id;
