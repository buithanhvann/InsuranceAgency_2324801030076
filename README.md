# InsuranceAgency — BTH4: Cai dat kien truc Microservices (.NET 8)

Case study cong ty bao hiem: Agent Portal (Blazor WASM + Vue Web) + API Gateway (Ocelot)
+ 8 microservices (Auth, Chat, Pricing, Policy, PolicySearch, Payment, Product, Document).

## 1) Quy tac to chuc source (Task #1) — DA DAT
Moi microservice gom 3 project: `.API` (dinh nghia API/DTO), `.Implementation`
(trienkhai + MediatR handlers), `.Test` (kiem thu). Solution `InsuranceAgency.sln`
chua them: `DashboardService.Api` (contracts cho Blazor), `AgentPortalApiGateway`,
`BlazorWasmClient`.

**NEW (BTH6 - Task #3):** Moi microservice co them 1 **Host project** (chay rieng, dang ky Eureka):
- `ProductService/` (port 5030)
- `PolicyService/` (port 5050)
- `PolicySearchService/` (port 5065)
- `PaymentService/` (port 5070)
- `ChatService/` (port 4099)
- `AuthService/` (port 6060)
- `DocumentService/` (port 5080)
- `DashboardService/` (port 5035)
- `PricingService/` (port 5040) — da co san

Moi host project:
- `Program.cs` — cau hinh Steeltoe Discovery Client (Eureka), Swagger, MediatR
- `appsettings.json` — cau hinh Eureka client (ServiceUrl, InstanceId, Port, HostName)
- `Controllers/` — endpoints `/info`, `/health`, `/api/...`
- `Dockerfile` — multi-stage build cho container
- `launchSettings.json` — profile chay local

## 2) Cai dat moi truong (Task #2) — DA DAT
| Yeu cau | Ket qua |
|---|---|
| MediatR | 12.4.1 trong ca 8 `.Implementation`; mau `AddXService()` goi `services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...))` (xem `PolicyService.Implementation/MediatRSetup.cs`) |
| PostgreSQL >= 10 | 17.11 (host); da chay `postgres/createdatabases.sql` → 4 DB: `lab_netmicro_payments`, `lab_netmicro_jobs`, `lab_netmicro_policy`, `lab_netmicro_pricing` + user `lab_user` |
| RabbitMQ | 3-management (Docker `bth4-rabbit`, UI http://localhost:15672 → 200) |
| Elasticsearch >= 6 | 8.9.2 (Docker `bth4-elastic`, http://localhost:9200 tra ve `number: 8.9.2`) |
| Docker + Compose | Docker 29.8.2, Compose v5.5.1 |
| eureka-server | Thu muc `eureka-server/` (Spring Cloud Eureka, can Java 21 + Gradle) |
| postgres | Thu muc `postgres/` (`Dockerfile` FROM postgres:11.4 + `createdatabases.sql`) |
| scripts | Thu muc `scripts/` (`infra.yml`, `app.yml`, build/run scripts) |
| Web (VueJS) | Thu muc `Web/` |
| BlazorWasmClient | Thu muc `BlazorWasmClient/` — da bo sung DTO contracts con thieu (Product/Policy/PolicySearch/Dashboard) → build 0 loi |
| AgentPortalApiGateway | Thu muc `AgentPortalApiGateway/` (Ocelot + Eureka + JWT) — build 0 loi |

## 3) Eureka Service Discovery (BTH6 - Task #3)
- **Eureka Server**: Chay trong Docker (`docker-compose -f scripts/infra.yml up -d eureka`), UI: http://localhost:8761
- **Steeltoe.Eureka.Client** (v3.2.4): Da cai dat trong tat ca 9 host projects
- **Cau hinh** (`appsettings.json` moi service):
  - `Spring.Application.Name`: Ten service (VD: `ProductService`)
  - `Eureka.Client.ServiceUrl`: `http://localhost:8761/eureka/`
  - `Eureka.Instance.HostName`: `host.docker.internal` (de container goi duoc host)
  - `Eureka.Instance.Port`: Port cua service
  - `Eureka.Instance.InstanceId`: `${ServiceName}:${random.value}`
- **Kiem tra**: Chay tat ca services → truy cap http://localhost:8761 → thay 9 services hien thi `UP`

## 4) Build
- File build: `BTH4-Build.bat` (hoac `dotnet build InsuranceAgency.sln`).
- Bien ban: `BuildLog.txt` → **0 Error, 11 Warning** (warning co san tu Steeltoe/Blazor template, khong phai loi).
- Chup man hinh doan `Build succeeded / 0 Error(s)` trong `BuildLog.txt` de nop San pham #3.

## 5) San pham nop
- **#1 (code):** day repo nay len GitHub + Add thay `caotxuan@gmail.com` lam collaborator
  (trong GitHub: Settings → Collaborators → Add people). Chua lam buoc Add thay thi toi da 8 diem.
- **#2 (DB):** thu muc `DbBackups/` — 4 file `.backup` (pg_dump custom format, PostgreSQL 17):
  `lab_netmicro_payments.backup`, `lab_netmicro_jobs.backup`,
  `lab_netmicro_policy.backup`, `lab_netmicro_pricing.backup`.
  Thay restore: `pg_restore -U postgres -d <db> <file>.backup`.
- **#3 (build):** `BuildLog.txt` + anh chup man hinh ket qua build.

## 6) Ghi chu ky thuat
- Blazor tham chieu `..\Xxx.Api` (chu thuong) trong khi folder la `Xxx.API`; Windows
  khong phan biet hoa-thuong nen van resolve duoc — giu nguyen de khong vo reference goc.
- `PricingService.Api` duoc Blazor tham chieu nhung khong dung type nao → giu rong.
- DB backup hien ~1KB/file vi `createdatabases.sql` chi tao DB rong (chua co bang) — dung voi de bai.
- **Ocelot Gateway** (`AgentPortalApiGateway/ocelot.json`): Da cap nhat routes cho tat ca 9 services (Product, Policy, PolicySearch, Payment, Chat, Auth, Document, Dashboard, Pricing) su dung Service Discovery qua Eureka.
- **Docker Compose** (`scripts/app.yml`): Da co san cac service definitions, can build Dockerfile moi service.
