---
type: "Anwenderwissen"
title: "Beurteilungsassistent"
description: "Anwenderhinweise zu Beurteilungsassistent."
tags: ["klims", "anwender", "proben"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/proben/proben-beurteilung-beurteilungsassistent-md-77fccacc"
---
# Beurteilungsassistent

# **Beurteilungsassistent**

## **Sequenz wird nicht angezeigt:**

- Probe ist vorgemerkt, siehe Probendetailansicht
- BU-Sequenz wurde schon gestartet, siehe Menü
- Dienstlistungsparameter vorhanden (z.B. bei Freigaben)

Bedingungen in der Sequenz passen nicht zur Proben:
- DE Sequenz fehlt noch oder wurde nicht abgeschlossen (bei Duplikaten fehlt der Log der DE-Sequenz, daher DE nochmal durchführen), siehe Menu / Sequenz
- Probe hat abweichenden Umfang und taucht daher nicht in der erwartetenden Sequenz auf.
- generell keine passende Sequenz vorhanden, sollte eher die Ausnahme sein, Rücksprache mit Sarah Peter

## **Probe wird nicht angezeigt:**

- Kundenfilter wirksam, siehe Beurteilungsassistent
- Probe wurde noch nicht geprüft, Filtereinstellung
- ist ein Duplikat, hat nicht funktioniert Juni 2023, da bei Duplikaten der gedruckt/vorgemerkt Haken gesetzt wird
- ist eine Korrektur, sollte eigentlich kein Problem bereiten

## **Einzelne Sollwerte löschen:**

Das geht aktuell über die BU V01 und auch erst wenn du die erste Aktion abgeschlossen hast. Ansonsten aktualisiert der Assistent die Grenzwerte und packt sie dir wieder rein.
Also folgendes Vorgehen:

- Erste Aktion abschließen (Aktion mit Produktgruppe einfügen, in der Regel die erste Aktion)
- Sollwerte löschen
- beurteilen

## **Fehlermeldungen**

### Fehler: Die Sequenz enthält mehr als ein Element.

- Hier wurde bei einer Probe die Sequenz doppelt gestartet.
> - Suche aktuelles Jahr/Monat für jede Sequenzvorlage einzeln, bis die "fehlerhafte" Sequenz gefunden ist
> - Suche ggf. über Probeingangsdatum und dann Probenbezeichnung weiter einschränken
> - Suche in Datenerfassung / Beurteilung ... über Auftrageber oder Sequenz nach Probe mit doppelter Sequenz.
> - eine Sequenz löschen

### Fehler: Der Objektverweis wurde nicht auf eine Objektinstanz festgelegt.

- hier ist ein Pflichtfeld leer oder falsch
> - Konsistenzcheck für leeren Auftrageber <externer Link entfernt> 
- Sonderzeichen in Sequenznamen: Es gibt Zeichen, die in bestimmten Kontexten problematisch sein können, wie ?, &, =, !, @, $, (, ), +, ,, ;, : und *. Diese Zeichen haben spezifische Funktionen in URLs (z.B. zur Trennung von Parametern). S. <externer Link entfernt>

### Fehler: Ein Aufrufziel hat einen Ausnahmefehler verursacht.

- hier ist ein Logikfeld mit Auswahlliste falsch befüllt
- z.B. Berichtsname mit Tagebuchnummer gefüllt statt mit LLSpaltenEnglisch oder Prüfbericht CR 2008

### Fehler: ChangeTrackerListtemid instrumentelle Analytik already exists

- in Probendetailansicht
- hier ist die Probe wurde 2x in der Zerkleinerung Nachverfolgung eingetragen .

### Fehler: String or binary data would be truncated

- HMLims Feld ist Zeichenanzahl beschränkt: z.B. Kundenprobenummer, Prüfkategorie, Probenbezeichnung

### Fehler: Ungültige Aktion für Parameterbedingung

- Fehler war nicht reproduzierbar, ggf fehlerhafter BB
- Abhilfe: Sequenz löschen und neu starten
