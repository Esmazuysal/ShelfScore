# Security Policy

## Supported Versions

We provide security updates for the following versions of ShelfScore:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Reporting a Vulnerability

We take security vulnerabilities seriously. If you discover a security vulnerability in ShelfScore, please report it responsibly.

### How to Report

**DO NOT** create a public GitHub issue for security vulnerabilities.

Instead, please:

1. **Email us directly** at: security@shelfscore.com
2. **Include the following information**:
   - Description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Suggested fix (if any)
   - Your contact information

### What to Expect

- **Response Time**: We will acknowledge your report within 48 hours
- **Updates**: We will provide regular updates on our progress
- **Credit**: We will credit you in our security advisories (unless you prefer to remain anonymous)
- **Timeline**: We aim to resolve critical vulnerabilities within 7 days

### Security Best Practices

#### For Users
- Keep your ShelfScore installation updated
- Use strong, unique passwords
- Enable two-factor authentication where available
- Regularly review user access and permissions
- Monitor system logs for suspicious activity

#### For Developers
- Follow secure coding practices
- Validate all input data
- Use parameterized queries to prevent SQL injection
- Implement proper authentication and authorization
- Keep dependencies updated
- Use HTTPS in production
- Implement proper error handling without exposing sensitive information

### Security Features

ShelfScore includes the following security features:

- **Authentication**: JWT-based authentication system
- **Authorization**: Role-based access control (RBAC)
- **Password Security**: BCrypt hashing with configurable work factor
- **Input Validation**: Comprehensive input validation and sanitization
- **SQL Injection Prevention**: Parameterized queries and Entity Framework Core
- **XSS Protection**: Output encoding and Content Security Policy
- **CORS Configuration**: Properly configured Cross-Origin Resource Sharing
- **File Upload Security**: File type validation and size limits
- **Session Management**: Secure session handling with timeout
- **Error Handling**: Secure error messages without information disclosure

### Known Security Considerations

- **File Uploads**: Only image files are allowed, with size limits
- **Database**: SQLite database files should be properly secured
- **JWT Tokens**: Tokens expire after 60 minutes by default
- **CORS**: Configured for specific origins in production
- **Logging**: Sensitive information is not logged

### Security Updates

Security updates are released as:
- **Patch releases** (e.g., 1.0.1) for critical security fixes
- **Minor releases** (e.g., 1.1.0) for security improvements
- **Security advisories** published on our GitHub repository

### Third-Party Dependencies

We regularly audit and update our dependencies:
- **Flutter**: Updated to latest stable version
- **.NET**: Updated to latest LTS version
- **NuGet packages**: Regularly updated
- **npm packages**: Regularly updated

### Contact

For security-related questions or concerns:
- **Email**: esuysll1@gmail.com
- **GitHub**: Create a private issue (for non-vulnerability security questions)

---

**Thank you for helping keep ShelfScore secure!** 🛡️
