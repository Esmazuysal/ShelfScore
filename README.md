iyorumm pemejnile lışt# 🛒 ShelfScore - Raf Fotoğraf Analiz Uygulaması

## 📱 **Proje Hakkında**

ShelfScore, mağaza çalışanlarının raf fotoğraflarını çekip analiz edebileceği, yöneticilerin ise çalışan performansını takip edebileceği modern bir mobil uygulamadır. Flutter ve .NET Web API kullanılarak geliştirilmiştir.

## ✨ **Özellikler**

### 👥 **Kullanıcı Rolleri**
- **Manager/Admin**: Yönetici paneli, çalışan yönetimi, duyuru sistemi
- **Employee**: Fotoğraf çekme, geçmiş görüntüleme, istatistikler

### 🎯 **Ana İşlevler**
- 📸 **Fotoğraf Çekme**: Raf fotoğraflarını çekme ve yükleme
- 🔍 **Fotoğraf Analizi**: AI destekli raf düzeni analizi
- 📊 **Performans Takibi**: Çalışan performans metrikleri
- 📢 **Duyuru Sistemi**: Yönetici-çalışan iletişimi
- 🏪 **Market Yönetimi**: Market bilgileri ve departman yönetimi
- 👤 **Profil Yönetimi**: Kullanıcı profil ve ayarları

## 🏗️ **Teknik Mimari**

### 📱 **Frontend (Flutter)**
```
mobile_app/lib/
├── main.dart                    # Ana uygulama girişi
├── providers/                   # State management
│   ├── auth_provider.dart      # Kimlik doğrulama
│   └── settings_provider.dart  # Ayarlar yönetimi
├── models/                      # Veri modelleri
│   ├── user.dart               # Kullanıcı modeli
│   ├── photo.dart              # Fotoğraf modeli
│   ├── auth.dart               # Kimlik doğrulama modelleri
│   ├── employee.dart           # Çalışan modeli
│   ├── department.dart         # Departman modeli
│   └── store.dart              # Market modeli
├── services/                    # API servisleri
│   ├── api_service.dart        # HTTP istekleri
│   └── user_api_service.dart   # Kullanıcı API servisleri
├── screens/                     # Ekranlar
│   ├── employee/                # Çalışan ekranları
│   │   └── dashboard/          # Dashboard
│   │       └── employee_dashboard.dart
│   ├── admin/                   # Yönetici ekranları
│   │   ├── main_screen.dart    # Ana yönetici dashboard'u
│   │   ├── employee_management_screen.dart
│   │   ├── department_management_screen.dart
│   │   ├── announcement_screen.dart
│   │   ├── market_info_screen.dart
│   │   └── top_performers_screen.dart
│   └── common/                  # Ortak ekranlar
│       ├── login_screen.dart   # Giriş ekranı
│       ├── profile_screen.dart # Profil ekranı
│       ├── results_screen.dart # Sonuçlar ekranı
│       ├── photo_capture_screen.dart
│       ├── photo_history_screen.dart
│       └── statistics_screen.dart
├── theme/                       # Uygulama teması
│   └── app_colors.dart         # Renk paleti
└── widgets/                     # Genel widget'lar
    ├── modern_components.dart  # Modern UI bileşenleri
    └── performance_monitor.dart # Performans izleme
```

### 🔧 **Backend (.NET Web API)**
```
backend_api/
├── Controllers/                 # API Controller'ları
│   ├── AuthController.cs       # Kimlik doğrulama
│   ├── PhotoController.cs      # Fotoğraf işlemleri
│   ├── UserController.cs       # Kullanıcı yönetimi
│   ├── StoreController.cs      # Market bilgileri
│   ├── AnnouncementController.cs # Duyuru sistemi
│   └── DepartmentController.cs # Departman yönetimi
├── Models/                      # Veri modelleri
│   ├── User.cs                 # Kullanıcı modeli
│   ├── Photo.cs                # Fotoğraf modeli
│   ├── Store.cs                # Market modeli
│   ├── Department.cs           # Departman modeli
│   └── Announcement.cs         # Duyuru modeli
├── Services/                    # İş mantığı servisleri
│   ├── AuthService.cs          # Kimlik doğrulama servisi
│   ├── UserService.cs          # Kullanıcı servisi
│   ├── JwtService.cs           # JWT token servisi
│   └── JwtValidatorService.cs  # JWT doğrulama servisi
├── Repositories/                # Veri erişim katmanı
│   ├── Interfaces/             # Repository interface'leri
│   │   ├── IUserRepository.cs  # Kullanıcı repository interface
│   │   └── IPhotoRepository.cs # Fotoğraf repository interface
│   └── UserRepository.cs       # Kullanıcı repository implementasyonu
├── DTOs/                        # Data Transfer Objects
│   ├── Requests/               # Request DTO'ları
│   │   ├── LoginRequest.cs     # Giriş request'i
│   │   ├── RegisterRequest.cs  # Kayıt request'i
│   │   └── CreateEmployeeRequest.cs # Çalışan oluşturma request'i
│   └── Responses/              # Response DTO'ları
│       └── ApiResponse.cs      # API response wrapper
├── Data/                        # Veritabanı context
│   └── ApplicationDbContext.cs # Entity Framework context
├── Infrastructure/              # Altyapı servisleri
│   ├── Extensions/             # Extension metodlar
│   └── Middleware/             # Middleware'ler
└── Program.cs                   # Uygulama girişi
```

