<div align="center">

# Clinic Reservation Web

**A clinic management and appointment platform covering patient records, reservation scheduling, authentication, OTP workflows, and administrative operations.**

[![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/mvc/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Data-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)

<a href="#english"><strong>🇬🇧 English</strong></a> &nbsp;•&nbsp; <a href="#فارسی"><strong>🇮🇷 فارسی</strong></a>

</div>

---

<a id="english"></a>

# 🇬🇧 English

## Overview

`Clinic_Reservation_Web` is a clinic-oriented web application built around patient management and appointment scheduling. The solution separates the MVC presentation layer, application services, and data access into three dedicated projects.

The domain goes beyond a simple appointment form. The application contains patient records, reservation records, user authentication, OTP verification, filtering and paging, file/image handling, and administrative workflows.

## Architecture

```text
┌──────────────────────────────┐
│          Clinic.Mvc          │
│  Web UI • Areas • Controllers│
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│      Clinic.Application      │
│ DTOs • Services • Validation │
│ Auth • Reservations • Paging │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         Clinic.Data          │
│ EF Core • SQL Server • Data  │
└──────────────────────────────┘
```

The solution structure explicitly contains `Clinic.Mvc`, `Clinic.Application`, and `Clinic.Data`, keeping web concerns away from application services and persistence.

## Core Domain

### Patient Management

The application contains dedicated contracts for creating, editing, filtering, grouping, and viewing patient records. Filtering supports patient attributes such as name, mobile number, national identifier, age, gender, and ordering.

### Reservation Management

Reservation contracts cover creation, grouped reservations, filtering, reservation records, and reservation time information. The model separates the reservation itself from the records associated with patients and scheduled times.

### Authentication & OTP

The application contains user authentication DTOs and an OTP-oriented workflow, including OTP resend support. This indicates a mobile-first authentication flow rather than relying only on a traditional username/password form.

### Pagination

Reusable paging DTOs and extensions are present in the Application layer for consistent list and filtering workflows.

### File & Image Handling

The Application layer includes upload-related extensions, while the project dependencies include image processing and file-oriented utilities.

## Application Structure

```text
Clinic_Reservation_Web/
├── Clinic.Mvc/
│   └── Web presentation and administrative UI
│
├── Clinic.Application/
│   ├── DTOs/
│   │   ├── Patients/
│   │   ├── Reservations/
│   │   ├── ReserveRecords/
│   │   ├── Users/
│   │   └── Paging/
│   ├── Extensions/
│   └── Application services
│
├── Clinic.Data/
│   └── Persistence and entities
│
└── Clinic.sln
```

## Technology Stack

- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQL Server
- Newtonsoft.Json
- SMS integration through `IPE.SmsIr`
- Image processing with ImageSharp
- File/image resizing utilities
- Google reCAPTCHA integration
- Bootstrap datetime picker

## Engineering Value

The strongest aspect of the project is the business workflow it models. Patient information, reservation scheduling, reservation records, authentication, OTP verification, filtering, and paging are represented as separate application concerns instead of being collapsed into a single controller-centric design.

The three-project separation also gives the system a clear direction of dependency:

```text
Web → Application → Data
```

That makes the codebase easier to evolve than a single-project MVC application where UI, business logic, and persistence are mixed together.

## Author

**Mohammad Amin Nazeri**

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/mohammad-amin-nazeri)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Mohammad-Amin-Nazeri)
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E2?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/Aminn02)
[![Instagram](https://img.shields.io/badge/Instagram-E4405F?style=for-the-badge&logo=instagram&logoColor=white)](https://www.instagram.com/mohammad_amin_nazeri/)

---

<a id="فارسی"></a>

# 🇮🇷 فارسی

## معرفی

`Clinic_Reservation_Web` یک سامانه وب مدیریت کلینیک و نوبت‌دهی است که روی مدیریت بیماران و زمان‌بندی Reservation تمرکز دارد. Solution پروژه Presentation، Application و Data Access را در سه پروژه مستقل نگه می‌دارد.

Domain پروژه فقط به یک فرم ساده نوبت‌دهی محدود نیست و شامل پرونده بیمار، Reservation، Recordهای نوبت، احراز هویت، OTP، فیلتر و Pagination و عملیات مدیریتی است.

## معماری

```text
┌──────────────────────────────┐
│          Clinic.Mvc          │
│ UI • Areas • Controllers     │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│      Clinic.Application      │
│ DTO • Service • Validation   │
│ Auth • Reservation • Paging  │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         Clinic.Data          │
│ EF Core • SQL Server         │
└──────────────────────────────┘
```

Solution به‌صورت صریح شامل `Clinic.Mvc`، `Clinic.Application` و `Clinic.Data` است و Presentation را از Application و Persistence جدا می‌کند.

## هسته Domain

### مدیریت بیماران

DTOهای اختصاصی برای ایجاد، ویرایش، فیلتر، گروه‌بندی و مشاهده اطلاعات بیماران وجود دارد. فیلتر بیماران بر اساس نام، موبایل، کد ملی، سن، جنسیت و ترتیب نمایش پشتیبانی می‌شود.

### مدیریت نوبت

برای ایجاد Reservation، Reservationهای گروهی، فیلتر، Recordهای رزرو و اطلاعات زمان نوبت DTOهای جداگانه تعریف شده‌اند. این جداسازی باعث می‌شود خود Reservation از اطلاعات Record و زمان‌بندی قابل تفکیک باشد.

### احراز هویت و OTP

پروژه دارای قراردادهای Authentication و Workflow مرتبط با OTP و Resend OTP است و در نتیجه Authentication آن به یک Login ساده محدود نیست.

### Pagination

DTO و Extensionهای Paging در Application قرار گرفته‌اند تا فهرست‌ها و Filterها رفتار یکپارچه‌ای داشته باشند.

### فایل و تصویر

Extensionهای مرتبط با Upload در Application وجود دارند و پروژه از کتابخانه‌های پردازش و Resize تصویر نیز استفاده می‌کند.

## ساختار پروژه

```text
Clinic_Reservation_Web/
├── Clinic.Mvc/
│   └── Presentation و UI مدیریتی
│
├── Clinic.Application/
│   ├── DTOs/
│   │   ├── Patients/
│   │   ├── Reservations/
│   │   ├── ReserveRecords/
│   │   ├── Users/
│   │   └── Paging/
│   ├── Extensions/
│   └── Application Services
│
├── Clinic.Data/
│   └── Persistence و Entityها
│
└── Clinic.sln
```

## تکنولوژی‌ها

- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQL Server
- Newtonsoft.Json
- `IPE.SmsIr` برای SMS
- ImageSharp برای پردازش تصویر
- ابزارهای Resize فایل و تصویر
- Google reCAPTCHA
- Bootstrap DateTime Picker

## ارزش فنی پروژه

نقطه قوت این پروژه در Domain Workflow آن است. Patient، Reservation، Reservation Record، Authentication، OTP، Filtering و Paging به‌عنوان دغدغه‌های جداگانه در Application مدل شده‌اند و همه چیز در یک Controller بزرگ فرو نرفته است.

همچنین مرز Dependency پروژه واضح است:

```text
Web → Application → Data
```

این ساختار نسبت به یک MVC تک‌پروژه‌ای که UI، Business Logic و Persistence را با هم ترکیب می‌کند، امکان توسعه و نگهداری بهتری فراهم می‌کند.

## توسعه‌دهنده

**Mohammad Amin Nazeri**

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/mohammad-amin-nazeri)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Mohammad-Amin-Nazeri)
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E2?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/Aminn02)
[![Instagram](https://img.shields.io/badge/Instagram-E4405F?style=for-the-badge&logo=instagram&logoColor=white)](https://www.instagram.com/mohammad_amin_nazeri/)
