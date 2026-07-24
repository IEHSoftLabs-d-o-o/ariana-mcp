---
type: "Anwenderwissen"
title: "Paket-/Parameter-Abgleich beim Kopieren von Proben"
description: "Anwenderhinweise zu Paket-/Parameter-Abgleich beim Kopieren von Proben."
tags: ["klims", "anwender", "hm-lims"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/hm-lims/hm-lims-hm-lims-paket-parameter-abgleich-beim-kopieren-von-probe-51d85cb6"
---
# Paket-/Parameter-Abgleich beim Kopieren von Proben

# **Paket-/Parameter-Abgleich beim Kopieren von Proben**

_Beim Kopieren erfolgt erst mal kein 1:1-Abgleich der vorhandenen Parameter in der zu kopierenden Probe mit den Stammdaten des Paketes!_

_Nur wenn Parameter oder Pakete oder beides in den Kopiereinsteilungen (HM-LIMS --> Stammdaten (System) --> Einstellungen Proben kopieren) hinterlegt sind, erfolgt ein Abgleich (wie genau, siehe unten).
Diese Einstellung ist wichtig beim Ändern von Paketen bzgl. der Parameterzuordnung. Wenn an dieser Stelle keine Hinterlegung erfolgt und in einem Paket ein Parameter z.B. entfernt wird, wird durch Kopieren einer alten Probe der Parameter wieder eingefügt, obwohl er in den Stammdaten nicht mehr hinterlegt ist! Diese Einstellungen dürfen nur von Prüfleitern bzw. Personen, die Parameter und Paket anlegen, eingefügt oder geändert werden._

## **Folgende Einstellungen sind möglich:**

**Parameter ohne Berücksichtigung des Pakets; Spalte Paket bleibt leer:**
- _Für jeden aufgelisteteten und in der Probe vorhandenen Parameter wird geprüft, ob er sich in den Stammdaten auch im gleichen Paket, wie in der Probe befindet. Sollte dies nicht der Fall sein, wird der Parameter aus dem Paket aus der Probe entfernt._

### **Parameter mit Einstellung für ein Paket; Spalten Paket und Parameter gefüllt:**
- _Für jeden aufgelisteteten und in der Probe vorhandenen Parameter wird geprüft, ob sich dieser auch in den Stammdaten des Pakets befindet._
- _Falls ja, verbleibt dieser im Paket in der Probe._
- _Falls nein, wird der Parameter aus dem Paket aus der Probe entfernt._
- _Sollte ein hier aufgelisteter Parameter nicht in diesem Paket in der Probe existieren, so wird geprüft, ob sich der Parameter in den Stammdaten des Pakets befindet._
- _Falls ja, wird der Parameter in das Paket in die Probe entsprechend seiner Sortierung in den Stammdaten eingefügt._
- _Falls nein, geschieht nichts._

### **Pakete ohne Parametereinstellung; Spalte Parameter bleibt leer:**
- _Alle Parameter im Paket in der Probe, die nicht in den Stammdaten des Pakets enthalten sind, werden aus dem Paket aus der Probe entfernt._
- _Alle Parameter aus den Stammdaten des Pakets, die nicht im Paket in der Probe enthalten sind, werden entsprechend ihrer Sortierung in den Stammdaten des Pakets in das Paket in die Probe eingefügt._ 

_Bei Rechnern mit Windows 10 kann es dazu kommen, dass die Kopiereinstellungen nicht angezeigt werden und auch in der Navigation nirgends zu finden sind. Hier ist es nötig, dass man nochmal bei geöffnetem Fenster zurück geht ins HM-Lims und erneut auf "Einstellungen Proben kopieren" klickt! Nur dann kann man sich über den Klick auf "Suchen" alle Pakete und Parameter die zum Abgleich eingetragen sind anzeigen lassen._