## 🚀 **Kurulum ve Çalıştırma**

### 📋 **Gereksinimler**
- Flutter SDK (3.16.0+)
- .NET 9.0 SDK
- SQLite (Gömülü veritabanı)
- Visual Studio Code / Visual Studio
- Chrome (Web test için)

### 🔧 **Backend Kurulumu**
```bash
cd backend_api
dotnet restore
dotnet run
```
Backend API `http://localhost:5002` adresinde çalışacak.

### 📱 **Frontend Kurulumu**
```bash
cd mobile_app
flutter pub get
flutter run -d chrome  # Web için
flutter run             # Mobil için
```

## 🔐 **Kimlik Doğrulama**

### 👤 **Kullanıcı Girişi**
- JWT token tabanlı kimlik doğrulama
- Rol tabanlı erişim kontrolü (RBAC)
- Otomatik token yenileme
- BCrypt ile şifre hashleme

### 🔑 **API Güvenliği**
- JWT Bearer token authentication
- Token tabanlı yetkilendirme
- CORS policy yapılandırması
- Exception handling middleware

## 📊 **Veri Akışı**

### 📸 **Fotoğraf İşleme**
1. Çalışan fotoğraf çeker (kamera/galeri)
2. Fotoğraf backend'e yüklenir (`/api/photo`)
3. Fotoğraf `wwwroot/uploads` klasörüne kaydedilir
4. Veritabanına metadata kaydedilir
5. Çalışan sonuçları görüntüler

### 👥 **Çalışan Yönetimi**
1. Manager çalışan oluşturur (`/api/auth/create-employee`)
2. Çalışan bilgileri veritabanına kaydedilir
3. Çalışan listesi güncellenir
4. Dashboard istatistikleri yenilenir

### 📢 **Duyuru Sistemi**
1. Yönetici duyuru oluşturur
2. Duyuru veritabanına kaydedilir
3. Çalışanlar bildirim alır
4. Duyuru çalışan ekranında görüntülenir

### 🏪 **Market Bilgileri**
1. Yönetici market bilgilerini günceller
2. Değişiklikler veritabanına kaydedilir
3. Çalışanlar güncel bilgileri görür
4. Çalışanlar bilgileri değiştiremez (sadece görüntüleme)

## 🎨 **UI/UX Özellikleri**

### 🎯 **Modern Tasarım**
- Material Design 3
- Responsive layout
- Custom color palette
- Accessibility uyumlu

### 📱 **Kullanıcı Deneyimi**
- Intuitive navigation
- Smooth animations
- Real-time updates
- Error handling with SnackBar
- Loading indicators

## 🔧 **Geliştirme**

### 📝 **Kod Kalitesi**
- Clean Architecture
- SOLID principles
- Dependency Injection
- Repository Pattern
- Unit of Work Pattern

### 🚀 **Performance**
- Lazy loading
- Image optimization
- Caching strategies
- Memory management
- Async/await operations

## 📈 **API Endpoints**

### 🔐 **Authentication**
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Manager kaydı
- `POST /api/auth/create-employee` - Çalışan oluşturma

### 📸 **Photo Management**
- `GET /api/photo` - Kullanıcı fotoğrafları
- `POST /api/photo` - Fotoğraf yükleme
- `GET /api/photo/{id}` - Tek fotoğraf
- `DELETE /api/photo/{id}` - Fotoğraf silme
- `GET /api/photo/all-employees` - Tüm çalışan fotoğrafları

### 👥 **User Management**
- `GET /api/user/profile` - Kullanıcı profili
- `PUT /api/user/profile` - Profil güncelleme
- `GET /api/user/employees` - Çalışan listesi
- `DELETE /api/user/employees/{id}` - Çalışan silme

### 🏪 **Store Management**
- `GET /api/store/info` - Market bilgileri
- `PUT /api/store/info` - Market bilgilerini güncelle

