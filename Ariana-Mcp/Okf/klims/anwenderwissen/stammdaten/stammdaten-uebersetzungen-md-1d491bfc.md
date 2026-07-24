---
type: "Anwenderwissen"
title: "Uebersetzungen"
description: "Anwenderhinweise zu Uebersetzungen."
tags: ["klims", "anwender", "stammdaten"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/stammdaten/stammdaten-uebersetzungen-md-1d491bfc"
---
# Uebersetzungen

# **Uebersetzungen**

### **Englisch:**
1. Paket-Überschriften beginnen mit Großbuchstaben
1. Parameter-Namen / Attribut-Namen beginnen mit Kleinbuchstaben
1. Werden neue Parameter angelegt, bitte gleich entsprechend übersetzen
1. bei Zusatzfeldern ohne Doppelpunkt pflegen (Charge statt falsch: "Charge:")
1. Pestizide \r\nLC-Screening codiert einen Zeilenumbruch und wird derzeit (30/03/2020) nicht erkannt. Die Hinterlegung in Übersetzungen kann durchgeführt werden. Die Hinterlegung mit "\r\n" ist dann identisch mit der ohne. Daher findet man die Pestizide gerade doppelt. Ticket ist erstellt. Den wirklich im PB vorhandenen Namen findet man unter Probendetails ganz unten.

### **Übersetzunglogik:**
_der Prüfbericht wird in Klims folgendermaßen übersetzt:_

- wenn der Berichtsname auf LL Spalten Englisch geändert wird, dann wird ein englische Prüfbericht erzeugt
- Prinzipiell werden alle Zusatzfelder angezeigt, die auf zu drucken stehen und einen Wert besitzen
- In Klims wird überprüft, ob es für das Zusatzfeld eine englische Übersetzung gibt (<externer Link entfernt>):
- Falls ja: Das Zusatzfeld wird übersetzt
- Falls nein: Das Zusatzfeld wird in der Ursprungssprache, also deutsch, ausgegeben

_(Das englische Zusatzfeld ist rausgefallen, weil es nicht gefüllt war. Das deutsche ist rausgefallen, da es nicht auf zu drucken gestanden ist (wenn ich deine Beschreibung zur Originalprobe richtig verstanden hab))_
