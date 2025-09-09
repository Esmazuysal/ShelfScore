# 🤖 ML Models - Raf Fotoğraf Analizi

## 📋 **Gelecek Geliştirmeler**

Bu klasör, **raf fotoğraf analizi** için **TensorFlow** tabanlı **derin öğrenme** modellerini içerecek.

> **Not**: Bu modül şu anda **placeholder** durumunda. **TensorFlow** implementasyonu **gelecek sürümlerde** eklenecek.

### 🎯 **Hedeflenen Özellikler**

#### **A. Görüntü İşleme (Computer Vision)**
- **OpenCV** ile fotoğraf ön işleme
- **PIL/Pillow** ile format dönüştürme
- **NumPy** ile matris işlemleri
- **Image Augmentation** (döndürme, parlaklık, kontrast)


#### **B. Derin Öğrenme (Deep Learning)**
- **TensorFlow/Keras** ile CNN modeli
- **Transfer Learning** (VGG16, ResNet50, EfficientNet)
- **Custom CNN** mimarisi
- **Real-time inference** (1-2 saniye)

#### **C. Analiz Kriterleri (1-10 Puan)**
- **Düzen** (30% ağırlık): Ürün sıralaması, simetri
- **Temizlik** (25% ağırlık): Hijyen durumu, toz/kir
- **Ürün Yerleşimi** (25% ağırlık): Kategori ayrımı, fiyat etiketleri
- **Genel Görünüm** (20% ağırlık): Estetik, profesyonellik

### 🚀 **Geliştirme Roadmap**

#### **Phase 1: Temel Altyapı (1-2 hafta)**
- [ ] Python environment kurulumu
- [ ] Veri toplama stratejisi
- [ ] Temel CNN modeli
- [ ] Transfer Learning implementasyonu

#### **Phase 2: Model Geliştirme (2-3 hafta)**
- [ ] Custom CNN mimarisi
- [ ] Veri augmentation
- [ ] Model eğitimi ve optimizasyon
- [ ] Performans değerlendirme

#### **Phase 3: Production (1-2 hafta)**
- [ ] Backend entegrasyonu
- [ ] Real-time inference
- [ ] API optimizasyonu
- [ ] Monitoring ve logging

### 📊 **Teknik Detaylar**

#### **Model Mimarisi:**
```python
# CNN Model (1-10 puan sınıflandırması)
Input (224x224x3)
→ Conv2D + BatchNorm + ReLU + MaxPool
→ Conv2D + BatchNorm + ReLU + MaxPool  
→ Conv2D + BatchNorm + ReLU + MaxPool
→ GlobalAveragePooling
→ Dense + Dropout
→ Output (10 sınıf: 1-10 puan)
```

#### **Veri Gereksinimleri:**
- **Minimum**: 1,000 fotoğraf (100 per sınıf)
- **Optimal**: 10,000+ fotoğraf (1,000 per sınıf)
- **Format**: JPG/PNG, 224x224 piksel
- **Etiketleme**: Manuel (1-10 puan)

#### **Performans Hedefleri:**
- **Doğruluk**: %90+ (validation set)
- **Inference Süresi**: <2 saniye
- **Model Boyutu**: <100MB
- **Memory Kullanımı**: <1GB

### 🤝 **Katkıda Bulunma**

1. **Veri Toplama**: Raf fotoğrafları paylaşın
2. **Model Eğitimi**: Farklı mimariler deneyin
3. **Optimizasyon**: Performans iyileştirmeleri
4. **Dokümantasyon**: Kullanım kılavuzları

---

**İletişim**: [GitHub Issues](https://github.com/username/raf/issues) üzerinden geri bildirim verebilirsiniz.
