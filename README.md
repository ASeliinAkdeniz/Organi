<img width="1904" height="887" alt="Ekran görüntüsü 2026-06-11 172801" src="https://github.com/user-attachments/assets/e8cb9b96-cae5-451e-86a8-080827b1f5a1" />
🌾 Organi - Premium Organic E-Commerce Platform

Organi, organik tarım ürünlerini çiftlikten doğrudan tüketiciye ulaştıran, kurumsal yazılım mimarisi standartlarında ve tasarım desenleri (Design Patterns) odağında geliştirilmiş bir **ASP.NET Core MVC** e-ticaret web uygulamasıdır.

---

## 🚀 Öne Çıkan Özellikler

### 👤 Kullanıcı Arayüzü (UI)
* **Dinamik Vitrin:** 3 slaytlı otomatik geçişli Hero Banner ve öne çıkan ürünler listesi.
* **Gelişmiş Mağaza:** Kategori, fiyat aralığı ve popülariteye göre anlık filtreleme, sıralama ve sayfalama.
* **Ürün Karşılaştırma:** En fazla 4 ürünü fiyat, besin değerleri, stok ve puanlama bazında yan yana kıyaslama tablosu.
* **Akıllı Sepet & Checkout:** Sunucu tabanlı (Session) sepet yönetimi ve dinamik kargo ücreti hesaplama sistemi.
* **İletişim Portalı:** Veritabanı entegreli mesaj gönderme formu.

### 🔐 Admin Paneli (`/Admin`)
* **Merkezi Dashboard:** Toplam ürün, sipariş, okunmamış mesaj ve kritik stok sayılarını gösteren özet kartları ile son işlemler (Audit Log) akışı.
* **Gelişmiş CRUD Yönetimi:** Ürün ekleme/düzenleme/silme, sipariş takibi ve gelen mesajları okundu olarak işaretleme panelleri.
* **Sistem Günlükleri (Logs):** Mağazadaki tüm kritik hareketleri geriye dönük listeleyen izleme ekranı.

---

## 🛠️ Kullanılan Teknolojiler

* **Backend:** ASP.NET Core 8.0 MVC & C#
* **Veritabanı & ORM:** Microsoft SQL Server & Entity Framework Core 8.0 (Code-First)
* **Frontend:** Razor View Engine, Saf HTML5/CSS3 & Vanilla JavaScript 
* **Oturum Yönetimi:** Server-Side Session State (Sepet ve Karşılaştırma Listesi için)

---

## 💎 Uygulanan Tasarım Desenleri (Design Patterns)

Projenin sürdürülebilir, esnek ve kurumsal standartlarda olması için **5 adet GoF Tasarım Deseni** entegre edilmiştir:

| Desen | Projedeki Görevi | Sağladığı Avantaj |
| :--- | :--- | :--- |
| **Unit of Work** | Tüm veritabanı operasyonlarını tek bir çatı altında toplar. | İşlemlerin atomik (`Transaction`) olarak ya tamamen kaydedilmesini ya da tamamen geri alınmasını sağlar. |
| **Observer** | Ürün değişikliklerini (`Create/Update/Delete`) dinleyicilere yayınlar. | `AdminNotificationObserver` anlık bildirim üretirken, `AuditLogObserver` eşzamanlı olarak log dosyasına yazar. |
| **Chain of Responsibility** | Ürün kayıt formunu sıralı bir validasyon zincirinden (İsim ➔ Fiyat ➔ Stok ➔ Görsel) geçirir. | Kodun `if-else` cehennemine dönmesini engeller; yeni kurallar yeni halkalar olarak esnekçe eklenir. |
| **Factory Method** | Controller'ın doğrudan `new Product()` üretmesini engeller. | `ProductFactoryResolver` üzerinden form verilerini (`ViewModel`) otomatik olarak güvenli domain entity nesnelerine dönüştürür. |
| **Strategy** | Sepet toplam tutarına göre kargo ücreti hesaplama motorunu dinamik olarak seçer. | 99₺ altında `StandardShippingStrategy` (9.99₺), üstünde ise `FreeShippingStrategy` (0₺) otomatik devreye girer. |




## Proje Görselleri



<img width="1888" height="943" alt="Ekran görüntüsü 2026-06-11 170356" src="https://github.com/user-attachments/assets/048d3ac3-23e9-462c-aab9-7da1f4531275" />

<img width="1903" height="929" alt="Ekran görüntüsü 2026-06-11 170414" src="https://github.com/user-attachments/assets/03df70e4-acdf-43e6-a01f-7e7cb941c299" />

<img width="1881" height="923" alt="Ekran görüntüsü 2026-06-11 170450" src="https://github.com/user-attachments/assets/f3aa6803-530d-4c62-bb48-ce27433c9d30" />

<img width="1869" height="663" alt="Ekran görüntüsü 2026-06-11 170544" src="https://github.com/user-attachments/assets/16982fcc-34d9-4e7e-93f0-7b1f8b879917" />

<img width="1894" height="871" alt="Ekran görüntüsü 2026-06-11 170601" src="https://github.com/user-attachments/assets/9b562c49-0fe4-42c4-afec-b4c28b52b034" />

<img width="1907" height="901" alt="Ekran görüntüsü 2026-06-11 170614" src="https://github.com/user-attachments/assets/bb370612-b8c5-45a1-a391-8f18de46664c" />

<img width="1899" height="906" alt="Ekran görüntüsü 2026-06-11 170651" src="https://github.com/user-attachments/assets/77b5f45b-cd83-48d2-b334-523dc908ceca" />

<img width="1877" height="938" alt="Ekran görüntüsü 2026-06-11 170702" src="https://github.com/user-attachments/assets/2319ac27-b11b-4861-8a50-ff60a7d7e5e9" />

<img width="1906" height="839" alt="Ekran görüntüsü 2026-06-11 170816" src="https://github.com/user-attachments/assets/ca3b8da5-a32f-471c-87bb-9e6ae034c234" />

<img width="1892" height="734" alt="Ekran görüntüsü 2026-06-11 170826" src="https://github.com/user-attachments/assets/c37e4db4-9371-48aa-a1f2-4349f9a1cfc1" />

<img width="1909" height="635" alt="Ekran görüntüsü 2026-06-11 170840" src="https://github.com/user-attachments/assets/9665ae51-80a2-45a2-bf7c-d49cf9d7c55c" />

<img width="1904" height="887" alt="Ekran görüntüsü 2026-06-11 172801" src="https://github.com/user-attachments/assets/f3b83edf-f6e8-486b-836e-c1c03d3ff2e3" />










