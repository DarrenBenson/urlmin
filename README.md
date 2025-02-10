# UrlMin - URL Shortener API

A simple and efficient URL shortening service built with ASP.NET Core.

## Features

- Shorten long URLs to compact references
- Retrieve original URLs using short references
- Update existing shortened URLs
- Delete shortened URLs
- RESTful API design
- Input validation
- Comprehensive test coverage

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api` | List all URLs |
| GET | `/api/{urlRef}` | Get specific URL |
| POST | `/api/{longUrl}` | Create short URL |
| PATCH | `/api` | Update existing URL |
| DELETE | `/api/{urlRef}` | Delete URL |

## Getting Started

1. Prerequisites:
   - .NET 8.0 SDK
   - Visual Studio Code or Visual Studio 2022

2. Clone and run:
   ```bash
   git clone https://github.com/yourusername/urlmin.git
   cd urlmin
   dotnet run
   ```

3. Access the API at `http://localhost:5000`

## Development

- Build: `dotnet build`
- Test: `dotnet test`
- Run: `dotnet run`

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.