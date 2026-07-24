---
type: "Anwenderwissen"
title: "Kunden-Exporte"
description: "Anwenderhinweise zu Kunden-Exporte."
tags: ["klims", "anwender", "kunden"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/kunden/kunden-importe-und-exporte-kunden-exporte-md-25682edc"
---
# Kunden-Exporte

# **Kunden-Exporte**
### **Kunden-Exporte**:
- Grundlage der Exporte sind die Daten IM Prüfbericht
- Der Prüfbericht wird an die Mailadressen, die in den Exporteinstellungen der Stammdaten des Auftraggeber hinterlegt sind, versendet, die Adresse im Prüfbericht wird aus Prüfbericht an gezogen.
- Durchschlag an Logik hier abgebildet?
- das Layout der Excel-Exporte wird über den Punkt Exportdarstellungen Auswertungsvorlage festgelegt. Letztere wird unter Stammdaten / Auswertungen / Vorlagen erstellt bzw. gepflegt (z.B. Freigaben: Stockmeyer Teewurstfreigabe pebr) Dazu werden die entsprechenden Auswertungsfelder in der Auswertungsvorlage hinterlegt
- Änderungen hinsichtlich Versandzeitpunkt oder bereits exportierte nochmal exportieren, werden nur sofort übernommen, wenn die Änderung auf _http://k-lims-1/_ erfolgt. Bei Klims 2 bzw. 3. werden die Änderungen erst über Nacht aktiv.
- Änderungsindex wird automatisch erstellt, Wiederherstellung möglich
- Verwendung: Prüfberichtversand, Versand von Ergebnistabellen, weitere Zusatzfelder in Rechnung angeben

[28.12.2023 17:08] Stefan Jünger 19:40, 21:30 und 23:40 und dann 4:40 18.03.2024

- Sonderexprot für KiS zum Löschen von falsch exportierten Proben: <externer Link entfernt> 
Wenn man hier auf ausführen geht, eine Tagebuchnummer eingibt und ausführt, so wird diese Nummer aus KIS gelöscht. Anwendbar beispielsweise für Korrekturen, bei denen der Auftraggeber falsch gewählt wurde, und die Datensätze somit ins falsche KIS gelangt sind.

### **Feldtypen:**
- Kopffeld äquivalent zu den Kopfdatenfeldern, sollten eigentlich fast alle bereits gepflegt sein
- Zusatzfeld äquivalent zu den zusatzfeldern (verfügbare Gruppe ist Hmlims-Relikt)
- Ergebnisfeld: hier können mehrere Parameter angegeben werden, dem ersten verfügbaren wird das Ergebnis entnommen
- Konstante gibt festen Text aus, z.B. bei Dateinamen
- IfNull, hier kann eine Reihe definiert werden, erster vorhandener, nicht leerer Wert wird verwendet. z.B. Charge
- Translation, hier kann ein Ergebnis zu einem anderen Ergebnis geändert werden, z.B. verdächtig zu Verdacht

### **Bedingungen:** 

- Parameter existiert, Ergebnis egal (auch leer) = % _(<externer Link entfernt>)_
- Parameter existiert, Ergebnis vorhanden = %_% _(<externer Link entfernt>)_
- Zusatzfeld mit bestimmten Inhalt siehe z:B. Aldi Süd Lieferanten PDF Emails

### **ausgeschlossene Proben:**

- Abrechnungsproben: reine Abrechnungsproben werden von unserem automatischen Exportern ausgeschlossen. Wir identifizieren diese anhand des Prüfpakets, d.h. wenn nur Prüfpakete in der Probe sind, welche mit „D …“ beginnen, werden diese ausgeschlossen. 
- weitere möglich ggf bitte ergänzen

### **Probenauswahl:**
**Status:**

- Manuelle Exportfreigabe: dieser Statusfilter schaut auf einen Verfolgungseintrag „23100“ – Freigabe für Export. wurde bei uns seit 2019 nicht mehr verwendet und konnte anscheinend auch nur im HMLIMS erzeugt werden. Ich glaube das ist eine verweiste Option …

### **Erweiterte Einstellungen:**

**Simultan**
- Standardeintrag kein Haken

**no group**
- Standardeintrag "Standard"
- Freigabe und manuellen Exportern z.B. abweichende Einstellung

**Fehler**
Lösung für folgenden Fehler beim Speichern der Exportvorlage: strg + F5
