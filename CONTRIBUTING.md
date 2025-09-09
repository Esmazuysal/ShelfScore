# Contributing to ShelfScore

Thank you for your interest in contributing to ShelfScore! This document provides guidelines and information for contributors.

## 🚀 Getting Started

### Prerequisites
- Flutter SDK 3.24.0+
- .NET 8.0 SDK
- Git
- Docker (optional)

### Development Setup
1. Fork the repository
2. Clone your fork: `git clone https://github.com/yourusername/shelfscore.git`
3. Create a feature branch: `git checkout -b feature/amazing-feature`
4. Install dependencies:
   ```bash
   # Backend
   cd backend_api
   dotnet restore
   
   # Frontend
   cd mobile_app
   flutter pub get
   ```

## 📝 Code Style

### Flutter/Dart
- Follow [Dart Style Guide](https://dart.dev/guides/language/effective-dart/style)
- Use `flutter analyze` to check code quality
- Write tests for new features
- Use meaningful variable and function names

### .NET/C#
- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful names
- Add XML documentation for public APIs
- Write unit tests for new features

## 🧪 Testing

### Flutter Tests
```bash
cd mobile_app
flutter test --coverage
flutter analyze
```

### Backend Tests
```bash
cd backend_api
dotnet test --collect:"XPlat Code Coverage"
```

### Run All Tests
```bash
./scripts/test.sh
```

## 📋 Pull Request Process

1. **Create a feature branch** from `main`
2. **Write tests** for your changes
3. **Ensure all tests pass** locally
4. **Update documentation** if needed
5. **Commit your changes** with descriptive messages
6. **Push to your fork** and create a Pull Request

### Commit Message Format
```
type(scope): description

[optional body]

[optional footer]
```

Types:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

## 🐛 Bug Reports

When reporting bugs, please include:
- Clear description of the issue
- Steps to reproduce
- Expected vs actual behavior
- Screenshots (if applicable)
- Environment details (OS, Flutter version, etc.)

## ✨ Feature Requests

When requesting features, please include:
- Clear description of the feature
- Use case and motivation
- Proposed implementation (if you have ideas)
- Any additional context

## 📚 Documentation

- Update README.md for significant changes
- Add inline comments for complex code
- Update API documentation for backend changes
- Add screenshots for UI changes

## 🔒 Security

- Report security vulnerabilities privately to security@shelfscore.com
- Do not create public issues for security vulnerabilities
- Follow responsible disclosure practices

## 📞 Getting Help

- Create an issue for questions
- Join our Discord community (link coming soon)
- Email: support@shelfscore.com

## 🎯 Areas for Contribution

- **Frontend**: UI/UX improvements, new features
- **Backend**: API enhancements, performance optimizations
- **Testing**: Increase test coverage, add integration tests
- **Documentation**: Improve guides, add examples
- **DevOps**: CI/CD improvements, deployment automation
- **AI/ML**: Photo analysis improvements, new algorithms

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to ShelfScore! 🎉
