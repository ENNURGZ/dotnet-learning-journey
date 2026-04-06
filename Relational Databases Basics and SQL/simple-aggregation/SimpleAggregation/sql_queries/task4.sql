SELECT SUM(product_amount) AS product_total, 
       SUM(price_with_discount * product_amount) AS to_pay_total, 
       SUM((price - price_with_discount) * product_amount) AS discount_total
FROM order_details
WHERE price > price_with_discount;
