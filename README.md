# ContosoPizza Template

A clean and modern ASP.NET Core Web API project template featuring:

- Entity Framework Core with PostgreSQL
- Docker support via Testcontainers and Docker Compose
- xUnit test project with integration testing
- GitHub Actions CI pipeline
- Code formatting and branch protection setup
- Ready for scalable development with folder structure and patterns

---

## 🚀 Overview

This project is a **starter template** for building RESTful APIs using ASP.NET Core 8 and PostgreSQL. It is designed to provide a solid foundation for new projects by including modern development practices like integration testing with Testcontainers, Docker-based environments, GitHub Actions CI workflows, and opinionated code structure.

Use this template to quickly scaffold new APIs with testing and infrastructure in place.

---

## 📦 Project Structure

```
ContosoPizza/
├── ContosoPizza.sln               # Solution file
├── ContosoPizza.Web/             # Main ASP.NET Core Web API project
├── ContosoPizza.Tests/           # Test project with unit and integration tests
├── docker-compose.yml            # Compose file for local PostgreSQL
├── Dockerfile                    # Container build for API
└── .github/workflows/            # CI pipeline (build, test, format)
```

---

## 🧪 Tests

The test project includes:

- Unit tests using xUnit, Moq, and FluentAssertions
- Integration tests using Testcontainers with PostgreSQL
- Automatic truncation of database between tests

To run tests:

```bash
dotnet test
```

---

## 🐳 Docker & Local Dev

You can run a local PostgreSQL instance using Docker Compose:

```bash
docker-compose up -d
```

> **NB:** Docker Desktop must be running.

The integration tests will spin up an isolated PostgreSQL container via Testcontainers.

---

## ✅ GitHub Actions CI

CI pipeline includes:

- Build & Test
- Code Formatting Check (via `dotnet format`)
- Cache for faster runs

---

## 🧹 Code Formatting

Run formatting locally:

```bash
dotnet format
```

This ensures consistency across developers and avoids merge conflicts.

---

## 🔒 Branch Protection

To protect `main`:

1. Go to **Settings > Branches > main**
2. Enable:
   - ✅ Require status checks
   - ✅ Require branches to be up to date
   - ✅ Require approval before merging
3. Require all GitHub Actions workflows to pass

---

## 🧪 Project Template Usage

You can turn this into a reusable template:

```bash
dotnet new -i .
dotnet new contosopizza -n MyNewApi
```

This will scaffold a fresh project with renamed namespaces and folder structure.

---

## 📝 License

MIT
