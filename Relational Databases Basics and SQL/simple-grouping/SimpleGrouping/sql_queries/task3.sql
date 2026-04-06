SELECT SUM((price - price_with_discount) * product_amount) AS max_order_discount
FROM order_details
GROUP BY customer_order_id
ORDER BY max_order_discount DESC
LIMIT 1;
