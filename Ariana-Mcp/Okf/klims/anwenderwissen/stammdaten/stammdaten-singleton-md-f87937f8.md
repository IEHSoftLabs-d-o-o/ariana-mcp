---
type: "Anwenderwissen"
title: "Automatische Beurteilung"
description: "Anwenderhinweise zu Automatische Beurteilung."
tags: ["klims", "anwender", "stammdaten"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/stammdaten/stammdaten-singleton-md-f87937f8"
---
# Automatische Beurteilung

#Automatische Beurteilung
ValueComparisonStrategyConfiguration: Indikatorliste für alphanummerische Werte
- Häkchen steht für exakter Match
- kein Häkchen: Ergebnis enthält Zeichenfolge

#Pobenanlage (Fotos) Konfiguration
      
1.  **Beispiel bei zwei Treffern:**  
    Wenn „Kundennummer = erstes Bild drucken“ und „Probenbezeichnung enthält Tupfer = kein Bild drucken“ beide passen, entscheidet die Reihenfolge in der Liste. Sprich die Regel, die weiter oben steht, gewinnt. wobei Produktklasse und Probenbezeichnung generell höher gewichtet wird (mündliche Auskunft pebr)

2.  **Originale behalten:**
Wenn aktiviert, wird zusätzlich zum bearbeiteten Bild eine Originalkopie im Ordner _.**original**_  gespeichert. Das normale Bild wird trotzdem weiterverarbeitet (Skaliert, Metadaten entfernt, etc.)
