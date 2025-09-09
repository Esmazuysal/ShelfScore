# ShelfScore API Documentation

## Overview

ShelfScore provides a RESTful API built with .NET 8.0 Web API. The API supports authentication, employee management, photo analysis, and store management.

**Base URL**: `http://localhost:5000/api`

## Authentication

All API endpoints (except login and register) require JWT authentication.

### Headers
```
Authorization: Bearer <jwt_token>
Content-Type: application/json
```

## Endpoints

### Authentication

#### POST /api/auth/login
Login with username and password.

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Response:**
```json
{
  "token": "string",
  "user": {
    "id": 1,
    "username": "string",
    "email": "string",
    "role": "Manager|Employee",
    "departmentId": 1
  }
}
```

#### POST /api/auth/register
Register a new manager account.

**Request Body:**
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "storeName": "string",
  "storeAddress": "string"
}
```

**Response:**
```json
{
  "token": "string",
  "user": {
    "id": 1,
    "username": "string",
    "email": "string",
    "role": "Manager",
    "departmentId": 1
  }
}
```

### Employee Management

#### GET /api/employees
Get all employees for the authenticated manager.

**Response:**
```json
[
  {
    "id": 1,
    "username": "string",
    "email": "string",
    "role": "Employee",
    "departmentId": 1,
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

#### POST /api/employees
Create a new employee.

**Request Body:**
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "departmentId": 1
}
```

#### PUT /api/employees/{id}
Update an employee.

**Request Body:**
```json
{
  "username": "string",
  "email": "string",
  "departmentId": 1
}
```

#### DELETE /api/employees/{id}
Delete an employee.

### Photo Management

#### GET /api/photos
Get all photos for the authenticated user.

**Query Parameters:**
- `page` (optional): Page number (default: 1)
- `limit` (optional): Items per page (default: 10)

**Response:**
```json
{
  "photos": [
    {
      "id": 1,
      "fileName": "string",
      "filePath": "string",
      "uploadedAt": "2024-01-01T00:00:00Z",
      "analysisResult": "string",
      "score": 85
    }
  ],
  "totalCount": 10,
  "page": 1,
  "totalPages": 1
}
```

#### POST /api/photos
Upload a new photo.

**Request:** Multipart form data
- `file`: Image file (jpg, jpeg, png, webp)
- `description` (optional): Photo description

**Response:**
```json
{
  "id": 1,
  "fileName": "string",
  "filePath": "string",
  "uploadedAt": "2024-01-01T00:00:00Z",
  "analysisResult": "string",
  "score": 85
}
```

#### DELETE /api/photos/{id}
Delete a photo.

### Statistics

#### GET /api/statistics
Get statistics for the authenticated user.

**Response:**
```json
{
  "totalPhotos": 100,
  "averageScore": 85.5,
  "photosThisMonth": 25,
  "topPerformers": [
    {
      "employeeId": 1,
      "employeeName": "string",
      "averageScore": 90.0,
      "photoCount": 15
    }
  ]
}
```

### Store Information

#### GET /api/store
Get store information for the authenticated manager.

**Response:**
```json
{
  "id": 1,
  "name": "string",
  "address": "string",
  "phone": "string",
  "email": "string",
  "managerId": 1
}
```

#### PUT /api/store
Update store information.

**Request Body:**
```json
{
  "name": "string",
  "address": "string",
  "phone": "string",
  "email": "string"
}
```

### Announcements

#### GET /api/announcements
Get all announcements for the authenticated user.

**Response:**
```json
[
  {
    "id": 1,
    "title": "string",
    "content": "string",
    "createdAt": "2024-01-01T00:00:00Z",
    "isImportant": true
  }
]
```

#### POST /api/announcements
Create a new announcement (Manager only).

**Request Body:**
```json
{
  "title": "string",
  "content": "string",
  "isImportant": false
}
```

## Error Responses

All error responses follow this format:

```json
{
  "error": "string",
  "message": "string",
  "details": "string"
}
```

### Common HTTP Status Codes

- `200 OK`: Request successful
- `201 Created`: Resource created successfully
- `400 Bad Request`: Invalid request data
- `401 Unauthorized`: Authentication required
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Rate Limiting

- **Authentication endpoints**: 5 requests per minute
- **Photo upload**: 10 requests per minute
- **Other endpoints**: 100 requests per minute

## File Upload Limits

- **Maximum file size**: 10MB
- **Allowed formats**: JPG, JPEG, PNG, WEBP
- **Maximum dimensions**: 4096x4096 pixels

## CORS Configuration

The API supports CORS for the following origins:
- `http://localhost:8081` (Development)
- `https://yourdomain.com` (Production)

## Swagger Documentation

Interactive API documentation is available at:
- **Development**: `http://localhost:5000/swagger`
- **Production**: `https://yourdomain.com/swagger`

## SDKs and Libraries

### Flutter
```yaml
dependencies:
  http: ^1.1.0
  dio: ^5.3.2
```

### JavaScript/TypeScript
```bash
npm install axios
```

### C#
```xml
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" />
```

## Examples

### Flutter Example
```dart
import 'package:http/http.dart' as http;

class ApiService {
  static const String baseUrl = 'http://localhost:5000/api';
  
  Future<Map<String, dynamic>> login(String username, String password) async {
    final response = await http.post(
      Uri.parse('$baseUrl/auth/login'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({
        'username': username,
        'password': password,
      }),
    );
    
    return jsonDecode(response.body);
  }
}
```

### JavaScript Example
```javascript
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add auth token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Login
const login = async (username, password) => {
  const response = await api.post('/auth/login', {
    username,
    password,
  });
  return response.data;
};
```

## Support

For API support:
- **Email**: api-support@shelfscore.com
- **GitHub Issues**: Create an issue with the `api` label
- **Documentation**: Check the Swagger UI for the latest updates
