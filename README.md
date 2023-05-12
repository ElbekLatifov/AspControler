[# Avtotest](https://avotest.bsite.net/)

Xulosa:
  -Projectda dastlab jsonga, so'ng databazaga saqlash structurasi qilindi
  {
    Databases(SqlServer, Sqllite, Postgres va hkz). Og'ir dasturlarni o'rnatib yoki internetdan databaza server yaratish ehtiyojidan ko'ra qulay, tekshirish uchun oson bo'lgan Sqlite dan                   foydalanildi.
  }
  
  -Savollar 3 xil tilda foydalanuvchi xizmatida bo'ladi.
  
  -Dependense injection texnologiyasi qo'llanildi
  {
    -transparient
    {
      har bir so'rov uchun alohida obyekt olinadi
    }
    -scoped
    {
      dastur ishlashi davomida faqat bir marta obyekt olinadi
    }
    -singleton
    {
      har bir obyekt so'ralgan joy(controller, services) uchun faqat bir marta obyekt olinadi. Forexample, UserController uchun bitta, UserService uchun bitta va hkz.
    }
  }
