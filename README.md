# 🧾 Stok Takip Otomasyonu

Bu proje, C# ve Windows Forms kullanılarak geliştirilmiş bir **stok takip otomasyon sistemidir**. Küçük ve orta ölçekli işletmelerin ürün, müşteri ve satış takibini kolaylaştırmak amacıyla hazırlanmıştır.


## 🚀 Özellikler

- 🟢 Ürün ekleme, güncelleme ve silme
- 🟢 Stok miktarı kontrolü 
- 🟢 Müşteri kayıt sistemi
- 🟢 Sepete ürün ekleyerek satış yapma
- 🟢 Satış sonrası stoktan otomatik düşme
- 🟢 Genel toplam ve satış tutarı hesaplama
- 🟢 Kullanıcı girişi ile yetkilendirme 

---
📘 Kullanıcı Kılavuzu
🔐 Giriş ve Kayıt Olma
-Uygulama, giriş ekranı ile başlar.
-Kullanıcı adı ve şifre ile sisteme giriş yapılır.
-“Üye Ol” butonu ile yeni kullanıcı kaydı oluşturulabilir.
-Yeni kayıt olan kullanıcılar varsayılan olarak personel yetkisiyle sisteme tanımlanır.
-Yönetici yetkisi (admin) verilecekse, kullanıcı veritabanına admin olarak eklenmelidir.

🏠 Ana Ekran Özellikleri
--Müşteri İşlemleri
Müşteri ekleme, güncelleme ve silme işlemleri yapılabilir.

--Ürün İşlemleri
Ürün ekleme, güncelleme ve silme işlemleri yapılabilir.

--Stok İşlemleri
Ürün stokları güncellenebilir.
Stok adedi 5'in altına düşen ürünler, otomatik olarak "Azalan Stoklar" ekranında listelenir.

--Satış İşlemleri
Sepete ürün eklenerek satış yapılabilir.
Satış yapıldığında stok miktarı otomatik olarak güncellenir.
Satış işlemleri geçmişte kayıt altına alınır.

--Uygulama Açılış Uyarısı

Uygulama ilk açıldığında, kullanıcıya stok durumu hakkında uyarı mesajı gösterilir (örneğin: “Azalan stoklarınız mevcut!”).

⚙️ Kullanıcı Ayarları
--Yöneticiler:
Yeni kullanıcı ekleyebilir.
Kullanıcı şifresi güncelleyebilir.
Yetkileri değiştirebilir.

--Personeller:
Kendi şifresini güncelleyebilir.

📉 Azalan Stoklar
Stok seviyesi 5’in altına düşen ürünler, sistem tarafından takip edilir.
“Azalan Stoklar” ekranında listelenerek stok yenileme ihtiyacı hakkında kullanıcı bilgilendirilir.

## 🛠️ Kullanılan Teknolojiler

| Teknoloji     | Açıklama                        |
|---------------|----------------------------------|
| C#            | Programlama dili                 |
| .NET Framework| Windows Forms arayüzü için       |
| Windows Forms | Masaüstü kullanıcı arayüzü       |
| SQL Server    | Veritabanı yönetimi              |
| ADO.NET       | Veritabanı bağlantısı için       |

---

## 💻 Kurulum

1. Visual Studio ile projeyi açın
2. `App.config` içindeki bağlantı cümlesini kendi SQL Server bilgilerinize göre düzenleyin
3. Veritabanı dosyasını yükleyin (`.bak` ya da `.bacpac` olabilir)
4. Projeyi çalıştırın

---

## 🗃️ Veritabanı Yapısı

- `Urun` tablosu → Ürün bilgileri ve stok miktarını gösterir.
- `Musteriekle` tablosu → Müşteri bilgilerini gösterir.
- `Sepet` tablosu → Geçici satış verilerini gösterir.
- `Satıs` tablosu → Tamamlanan satış kayıtlarını gösterir.
- `Kullanicilar` tablosu → Kullanıcı bilgilerini gösterir.
-  `Kategori` tablosu → Eklenen kategori bilgilerini gösterir.


## 🧑‍💻 Geliştirici

**Esra Kap Kurt**  




