SELECT 
    pt.id AS product_id, 
    pt.title,
    (SELECT m.id 
     FROM manufacturer m 
     JOIN product p ON m.id = p.manufacturer_id 
     JOIN order_details od ON p.id = od.product_id 
     WHERE p.product_title_id = pt.id 
     GROUP BY m.id 
     ORDER BY SUM(od.product_amount) DESC, m.id ASC 
     LIMIT 1) AS manufacturer_id,
    (SELECT m.name 
     FROM manufacturer m 
     JOIN product p ON m.id = p.manufacturer_id 
     JOIN order_details od ON p.id = od.product_id 
     WHERE p.product_title_id = pt.id 
     GROUP BY m.id 
     ORDER BY SUM(od.product_amount) DESC, m.id ASC 
     LIMIT 1) AS manufacturer
FROM product_title pt
ORDER BY pt.id ASC;
