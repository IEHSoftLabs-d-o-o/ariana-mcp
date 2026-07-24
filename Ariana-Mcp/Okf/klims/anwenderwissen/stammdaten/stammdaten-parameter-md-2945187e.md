---
type: "Anwenderwissen"
title: "Basisparameter"
description: "Anwenderhinweise zu Basisparameter."
tags: ["klims", "anwender", "stammdaten"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/stammdaten/stammdaten-parameter-md-2945187e"
---
# Basisparameter

# **Basisparameter**
_wenn Basisparameter angelegt werden, dann gibt es dort ein Feld Kostenstelle.
Dieses bitte wie folgt ausfüllen:_

- Mibi = 4402
- Chemie = 4406
- Wasser = 4403
- Legionellen = 4452

Fremdlabor Kurzname nur über HMlims pflegbar. Das Feld ist u.a. verantwortlich, dass ein Fremdvergabeauftrag erstellt werden kann.

## Abklatsch beprobte Fläche
* via Klims wird Abklatsch beprobte Fläche als "* Abklatsch beprobte Fläche" und Hilfsparameter-Attribut angelegt
  * dieses Attribut ist in Klims konfigurierbar
* Basisparameter "* Abklatsch beprobte Fläche" erstellt als Hilfe für PA  bei IFU
  * mit fester Hinterlegung von 10qcm
  * Basisparameter als * Abklatsch beprobte Fläche" benannt damit die Berechnung für GKZ... klappt
  * als Formel damit direkt abgehakt
  * damit in Klims nicht nachträglich anpassbar
* Basisparameter "Abklatsch beprobte Fläche" früherer Hilfsparameter 
  * mit fester Hinterlegung von 10qcm
  * als Formel damit direkt abgehakt
  * damit in Klims nicht nachträglich anpassbar
  * früher Hilfsparameter formatiert, warum jetzt nicht mehr ist unklar
