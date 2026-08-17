# 🏥 Clinic Reservation Web

A web-based clinic management and appointment scheduling system built with **ASP.NET Core MVC** and **C#**.

## What It Does

- 👤 Manage patients and user accounts
- 📅 Create individual and group appointment schedules
- 📝 Register and manage reservation records
- 🔐 Login with OTP verification and SMS support
- 🔎 Filter and paginate patients and reservations
- 🖼️ Handle image/file processing
- 🇮🇷 RTL Persian interface

## Structure

```text
Clinic.Mvc
    ↓
Clinic.Application
    ↓
Clinic.Data
```

- **Clinic.Mvc** — Web UI and controllers
- **Clinic.Application** — DTOs and application services
- **Clinic.Data** — Entity Framework Core and SQL Server data access

## Tech Stack

- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core 9
- SQL Server
- Cookie Authentication & Session
- SMS integration
- ImageSharp
- Google reCAPTCHA

## Run

```bash
git clone https://github.com/Mohammad-Amin-Nazeri/Clinic_Reservation_Web.git
cd Clinic_Reservation_Web
```

Open `Clinic.sln` in Visual Studio, configure the SQL Server connection in `appsettings.json`, and run `Clinic.Mvc`.

> ⚠️ Use your own database credentials and secrets for local or production environments.

## 👨‍💻 Author

**Mohammad Amin Nazeri**
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/mohammad-amin-nazeri)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Mohammad-Amin-Nazeri)
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E2?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/Aminn02)
[![Instagram](https://img.shields.io/badge/Instagram-E4405F?style=for-the-badge&logo=instagram&logoColor=white)](https://www.instagram.com/mohammad_amin_nazeri/)

