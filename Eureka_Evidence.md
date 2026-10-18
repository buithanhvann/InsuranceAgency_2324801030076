# BẢNG CHỨNG MINH EUREKA SERVER ĐÃ CÀI ĐẶT VÀ HOẠT ĐỘNG

**Sinh viên:** Bùi Thanh Vân - 2324801030076  
**Môn:** Bài thực hành 5 - Case study Công ty bảo hiểm  
**Ngày:** 18/10/2026

---

## 1. Docker Container Eureka đang chạy

```powershell
PS> docker ps --format "{{.Names}} {{.Status}} {{.Ports}}"
eureka-server   Up 2 minutes   0.0.0.0:8761->8761/tcp
```

![Container Status](eureka_docker_ps.png)

---

## 2. Log khởi động Eureka Server (thành công)

```
2026-10-18T06:05:47.438Z  INFO 1 --- [           main] o.s.b.w.embedded.tomcat.TomcatWebServer  : Tomcat initialized with port 8761 (http)
2026-10-18T06:05:47.450Z  INFO 1 --- [           main] o.apache.catalina.core.StandardService   : Starting service [Tomcat]
2026-10-18T06:05:47.478Z  INFO 1 --- [           main] w.s.c.ServletWebServerApplicationContext : Root WebApplicationContext: initialization completed in 1554 ms
2026-10-18T06:05:49.893Z  INFO 1 --- [           main] c.n.eureka.DefaultEurekaServerContext    : Initializing ...
2026-10-18T06:05:50.006Z  INFO 1 --- [           main] c.n.e.registry.AbstractInstanceRegistry  : Finished initializing remote region registries.
2026-10-18T06:05:50.011Z  INFO 1 --- [           main] c.n.eureka.DefaultEurekaServerContext    : Initialized
2026-10-18T06:05:50.117Z  INFO 1 --- [           main] o.s.c.n.e.s.EurekaServiceRegistry        : Registering application UNKNOWN with eureka with status UP
2026-10-18T06:05:50.136Z  INFO 1 --- [       Thread-9] c.n.e.r.PeerAwareInstanceRegistryImpl    : Changing status to UP
2026-10-18T06:05:50.146Z  INFO 1 --- [       Thread-9] e.s.EurekaServerInitializerConfiguration : Started Eureka Server
2026-10-18T06:05:50.152Z  INFO 1 --- [           main] o.s.b.w.embedded.tomcat.TomcatWebServer  : Tomcat started on port 8761 (http) with context path ''
2026-10-18T06:05:50.175Z  INFO 1 --- [           main] n.c.m.eurekaserver.EurekaServer          : Started EurekaServer in 4.916 seconds (process running for 5.58)
```

**Kết luận:** Eureka Server khởi động thành công trong **4.916 giây**, Tomcat lắng nghe trên port **8761**.

---

## 3. Eureka Dashboard truy cập được (HTTP 200)

**URL:** http://localhost:8761  
**Status Code:** 200 OK  
**Response:** Trang HTML Eureka Dashboard chuẩn

![Eureka Dashboard](eureka_dashboard.png)

---

## 4. Kiến trúc triển khai

| Thành phần | Công nghệ | Trạng thái |
|------------|-----------|------------|
| Eureka Server | Spring Boot 3.2.5 + Spring Cloud 2023.0.1 | ✅ Running |
| Java | Eclipse Temurin 21 | ✅ |
| Build Tool | Maven 3.9.6 | ✅ |
| Container | Docker (eclipse-temurin:21-jre) | ✅ |
| Port | 8761 | ✅ Listening |

---

## 5. Docker Image & Commands

**Build:**
```bash
cd eureka-server
docker build -t eureka-server .
```

**Run:**
```bash
docker run -d --name eureka-server -p 8761:8761 eureka-server
```

**Verify:**
```bash
docker logs eureka-server | grep "Started EurekaServer"
# Output: Started EurekaServer in 4.916 seconds
curl http://localhost:8761
# Output: HTTP 200 + HTML Dashboard
```

---

## 6. Source Code

**GitHub:** https://github.com/buithanhvann/InsuranceAgency_2324801030076/tree/main/eureka-server

- `pom.xml` - Maven config Spring Boot 3.2.5 + Spring Cloud 2023.0.1
- `src/main/java/.../EurekaServer.java` - Main class với `@EnableEurekaServer`
- `Dockerfile` - Multi-stage build (Maven → JRE 21)

---

**XÁC NHẬN:** Eureka Server đã được cài đặt, build, chạy thành công trên laptop và dashboard truy cập được tại http://localhost:8761