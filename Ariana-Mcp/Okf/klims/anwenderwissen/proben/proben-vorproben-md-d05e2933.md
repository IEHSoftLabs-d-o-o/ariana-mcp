---
type: "Anwenderwissen"
title: "Vorproben"
description: "Anwenderhinweise zu Vorproben."
tags: ["klims", "anwender", "proben"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/proben/proben-vorproben-md-d05e2933"
---
# Vorproben

[[_TOC_]]

# Vorproben

## Aktivierung und Etikettiert-Haken

- beim Aktivieren werden die Eigenschaften der stornierten Vorprobe = Hauptprobe übernommen
- beim Aktivieren werden aus der Hauptprobe Suffixproben erzeugt. Suffixanzahl ist durch die in der Vorproben hinterlegte Anzahl begrenzt.
- das Aktivieren erfolgt durch den Vorprobenimport (Datenimport aus einer Eceltabelle mit Tagebuchnummer) **ODER**
- das Aktivieren erfolgt durch Scannen der Probe
- beim Erfassen in der PA durch Scannen der Probe wird der Etikettiert-Haken sowie der Probeneingang, der Untersuchungsbeginn und der Termin gesetzt. Nachaktivieren über Vorprobenseite oder HMlims mit den drei Einzelschritten 
- beim Vorprobenimport wird der Etikettiert-Haken **nicht** gesetzt
- beim Vorprobenimport werden fertiggemeldete Proben nicht mehr verändert. Es wird dann "Error" in der Auflistung der importierten Proben angezeigt.
- beim Erfassen in der PCR wird der Etikettiert-Haken **nicht** gesetzt, ein Eintragen ist ohne Etikettiert-Haken möglich, wenn die Probe aktiviert ist
- eine Probe ohne Etikettiert-Haken erscheint jetzt auch zum Versenden (Es gibt für Proben ohne Etikettiert-Haken einen Live Check) (08/09/2020).
- über $etikettenvariante: Anzahl und Info auf Barcode-Etikett editierbar: z.B. Kompakt:2:L.mono (beide Doppelpunkte wichtig)
