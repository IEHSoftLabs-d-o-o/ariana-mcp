# Ariana MCP

Ein frueher MCP-Server fuer den Zugriff auf ArianaLab-Daten aus LLM-Clients wie Open WebUI. Der Server stellt die Tools per HTTP unter `/mcp` bereit und kann dadurch von Clients genutzt werden, die MCP ueber Streamable HTTP unterstuetzen.

## Wichtiger Hinweis

Dieses Projekt ist in einem sehr fruehen Entwicklungsstand. Die vorhandenen Tools sind experimentell, die Rueckgaben sind aktuell weitgehend rohe JSON-Antworten aus ArianaLab und das Verhalten wurde noch nicht breit mit verschiedenen LLMs validiert.

Besonders bei lokalen Ollama-Modellen ist nicht garantiert, dass das Modell die MCP-Tools zuverlaessig erkennt, korrekt auswaehlt oder die Antworten sinnvoll verarbeitet. Viele Modelle ignorieren Tools, verwenden falsche Parameter oder brechen Tool-Aufrufe ab. Fuer brauchbare Ergebnisse sollte ein Modell mit guter Tool- bzw. Function-Calling-Unterstuetzung verwendet werden.

## Aktuelle Features

- MCP-Server auf ASP.NET Core mit HTTP-Transport unter `http://<host>:5000/mcp`.
- Stateless MCP-Transport fuer einfache Einbindung in webbasierte Clients.
- Health-Endpunkt unter `/health`.
- Info-Endpunkt unter `/`, der Name und Version der Anwendung zurueckgibt.
- Konfiguration ueber `appsettings.json`, Umgebungsvariablen und optional `appsettings.override.json`.
- ArianaLab-Anbindung per HTTP-Client mit Basic Auth.
- Serilog-Konsolenlogging.
- Dockerfile und Docker-Compose-Grundkonfiguration fuer den Betrieb zusammen mit Open WebUI.

### MCP-Tools

| Tool | Beschreibung |
| --- | --- |
| `customer_by_name` | Sucht einen Kunden anhand des Namens und gibt die Kunden-JSON-Antwort aus ArianaLab zurueck. |
| `customer_info_by_id` | Gibt Kundeninformationen fuer eine Kunden-ID zurueck. |
| `all_customers` | Gibt alle Kunden als JSON zurueck. Dieser Aufruf kann wegen der Datenmenge laenger dauern. |
| `sample_by_id` | Sucht eine Probe anhand der Proben-ID und gibt die JSON-Antwort aus ArianaLab zurueck. |

Alle aktuellen Tools sind lesend, idempotent und nicht destruktiv markiert.

## Voraussetzungen

- .NET 10 SDK, wenn der MCP-Server lokal gestartet wird.
- Docker und Docker Compose, wenn Open WebUI und der MCP-Server als Container laufen sollen.
- Eine laufende Ollama-Instanz, z. B. lokal auf `http://localhost:11434`.
- Open WebUI mit MCP-Unterstuetzung fuer Streamable HTTP.
- ArianaLab-Zugangsdaten.

## Konfiguration

Der MCP-Server erwartet folgende Umgebungsvariablen:

```powershell
$env:ARIANALAB_USER = "<benutzername>"
$env:ARIANALAB_PASSWORD = "<passwort>"
$env:ARIANALAB_BASE_URL = "https://klims.labor-kneissler.de/"
```

Alternativ kann lokal eine nicht versionierte `appsettings.override.json` im Projektordner `Ariana-Mcp` verwendet werden:

```json
{
  "AraianLab": {
    "User": "<benutzername>",
    "Password": "<passwort>",
    "BaseUrl": "https://klims.labor-kneissler.de/"
  }
}
```

Zugangsdaten sollten nicht in Git committet werden.

## MCP-Server lokal starten

Aus dem Repository-Root:

```powershell
dotnet run --project .\Ariana-Mcp\Ariana-Mcp.csproj --urls http://localhost:5000
```

Danach sollten diese Endpunkte erreichbar sein:

- `http://localhost:5000/`
- `http://localhost:5000/health`
- `http://localhost:5000/mcp`

Der `/mcp`-Endpunkt ist der relevante Endpunkt fuer Open WebUI oder andere MCP-Clients.

## Verwendung mit Open WebUI und Ollama

Eine typische lokale Architektur sieht so aus:

```text
Ollama <-> Open WebUI <-> Ariana MCP <-> ArianaLab
```

Ollama stellt das Sprachmodell bereit. Open WebUI ist die Chat-Oberflaeche und verbindet das Modell mit externen Tools. Ariana MCP stellt die ArianaLab-Funktionen als MCP-Tools bereit.

### Open WebUI konfigurieren

1. Open WebUI starten und mit Ollama verbinden.
2. In Open WebUI zu `Admin Panel -> Settings -> External Tools` wechseln.
3. Einen neuen Server hinzufuegen.
4. Als Typ `MCP (Streamable HTTP)` auswaehlen.
5. Als URL den MCP-Endpunkt eintragen:
   - Bei lokalem Start des MCP-Servers: `http://host.docker.internal:5000/mcp`, wenn Open WebUI in Docker laeuft.
   - Wenn Open WebUI und der MCP-Server im gleichen Docker-Compose-Netz laufen: `http://Ariana-Mcp:5000/mcp`.
   - Wenn Open WebUI direkt auf dem Host laeuft: `http://localhost:5000/mcp`.
6. Speichern und die Tools im Chat bzw. fuer das gewuenschte Modell aktivieren.
7. Ein Modell verwenden, das Tool- bzw. Function-Calling unterstuetzt. In Open WebUI sollte Function Calling nach Moeglichkeit auf `Native` stehen.

Beispiel-Prompts:

```text
Suche den Kunden mit dem Namen "<Name>" ueber die verfuegbaren Tools.
```

```text
Hole die Informationen zur Kunden-ID "14197".
```

```text
Suche die Probe "26-0318054".
```

## Start mit Docker Compose

Die vorhandene `docker-compose.yml` enthaelt einen MCP-Service und Open WebUI. Vor dem Start sollten die ArianaLab-Zugangsdaten und die Ollama-URL angepasst werden.

```powershell
docker compose up --build
```

Danach ist Open WebUI standardmaessig unter `http://localhost:8080` erreichbar. Der MCP-Server ist auf dem Host unter `http://localhost:5000` und innerhalb des Compose-Netzwerks unter `http://Ariana-Mcp:5000` erreichbar.

Wenn Ollama auf dem Windows-Host laeuft und Open WebUI im Container gestartet wird, ist haeufig `http://host.docker.internal:11434` die passende Ollama-URL. Die aktuell in `docker-compose.yml` gesetzte `OLLAMA_BASE_URL` muss ggf. an die lokale Umgebung angepasst werden.

## Bekannte Einschraenkungen

- Die Tool-Antworten sind aktuell rohe JSON-Strings und noch nicht fuer LLMs aufbereitet.
- Es gibt noch keine Authentifizierung oder Zugriffsbeschraenkung fuer den MCP-Endpunkt selbst.
- Fehler werden teilweise als Textantworten an das Modell zurueckgegeben.
- `all_customers` kann grosse Datenmengen liefern und langsam sein.
- Lokale Modelle ueber Ollama koennen MCP-Tools je nach Modell und Open-WebUI-Konfiguration unzuverlaessig verwenden.
- Die Schnittstelle und Tool-Namen koennen sich noch aendern.

