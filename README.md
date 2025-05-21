# TestTube

TestTube is a .NET 8 Web API for managing laboratory scientists and their equipment. It provides a simple RESTful API for tracking scientists, their departments, and the laboratory equipment assigned to them.

## Features

- **Scientists Management**: Track scientists with their name, department, email, and hire date
- **Equipment Management**: Manage laboratory equipment with details like name, serial number, manufacturer, purchase date, and price
- **RESTful API**: Simple and intuitive API endpoints for accessing and managing data
- **PostgreSQL Database**: Persistent storage using PostgreSQL with Entity Framework Core
- **Comprehensive Testing**: Includes unit and integration tests with in-memory database support

## API Endpoints

### Scientists

- `GET /scientists` - Get a list of all scientists
- `GET /scientists/{id}` - Get detailed information about a specific scientist, including their equipment

### Equipment

- `GET /equipment` - Get a list of all laboratory equipment
- `GET /equipment/{id}` - Get detailed information about a specific piece of equipment, including the scientist it's assigned to

## Technology Stack

- **Framework**: .NET 7 with Minimal API
- **ORM**: Entity Framework Core 7
- **Database**: PostgreSQL
- **Testing**: xUnit with in-memory database for integration tests

## Getting Started

### Prerequisites

- .NET 7 SDK
- PostgreSQL database

### Database Setup

The application is configured to use PostgreSQL. Update the connection string in `appsettings.json` to point to your PostgreSQL instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=testtube;Username=your_username;Password=your_password"
  }
}
```

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

```bash
cd TestTube
dotnet run
```

Alternatively, use the provided shell script:

```bash
./start-api.sh
```

The API will be available at `http://localhost:5081` by default.

### Running Tests

To run the tests:

```bash
cd TestTube.Tests
dotnet test
```

## Data Model

### Scientist

- `Id` (int): Unique identifier
- `Name` (string): Scientist's full name
- `Department` (string): Department the scientist works in
- `Email` (string): Contact email
- `HireDate` (DateTime): Date when the scientist was hired
- `Equipment` (Collection): List of equipment assigned to the scientist

### Equipment

- `Id` (int): Unique identifier
- `Name` (string): Equipment name
- `SerialNumber` (string): Unique serial number
- `Manufacturer` (string): Equipment manufacturer
- `PurchaseDate` (DateTime): Date when the equipment was purchased
- `Price` (decimal): Purchase price
- `ScientistId` (int): ID of the scientist who owns this equipment
- `Scientist` (Navigation property): Reference to the scientist

## Seed Data

The application comes pre-configured with seed data for testing:

### Scientists:
- Marie Curie (Physics)
- Albert Einstein (Physics)
- Rosalind Franklin (Chemistry)

### Equipment:
- Microscope (assigned to Marie Curie)
- Centrifuge (assigned to Marie Curie)
- Spectrometer (assigned to Albert Einstein)
- PCR Machine (assigned to Rosalind Franklin)

## License

[MIT License](LICENSE)