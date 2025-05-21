# Navigate to your project directory
cd TestTube

# Drop the database
dotnet ef database drop --force

# Recreate the database with migrations
dotnet ef database update
