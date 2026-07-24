---
type: "Anwenderwissen"
title: "Exotische Stammdaten"
description: "Anwenderhinweise zu Exotische Stammdaten."
tags: ["klims", "anwender", "hm-lims"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/hm-lims/hm-lims-hm-lims-exotische-stammdaten-md-1f93e631"
---
# Exotische Stammdaten

# **Exotische Stammdaten**
##  **Automatisch über Nacht verschickte Listen**
**Abruflisten:**
_HMLims > Stammdaten(System) > Einstellungen Abruflisten_

### **Informationslisten:**
- Aktuell gibt es nur eine, die §44aLFGBMitteilungspflicht
- HMLims > Stammdaten(System) > Einstellungen Informationslisten

### **Scan-Tool Parameter für den externen Drucker:**

_Im Mibi Scan-Tool/Scan-Control werden bestimmte Parameter auf den externen Drucker gedruckt. Dies geschieht, wie bei den anderen Parametern auch, nicht automatisch, sondern auch sie werden erst in der Labelmaster-Liste auf der rechten Seite gesammelt und erst beim Drücken des Buttons "Export (LM1)" gedruckt._

_Für den auf dem externen Drucker zu druckenden Parameter müssen lediglich die entsprechenden Einträge unter Import/Export > Stammdaten > Exportübersetzungen > LabelmasterAlternativ eingetragen werden, z.B._

|**Element**| **Wert** | **Ersatzwert** |
|--|--|--|
| Naehrboden | M Campylobacter Bestätigung Mibi | CampyCount |
| Sotierung | M Campylobacter Bestätigung Mibi | 26|
|  Stofflaufzettel| M Campylobacter Bestätigung Mibi| Campq |

_(Welche Auswirkung Sortierung und Stofflaufzettel hier haben, ist nicht ganz klar.)_
