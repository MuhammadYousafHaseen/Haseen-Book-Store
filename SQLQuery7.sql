
INSERT INTO dbo.Books
    (CategoryId, Title, Author, Description, Price, Stock)
SELECT c.CategoryId, v.Title, v.Author, v.Description, v.Price, v.Stock
FROM (VALUES
    ('Fiction', 'The Silent River', 'A. Khan',
     'A compelling fictional story.', 850.00, 12),
    ('Science', 'A Beginner''s Guide to Science', 'S. Ahmed',
     'An introduction to scientific concepts.', 1200.00, 8),
    ('Technology', 'Learning Computer Programming', 'M. Ali',
     'Programming fundamentals for beginners.', 1500.00, 10),
    ('Business', 'Business Fundamentals', 'R. Malik',
     'Core principles of business management.', 1100.00, 7),
    ('Education', 'Study Skills for Success', 'F. Noor',
     'Practical study and learning techniques.', 650.00, 15),
    ('History', 'Stories from History', 'H. Raza',
     'An introduction to historical events.', 950.00, 9),
    ('Children', 'The Little Explorer', 'N. Fatima',
     'An adventure story for young readers.', 450.00, 20),
    ('Self-Help', 'Building Better Habits', 'U. Hassan',
     'A guide to personal development.', 750.00, 11)
) AS v(CategoryName, Title, Author, Description, Price, Stock)
INNER JOIN dbo.Categories c
    ON c.CategoryName = v.CategoryName
WHERE c.IsActive = 1
  AND NOT EXISTS (
      SELECT 1 FROM dbo.Books b WHERE b.Title = v.Title
  );

SELECT BookId, Title, Author, Price, Stock
FROM dbo.Books
ORDER BY BookId;
