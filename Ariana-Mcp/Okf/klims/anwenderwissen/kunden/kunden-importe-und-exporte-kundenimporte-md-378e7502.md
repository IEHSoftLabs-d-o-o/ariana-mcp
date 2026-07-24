---
type: "Anwenderwissen"
title: "Kundenimporte"
description: "Anwenderhinweise zu Kundenimporte."
tags: ["klims", "anwender", "kunden"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/kunden/kunden-importe-und-exporte-kundenimporte-md-378e7502"
---
# Kundenimporte

# **Kundenimporte**

 _Übersicht welche Import existieren bzw. geplant sind: Y:\BUL\Arbeitsgruppen\Kundendaten Probenannahme_Allgemeine Übersichten\Übersicht Import.xlsx_

### **Allgemeiner Import:**

- Probenanlage siehe auch entsprechend AA:
- Klims/Kunden/Allgemeiner Import: hier den entsprechenden Kunden auswählen (Kundenprobenummer (=Kneißler Debitorennummer) ist in Exceltabelle hinterlegt)
- Ergänzungen: Eingangstemperatur….
- aktuelle Exceldatei durch PAV unter Kundendaten Probenannahme/„Kunde“ abgelegt

### **Allgemeines:**

- Funktioniert über eine Exceltabelle
- Vorlage dazu liegt beim jeweiligen Kunden: Kundendaten Probenannahme/„Kunde“/Archiv/Vorlage
- Es gibt die Standardtabelle analog IFU bzw. auf Kundentabellen angepassten allgemeinen Import z.B. Kugler
- Produktbeschreibung kann bei Vorliegen einer Artikelbezeichnung über eigenständige Excetabelle (Aktualisierung durch Sensorik) (z.B. Gustoland) importiert werden; Alternative: Schlagwort? Exceltabelle (In mehrere Tabellenblätter aufgeteilt); Abmessung...,Produktbeschreibung, Farbe Konsistenz soll ggf als leere Zusatzfelder angelegt werden
- Prüfkategorie/Untersuchungscode immer in Prüfkategorie in HMLims übernehmen
- Kunde soll getrennte Aufträge für Freigaben, Wasserproben und LM-Proben und diese vorab an ausgefüllte per Mail an Probenannahme übermitteln

### **Exceltabelle: Blatt 1: Auftragsformular:**

- Diese ist über Zeile 1 (x setzen oder entfernen) und das Makro ein-/ausblenden auf die Kundenwünsche zum größten Teil konfigurierbar
- Die vorhandenen Felder werden standardmäßig als Kopfdaten, Zusatzfelder oder Hilfsparameter importiert
- Zwingend notwendige Felder sind Kundenprobenummer und die Prüfkategorie/Untersuchungscode (bzw. abweichende Untersuchung für abweichende Untersuchungen oder bei fehlendem Untersuchungscode)
- Die Zusatzfelder, die im HMlims angelegt werden sollen, müssen hier hinterlegt sein.
- Untersuchungsdatum legt fest, ob bei Mibiuntersuchungen das Eingangspaket oder das MHD-Paket verwendet wird.

### **Exceltabelle: Blatt 2: Prüfkategorie/Untersuchungscode:**

- Hier werden die Prüfkategorie/Untersuchungscode definiert, so dass diese leicht nachvollziehbar sind (Abkürzung=Code, ggf. Probenkategorie, Paket Lims, zu ergänzende, zu löschende Parameter)
- Hier aber nur Anhaltspunkt für Aufnahme, Kunde, SV, tatsächliche Zuordnung (die den Import bestimmt) erfolgt im Klims
- HINWEIS: wenn Mibi Kopf für Konfig verwendet wird, erfolgt hier eine alphabetische Sortierung der aller Parameter im HMlims; Stefan kann Sortierung in Hmlims für jeden Code einzeln vorgeben

### **Exceltabelle: Blatt 3:**

- Hier sind die Informationen für Auswahlfelder für Blatt 1 hinterlegt

### **Exceltabelle: ggf. Blatt 4:**

- z.B. QAL 
- hier werden unformatierte Datensätze (.csv) eingefügt und in Blatt 1 angezeigt

### **Neuen Import erstellen:**

- Beim Kunden Bereitschaft erfragen, ob er unser Formular verwenden will
- Auftragsformular anpassen (Zusatzfelder gemäß alten Proben)
- Prüfkategorie/Untersuchungscode festlegen und mit Kunden abklären (Codes gemäß alten Proben oder vom Kunden zusammenfassen lassen)
- Programmierer informieren und Import für neuen Kunden integrieren lassen.
- Klims-Zuordnungen hinterlegen (s.u.)
- Kunden aktuelle Tabelle zusenden und bei uns ablegen
- Test Import mit wenigen Proben
- Hinweissatz „verantwortlich für ….“ nach R Kunde/TL wird durch Stefan eingefügt

### **Klims pflegen**

- Prüfkategorie/Untersuchungscode im Klims hinterlegen (Vieraugenprinzip)
- Produkteigenschaften hinterlegen: dazu 
_Artikelliste von EDV mit Produktbeschreibung, Produktgruppe, Fleischanteil, natürlicher W/FE erstellen lassen
 diese Liste bearbeiten, so dass jede Artikelnummer nur noch einmal vorkommt, Angaben prüfen
 EDV hinterlegt diese dann im Klims; diese wird ab hier auch im Klims gepflegt_ 

- bei neuen Importer: Spaltenzuordnung durchführen (Zusatzfelder)