### 📢 **Announcements**
- `GET /api/announcements` - Duyuru listesi
- `POST /api/announcements` - Duyuru oluştur
- `PUT /api/announcements/{id}` - Duyuru güncelle
- `DELETE /api/announcements/{id}` - Duyuru sil

### 🏢 **Departments**
- `GET /api/departments` - Departman listesi
- `POST /api/departments` - Departman oluştur
- `DELETE /api/departments/{id}` - Departman sil

## 🗄️ **Veritabanı Şeması**

### 👤 **Users Tablosu**
```sql
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    Role TEXT NOT NULL, -- "Manager" veya "Employee"
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    StoreName TEXT NOT NULL,
    StoreAddress TEXT,
    StorePhone TEXT,
    DepartmentName TEXT,
    ProfilePhotoUrl TEXT,
    CreatedAt DATETIME NOT NULL,
    LastLoginAt DATETIME,
    IsActive BOOLEAN NOT NULL DEFAULT 1
);
```

### 📸 **Photos Tablosu**
```sql
CREATE TABLE Photos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FileName TEXT NOT NULL,
    FilePath TEXT NOT NULL,
    Description TEXT,
    CreatedAt DATETIME NOT NULL,
    UserId INTEGER NOT NULL,
    DepartmentName TEXT,
    Score REAL,
    AnalysisResult TEXT,
    IsProcessed BOOLEAN NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

### 🏪 **Stores Tablosu**
```sql
CREATE TABLE Stores (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Address TEXT,
    Phone TEXT,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME
);
```

### 🏢 **Departments Tablosu**
```sql
CREATE TABLE Departments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Description TEXT,
    CreatedAt DATETIME NOT NULL
);
```

### 📢 **Announcements Tablosu**
```sql
CREATE TABLE Announcements (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Content TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT 1
);
```

## 🐛 **Hata Yönetimi**

### 🔧 **Backend Hata Yönetimi**
- Global exception middleware
- Custom exception sınıfları
- Structured logging
- HTTP status code'ları
- Kullanıcı dostu hata mesajları

### 📱 **Frontend Hata Yönetimi**
- Try-catch blokları
- SnackBar ile hata gösterimi
- Retry mekanizması
- Network hata yönetimi
- Validation hataları

## 📊 **Monitoring ve Analytics**

### 📈 **Performans Metrikleri**
- API response times
- App startup time
- Memory usage
- Database query performance

### 🐛 **Error Tracking**
- Exception logging
- Error reporting
- Performance monitoring
- User feedback

## 🔮 **Gelecek Planları**

### 🚀 **Yakın Vadeli (1-2 ay)**
- [ ] **AI/ML Entegrasyonu**: TensorFlow tabanlı raf analizi
- [ ] **Gelişmiş Görüntü İşleme**: OpenCV ile fotoğraf ön işleme
- [ ] **Derin Öğrenme**: CNN modelleri ile 1-10 puan değerlendirme
- [ ] **Real-time Analytics**: Anlık performans takibi
- [ ] **Push Notifications**: Bildirim sistemi

### 🌟 **Orta Vadeli (3-6 ay)**
- [ ] **Object Detection**: Ürün tespiti ve kategorilendirme
- [ ] **Semantic Segmentation**: Raf bölümlerini otomatik ayırma
- [ ] **Predictive Analytics**: Trend analizi ve tahminleme
- [ ] **Multi-language Support**: Çoklu dil desteği
- [ ] **Offline Mode**: İnternet olmadan çalışma

### 🚀 **Uzun Vadeli (6-12 ay)**
- [ ] **Advanced AI**: GPT entegrasyonu ile akıllı öneriler
- [ ] **Computer Vision**: Gelişmiş görüntü analizi
- [ ] **Real-time Collaboration**: Ekip çalışması özellikleri
- [ ] **Mobile Apps**: iOS/Android native uygulamalar
- [ ] **Cloud Deployment**: AWS/Azure entegrasyonu
- [ ] **Enterprise Features**: Büyük şirketler için özellikler

### 🤖 **AI/ML Roadmap**
- [ ] **Phase 1**: Transfer Learning ile hızlı başlangıç
- [ ] **Phase 2**: Custom CNN mimarisi geliştirme
- [ ] **Phase 3**: Production-ready ML pipeline
- [ ] **Phase 4**: Gelişmiş AI özellikleri

## 🤝 **Katkıda Bulunma**

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📄 **Lisans**

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## 📞 **İletişim**

- **Proje Sahibi**: [Esma Zeynep Uysal]
- **Email**: [esuysll1@gmail.com]
- **Proje Linki**: [https://github.com/Esmazuysal/ShelfScore]


## 🙏 **Teşekkürler**

- Flutter team
- .NET team
- Open source community
- Beta testers

---

**⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!**