# BẢNG CHỨNG MINH EUREKA SERVER & PRICINGSERVICE

**Sinh viên:** Bùi Thanh Vân - 2324801030076  
**Môn:** BTH5 - Case study Công ty bảo hiểm  
**Ngày:** 18/10/2026  
**GitHub:** https://github.com/buithanhvann/InsuranceAgency_2324801030076

---

## 1. Eureka Server chạy trên Docker

```powershell
PS> docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
NAMES          STATUS              PORTS
eureka-server  Up 2 hours          0.0.0.0:8761->8761/tcp
```

![Docker Container](eureka_docker_ps.png)

---

## 2. Eureka Dashboard truy cập được (HTTP 200)

**URL:** http://localhost:8761  
**Status Code:** 200 OK  

Trang Eureka Dashboard hiển thị thông tin hệ thống:
- Environment: test
- Data center: default
- Current time: 2026-10-18T07:12:30 +0000
- Uptime: 01:06

---

## 3. PricingService đã đăng ký thành công với Eureka

**Service:** PRICINGSERVICE  
**Status:** UP  
**Instance:** PricingService:${random.value}  
**Home Page:** http://localhost:5040/info  

### Dashboard hiển thị:
```
Instances currently registered with Eureka:
| Application      | AMIs | Availability Zones | Status |
|------------------|------|-------------------|--------|
| PRICINGSERVICE   | n/a  | (1)               | UP (1) |
```

### Link instance:
- Home Page: http://localhost:5040/info
- Health Check: http://localhost:5040/health

---

## 4. Cấu hình Steeltoe Eureka Client

### Package đã cài:
```xml
<PackageReference Include="Steeltoe.Discovery.Eureka" Version="3.2.4" />
```

### Program.cs:
```csharp
using Steeltoe.Discovery.Client;

services.AddDiscoveryClient(hostContext.Configuration);
// app.UseDiscoveryClient() - obsolete in Steeltoe 3.x, registration automatic
```

### appsettings.json:
```json
{
  "Spring": {
    "Application": { "Name": "PricingService" }
  },
  "Eureka": {
    "Client": {
      "ShouldRegisterWithEureka": true,
      "ShouldFetchRegistry": true,
      "ServiceUrl": "http://localhost:8761/eureka/",
      "ValidateCertificates": false
    },
    "Instance": {
      "AppName": "PricingService",
      "HostName": "host.docker.internal",
      "Port": 5040,
      "NonSecurePortEnabled": true,
      "SecurePortEnabled": false,
      "InstanceId": "PricingService:${random.value}",
      "LeaseRenewalIntervalInSeconds": 10,
      "LeaseExpirationDurationInSeconds": 30
    }
  }
}
```

---

## 5. Kiến trúc triển khai

| Thành phần | Công nghệ | Trạng thái |
|------------|-----------|------------|
| Eureka Server | Spring Boot 3.2.5 + Spring Cloud 2023.0.1 | ✅ Running (Docker) |
| PricingService | .NET 8 + Steeltoe 3.2.4 | ✅ Registered & UP |
| Java | Eclipse Temurin 21 | ✅ |
| Build Tool | Maven 3.9.6 | ✅ |
| Container | Docker (eclipse-temurin:21-jre) | ✅ |
| Port | 8761 (Eureka), 5040 (PricingService) | ✅ Listening |

---

## 6. Verify Service hoạt động

**Swagger UI:** http://localhost:5040/swagger/index.html → **200 OK**  
**Eureka Dashboard:** http://localhost:8761 → **200 OK**  
**Instance Info:** http://localhost:5040/info → **200 OK**

---

**XÁC NHẬN:** Eureka Server đã được cài đặt, PricingService đã cấu hình Steeltoe Eureka Client, đăng ký thành công và hiển thị trạng thái UP trên Eureka Dashboard tại http://localhost:8761