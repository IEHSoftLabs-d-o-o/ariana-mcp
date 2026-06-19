# Ariana MCP

An early-stage MCP server for accessing ArianaLab data from LLM clients such as Open WebUI. The server exposes tools over HTTP at `/mcp` and can be used by clients that support MCP via Streamable HTTP.

## Important Notice

This project is in a very early stage of development. The available tools are experimental, responses are currently mostly raw JSON from ArianaLab, and behavior has not been broadly validated across different LLMs.

Especially with local Ollama models, there is no guarantee that the model will reliably recognize MCP tools, select the correct ones, or process responses meaningfully. Many models ignore tools, use incorrect parameters, or abort tool calls. For usable results, use a model with solid tool or function-calling support.

## Current Features

- MCP server on ASP.NET Core with HTTP transport at `http://<host>:5000/mcp`.
- Stateless MCP transport for easy integration with web-based clients.
- Health endpoint at `/health`.
- Info endpoint at `/` that returns the application name and version.
- Configuration via `appsettings.json`, environment variables, and optionally `appsettings.override.json`.
- ArianaLab integration via HTTP client with Basic Auth.
- Serilog console logging.
- Dockerfile and Docker Compose baseline configuration for running alongside Open WebUI.

### MCP Tools

| Tool | Description |
| --- | --- |
| `customer_by_name` | Looks up a customer by name and returns the customer JSON response from ArianaLab. |
| `customer_info_by_id` | Returns customer information for a customer ID. |
| `all_customers` | Returns all customers as JSON. This call may take longer due to payload size. |
| `sample_by_id` | Looks up a sample by sample ID and returns the JSON response from ArianaLab. |

All current tools are marked as read-only, idempotent, and non-destructive.

## Prerequisites

- .NET 10 SDK, if running the MCP server locally.
- Docker and Docker Compose, if running Open WebUI and the MCP server as containers.
- A running Ollama instance, e.g. locally at `http://localhost:11434`.
- Open WebUI with MCP support for Streamable HTTP.
- ArianaLab credentials.

## Configuration

The MCP server expects the following environment variables:

```powershell
$env:ARIANALAB_USER = "<username>"
$env:ARIANALAB_PASSWORD = "<password>"
$env:ARIANALAB_BASE_URL = "https://klims.labor-kneissler.de/"
```

Alternatively, you can use a non-versioned `appsettings.override.json` locally in the `Ariana-Mcp` project folder:

```json
{
  "AraianLab": {
    "User": "<username>",
    "Password": "<password>",
    "BaseUrl": "https://klims.labor-kneissler.de/"
  }
}
```

Credentials should not be committed to Git.

## Running the MCP Server Locally

From the repository root:

```powershell
dotnet run --project .\Ariana-Mcp\Ariana-Mcp.csproj --urls http://localhost:5000
```

After that, these endpoints should be reachable:

- `http://localhost:5000/`
- `http://localhost:5000/health`
- `http://localhost:5000/mcp`

The `/mcp` endpoint is the relevant endpoint for Open WebUI or other MCP clients.

## Using with Open WebUI and Ollama

A typical local architecture looks like this:

```text
Ollama <-> Open WebUI <-> Ariana MCP <-> ArianaLab
```

Ollama provides the language model. Open WebUI is the chat interface and connects the model to external tools. Ariana MCP exposes ArianaLab functionality as MCP tools.

### Configuring Open WebUI

1. Start Open WebUI and connect it to Ollama.
2. In Open WebUI, go to `Admin Panel -> Settings -> External Tools`.
3. Add a new server.
4. Set the type to `MCP (Streamable HTTP)`.
5. Enter the MCP endpoint URL:
   - If the MCP server runs locally on the host and Open WebUI runs in Docker: `http://host.docker.internal:5000/mcp`.
   - If Open WebUI and the MCP server run in the same Docker Compose network: `http://Ariana-Mcp:5000/mcp`.
   - If Open WebUI runs directly on the host: `http://localhost:5000/mcp`.
6. Save and enable the tools in chat or for the desired model.
7. Use a model that supports tool or function calling. In Open WebUI, set Function Calling to `Native` if possible.

Example prompts:

```text
Find the customer named "<Name>" using the available tools.
```

```text
Get the information for customer ID "14197".
```

```text
Look up sample "26-0318054".
```

## Starting with Docker Compose

The existing `docker-compose.yml` includes an MCP service and Open WebUI. Before starting, adjust the ArianaLab credentials and the Ollama URL.

```powershell
docker compose up --build
```

After that, Open WebUI is available by default at `http://localhost:8080`. The MCP server is reachable on the host at `http://localhost:5000` and inside the Compose network at `http://Ariana-Mcp:5000`.

If Ollama runs on the Windows host and Open WebUI is started in a container, `http://host.docker.internal:11434` is often the correct Ollama URL. The `OLLAMA_BASE_URL` currently set in `docker-compose.yml` may need to be adjusted for your local environment.

## Known Limitations

- Tool responses are currently raw JSON strings and are not yet tailored for LLMs.
- There is no authentication or access control on the MCP endpoint itself.
- Errors are sometimes returned to the model as plain text responses.
- `all_customers` can return large amounts of data and may be slow.
- Local models via Ollama may use MCP tools unreliably depending on the model and Open WebUI configuration.
- The API and tool names may still change.
