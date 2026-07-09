# Ariana MCP

MCP server for read-only access to ArianaLab (Klims LIMS) from LLM clients such as Open WebUI. The server exposes tools and resources over HTTP at `/mcp` and supports MCP Streamable HTTP.

Tool and resource descriptions are in German and optimized for DAUS-style prompts (e.g. *„Suche die Probe 26-0318054“*, *„Welcher Kunde gehört zu dieser Probe?“*).

## Important Notice

This project is still evolving. Responses are mostly JSON from ArianaLab. Behavior across different LLMs has not been broadly validated.

Especially with local Ollama models, there is no guarantee that the model will reliably select the correct tools or parameters. Use a model with solid tool/function-calling support and enable MCP tools explicitly in your client.

## Current Features

- MCP server on ASP.NET Core with stateless HTTP transport at `http://<host>:5000/mcp`
- MCP tools and resource templates for samples, customers, orders, reference data, and diagnostics
- German server instructions and German tool/resource metadata for DAUS workflows
- Structured error handling (`McpException` with `isError: true` for recoverable failures)
- Server-side EasyQuery search for customers and samples (no full customer list download)
- Sensitive endpoints gated by `AraianLab:EnableSensitiveData` (default: `false`)
- Health endpoint at `/health` and info endpoint at `/`
- Configuration via `appsettings.json`, environment variables, and optional `appsettings.override.json`
- ArianaLab integration via HTTP client with Basic Auth
- Serilog console logging
- Dockerfile and Docker Compose baseline for running alongside Open WebUI

## Typical DAUS Workflow

```text
Frage (Deutsch) → search_customers / search_samples
                → get_sample_short_info
                → customer_info_by_sample
                → sample_results_by_id
                → report_json_by_sample
```

| Step | Tool / Resource | When to use |
| --- | --- | --- |
| Find customer | `search_customers` | Partial name or customer number |
| Find sample | `search_samples` | Tagebuchnummer, customer, date range, status |
| Quick overview | `get_sample_short_info` | Status and links without full payload |
| Customer context | `customer_info_by_sample` | Which customer belongs to a sample |
| Results | `sample_results_by_id` | Parameters, measured values, methods |
| Report | `report_json_by_sample` | Prüfbericht content and assessment |

All read tools are marked as read-only, idempotent, and non-destructive.

## MCP Tools

### Samples (Proben)

| Tool | Description |
| --- | --- |
| `search_samples` | Search samples by Tagebuchnummer, customer, customer sample number, description, date range, or status |
| `get_sample` | Load full sample data for one Tagebuchnummer |
| `sample_by_id` | Alias for `get_sample`; also accepts a list of IDs (batch, per-item errors) |
| `get_sample_short_info` | Compact sample overview (preferred for quick questions) |
| `report_json_by_sample` | Structured exportable Prüfbericht JSON |
| `customer_info_by_sample` | Customer information for the sample's client |
| `sample_results_by_id` | Processing/results data including parameters and sub-samples |
| `get_sample_logs` | Sample audit log (**requires `EnableSensitiveData`**) |

### Customers (Kunden)

| Tool | Description |
| --- | --- |
| `search_customers` | Server-side search by name (`Anzeigename`) or number (`Nummer`) |
| `search_customers_batch` | Multiple search terms in one call |
| `customer_by_name` | Exact name lookup (batch supported) |
| `customer_info_by_id` | Detailed customer information by KundeId (batch supported) |

### Reference Data

| Tool | Description |
| --- | --- |
| `search_analyses` | Search analysis catalog |
| `get_public_analyses` | Public analysis catalog |
| `get_methods` | Test methods |
| `get_product_classes` | Product classes / Warengruppen |
| `list_lab_parameters` | Search lab parameters / analytes |
| `list_units` | Units |
| `list_product_groups` | Product groups |
| `list_sample_groups` | Sample groups |
| `list_test_packages` | Test packages (Prüfpakete) |

### Orders & Planning

| Tool | Description |
| --- | --- |
| `search_orders` | Search internal orders (Probenanlage) |
| `get_order` | Load one internal order by ID |
| `search_customer_orders` | Search imported customer orders |
| `get_customer_order` | Load one customer order by ID |
| `get_planning_orders` | Search planning data by module (`auftraege`, `kundenauftraege`) |

### System

| Tool | Description |
| --- | --- |
| `get_system_info` | Check ArianaLab reachability and authenticated user |

### Sensitive (gated)

These tools require `AraianLab:EnableSensitiveData=true`:

