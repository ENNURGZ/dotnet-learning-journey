import re

with open(r'C:\Users\ennur\OneDrive\Masaüstü\epam\Relational Databases Basics and SQL\simple-aggregation\SimpleAggregation.Tests\Data\insert.sql', 'r', encoding='utf-8') as f:
    content = f.read()

# Extract order_details rows
# VALUES (1, 1, 1, 20, 5, 4), ...
match = re.search(r'INSERT INTO order_details\(.*?\)\s*VALUES\s*(.*?);', content, re.DOTALL)
if match:
    rows_text = match.group(1)
    # Extract values: (id, customer_order_id, product_id, price, price_with_discount, product_amount)
    rows = re.findall(r'\((.*?)\)', rows_text)
    
    product_total = 0
    to_pay_total = 0.0
    discount_total = 0.0
    
    for row in rows:
        parts = [p.strip() for p in row.split(',')]
        price = float(parts[3])
        price_with_discount = float(parts[4])
        amount = int(parts[5])
        
        if price_with_discount < price:
            product_total += amount
            to_pay_total += price_with_discount * amount
            discount_total += (price - price_with_discount) * amount
        
    print(f"product_total: {product_total}")
    print(f"to_pay_total: {to_pay_total}")
    print(f"discount_total: {discount_total}")
else:
    print("No order_details found")
