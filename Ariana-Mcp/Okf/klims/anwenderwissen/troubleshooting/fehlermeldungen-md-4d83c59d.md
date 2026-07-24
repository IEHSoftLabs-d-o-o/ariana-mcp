---
type: "Anwenderwissen"
title: "Fehlermeldungen und Hilfe"
description: "Anwenderhinweise zu Fehlermeldungen und Hilfe."
tags: ["klims", "anwender", "troubleshooting"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/troubleshooting/fehlermeldungen-md-4d83c59d"
---
# Fehlermeldungen und Hilfe

## Allgemein

##Probenanlage: 

### Fehler: Der Wert darf nicht NULL sein. Parametername: s
- Mitarbeiter hat evtl einen schon aktivierten Kundenauftrag aufgerufen.

##Sequenzen:

### Fehler: Die Sequenz enthält mehr als ein Element. 

- Bei Sequenzen: Hier wurde bei einer Probe die Sequenz doppelt gestartet.
> - Suche aktuelles Jahr/Monat für jede Sequenzvorlage einzeln, bis die "fehlerhafte" Sequenz gefunden ist
> - Suche ggf. über Probeingangsdatum und dann Probenbezeichnung weiter einschränken
> - Suche in Datenerfassung / Beurteilung ... über Auftrageber oder Sequenz nach Probe mit doppelter Sequenz.
> - eine Sequenz löschen
- bei SEBAM Export 
> - sind Proben mit verschiedenen Empfängern ausgewählt.
- bei Sensorikergebniserfassung

###Fehler: Die Sequenz enthält mehrere übereinstimmende Elemente.

- Bei Auftragsaktivierung 
>- doppeltes Zusatzfeldattribut
>- Produktklasse doppelt gepflegt
>- Singleton Produktbeschreibung: Artikelnummer doppelt gepflegt

### Fehler: Der Objektverweis wurde nicht auf eine Objektinstanz festgelegt.

- hier ist ein Pflichtfeld leer oder falsch (Probenbezeichnung, fehlendes Normwerk)
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

##NIR Import: 
###Fehler: Die Datei x konnte nicht gefunden werden.
- Ein Prozess der für die xls Dateien zuständig ist funktioniert nicht mehr ordnungsgemäß und muss neu gestartet werden. Am einfachsten ist es hier den PC einfach herunterfahren(nicht neu starten) und wieder hochzufahren.