| Tool | Description |
| --- | --- |
| `search_invoices` | Search invoices |
| `get_invoice` | Load invoice by number |
| `search_cor` | Search Customer Order Requests |
| `get_cor` | Load one COR by ID |
| `validate_cor_gateway` | Validate a COR gateway payload without saving |

## MCP Resources

Resource templates for clients that prefer MCP resources over tool calls:

| URI template | Description | Gated |
| --- | --- | --- |
| `arianalab://sample/{tagebuchnummer}` | Full sample data | No |
| `arianalab://sample/{tagebuchnummer}/logs` | Sample audit log | Yes |
| `arianalab://sample/{tagebuchnummer}/attachments` | Attachment metadata | Yes |
| `arianalab://customer/{nummer}` | Customer master record | No |
| `arianalab://analysis/{id}` | Analysis catalog entry | No |
| `arianalab://cor/{corId}` | Customer Order Request | Yes |
| `arianalab://invoice/{id}` | Invoice | Yes |
| `arianalab://planning/{module}/{id}` | Planning/order record | No |

## Prerequisites

- .NET 10 SDK (local run)
- Docker and Docker Compose (optional, with Open WebUI)
- Ollama or another model backend with tool-calling support
- Open WebUI with MCP Streamable HTTP support (or another MCP client)
- ArianaLab credentials with `LK.Intern` (and additional roles for invoices/COR if needed)

## Configuration

Environment variables:

```powershell
$env:ARIANALAB_USER = "<username>"
$env:ARIANALAB_PASSWORD = "<password>"
$env:ARIANALAB_BASE_URL = "https://klims.labor-kneissler.de/"
```

Optional local override in `Ariana-Mcp/appsettings.override.json`:

```json
{
  "AraianLab": {
    "User": "<username>",
    "Password": "<password>",
    "BaseUrl": "https://klims.labor-kneissler.de/",
    "EnableSensitiveData": false
  }
}
```

| Setting | Default | Description |
| --- | --- | --- |
| `User` | required | ArianaLab Basic Auth username |
| `Password` | required | ArianaLab Basic Auth password |
| `BaseUrl` | required | ArianaLab base URL |
| `EnableSensitiveData` | `false` | Enable logs, attachments, invoices, and COR tools/resources |

Do not commit credentials to Git.

## Running Locally

From the repository root:

```powershell
dotnet run --project .\Ariana-Mcp\Ariana-Mcp.csproj --urls http://localhost:5000
```

Endpoints:

- `http://localhost:5000/` — app name and version
- `http://localhost:5000/health` — health check
- `http://localhost:5000/mcp` — MCP endpoint

## Using with Open WebUI and Ollama

```text
Ollama <-> Open WebUI <-> Ariana MCP <-> ArianaLab
```

### Open WebUI setup

1. Start Open WebUI and connect it to Ollama.
2. Go to **Admin Panel → Settings → External Tools**.
3. Add a server with type **MCP (Streamable HTTP)**.
4. Set the MCP URL:
   - Host MCP, Open WebUI in Docker: `http://host.docker.internal:5000/mcp`
   - Same Docker Compose network: `http://Ariana-Mcp:5000/mcp`
   - Both on host: `http://localhost:5000/mcp`
5. Enable tools for the desired model (Function Calling: **Native** if available).

### Example prompts (German)

```text
Suche den Kunden "Müller" und zeige mir die Treffer.
```

```text
Suche die Probe mit der Tagebuchnummer 26-0318054 und gib mir eine kurze Übersicht.
```

```text
Welcher Kunde gehört zur Probe 26-0318054? Zeige mir die Kundeninformationen.
```

```text
Zeige mir die Ergebnisse und Parameter für Probe 26-0318054.
```

```text
Lade den Prüfbericht für Probe 26-0318054.
```

## Docker Compose

```powershell
docker compose up --build
```

- Open WebUI: `http://localhost:8080`
- MCP server: `http://localhost:5000` (host) or `http://Ariana-Mcp:5000` (Compose network)

Adjust ArianaLab credentials and `OLLAMA_BASE_URL` in `docker-compose.yml` for your environment.

## Known Limitations

- Responses are raw or compact JSON from ArianaLab, not natural-language summaries.
- No authentication on the MCP endpoint itself; protect `/mcp` at the network or reverse-proxy level.
- Large payloads (full sample, report JSON) can be slow and may fill the model context window.
- Sensitive data (PII, billing, audit logs) is available only when explicitly enabled.
- Local Ollama models may use MCP tools unreliably depending on model and client configuration.
- Tool and resource names may still change as the integration matures.
