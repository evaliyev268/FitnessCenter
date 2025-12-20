# 🏋️‍♂️ Fitness Center Yönetim Sistemi ve AI Asistan Entegrasyonu

> **Web Programlama Dersi Dönem Projesi** > Bu proje, modern bir spor salonunun yönetim süreçlerini dijitalleştirmek, randevu sistemini otomatize etmek ve yapay zeka destekli kişisel analizler sunmak amacıyla geliştirilmiştir.

---

## 📋 Proje Hakkında

**Fitness Center**, kullanıcıların spor salonu hizmetlerini inceleyebileceği, eğitmenlerden randevu alabileceği ve **Yapay Zeka (ElBot)** desteği ile kişisel vücut analizi yaptırabileceği kapsamlı bir web uygulamasıdır.

Proje **ASP.NET Core 8.0 MVC** mimarisi kullanılarak geliştirilmiş olup, **Code-First** yaklaşımı ile veritabanı yönetimi sağlanmıştır. Ayrıca mobil uyumluluk ve dış servis entegrasyonu için **REST API** uçları (endpoints) içermektedir.

### 🔗 Canlı Demo / Video
*(Eğer projenin videosunu çektiyseniz buraya linkini ekleyebilirsiniz, yoksa bu satırı silin)*

---

## 🚀 Öne Çıkan Özellikler

### 🤖 1. ElBot - AI Fitness Asistanı (Yapay Zeka)
Projenin en yenilikçi özelliğidir.
- **Vücut Analizi:** Kullanıcının boy, kilo ve cinsiyet verilerini alarak BMI (Vücut Kitle İndeksi) hesaplar.
- **Kişisel Program:** BMI sonucuna göre (Zayıf, İdeal, Kilolu) kişiye özel antrenman ve beslenme programı yazar.
- **Görsel Üretimi (Image Gen):** Kullanıcının verilerine uygun "Hedef Fizik" görselini **Pollinations AI** kullanarak sıfırdan çizer.
- **Güvenlik Filtresi:** AI promptlarına eklenen özel filtreler sayesinde her zaman uygun ve profesyonel sporcu görselleri üretilir.

### 📅 2. Gelişmiş Randevu Sistemi
- **Çakışma Kontrolü (Conflict Detection):** Bir eğitmen, dolu olduğu saat aralığında (Örn: 14:00 - 15:00) başka bir randevu alamaz. Sistem bunu otomatik engeller.
- **Durum Yönetimi:** Randevular "Bekliyor", "Onaylandı" veya "Reddedildi" durumlarına sahiptir.

### 🔐 3. Yönetici (Admin) Paneli
- **Dashboard:** Özet veriler.
- **Eğitmen Yönetimi:** Yeni eğitmen ekleme, silme, düzenleme.
- **Randevu Onayı:** Gelen talepleri tek tıkla onaylama veya reddetme.
- **Hizmet Yönetimi:** Fiyat ve süre güncelleme.

### 🌐 4. REST API Desteği
- Mobil uygulamalar veya 3. parti yazılımlar için eğitmen verilerini JSON formatında dışa açan API uçları (`/api/trainers`).

---

## 🛠 Kullanılan Teknolojiler

| Kategori | Teknoloji |
|----------|-----------|
| **Backend** | ASP.NET Core 8.0, C# |
| **Veritabanı** | MSSQL, Entity Framework Core (Code-First) |
| **Frontend** | HTML5, CSS3, Bootstrap 5, JavaScript (jQuery/AJAX) |
| **AI Servisi** | Pollinations.ai (Ücretsiz Image Generation API) |
| **Geliştirme Ortamı** | Visual Studio 2022 |

---

## ⚙️ Kurulum ve Çalıştırma

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

1. **Repoyu Klonlayın:**
   ```bash
   git clone [https://github.com/evaliyev268/FitnessCenter.git](https://github.com/evaliyev268/FitnessCenter.git)
