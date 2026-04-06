SELECT customer_order_id, SUM(price * product_amount) AS to_pay
FROM order_details
GROUP BY customer_order_id
HAVING to_pay > 100
ORDER BY customer_order_id ASC;
