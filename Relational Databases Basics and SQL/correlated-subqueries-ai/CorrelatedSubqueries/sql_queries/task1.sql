SELECT id, name
FROM city
WHERE id NOT IN (SELECT city_id FROM street)
ORDER BY name;
