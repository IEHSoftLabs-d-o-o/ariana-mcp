---
type: "Anwenderwissen"
title: "Kunden-Exporte"
description: "Anwenderhinweise zu Kunden-Exporte."
tags: ["klims", "anwender", "allgemein"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/allgemein/kis-md-c9102e50"
---
# Kunden-Exporte

# Kunden-Exporte

* Grundlage der Exporte sind die Daten IM Prüfbericht
* das Layout der Excel-Exporte wird über den Punkt Exportdarstellungen Auswertungsvorlage festgelegt. Letztere wird unter Stammdaten / Auswertungen / Vorlagen erstellt bzw. gepflegt (z.B. Freigaben: Stockmeyer Teewurstfreigabe pebr). Dazu werden die entsprechenden Auswertungsfelder in der Auswertungsvorlage hinterlegt
* Änderungen hinsichtlich Versandzeitpunkt oder bereits exportierte nochmal exportieren, werden nur sofort übernommen, wenn die Änderung auf <externer Link entfernt> erfolgt. Bei Klims 2 bzw. 3. werden die Änderungen erst über Nacht aktiv.
* Änderungsindex wird automatisch erstellt, Wiederherstellung möglich
* Verwendung: Prüfberichtversand, Versand von Ergebnistabellen, weitere Zusatzfelder in Rechnung angeben

Feldtypen:

* Kopffeld äquivalent zu den Kopfdatenfeldern, sollten eigentlich fast alle bereits gepflegt sein
* Zusatzfeld äquivalent zu den zusatzfeldern (verfügbare Gruppe ist Hmlims-Relikt)
* Ergebnisfeld: hier können mehrere Parameter angegeben werden, dem ersten verfügbaren wird das Ergebnis entnommen
* Konstante gibt festen Text aus, z.B. bei Dateinamen
* IfNull, hier kann eine Reihe definiert werden, erster vorhandener, nicht leerer Wert wird verwendet. z.B. Charge
* Translation, hier kann ein Ergebnis zu einem anderen Ergebnis geändert werden, z.B. verdächtig zu Verdacht

Bedingungen: 

* Parameter existiert, Ergebnis egal (auch leer) = % (<externer Link entfernt>)
* Parameter existiert, Ergebnis vorhanden = %_% (<externer Link entfernt>)

### ausgeschlossene Proben

* Abrechnungsproben: reine Abrechnungsproben werden von unserem automatischen Exportern ausgeschlossen. Wir identifizieren diese anhand des Prüfpakets, d.h. wenn nur Prüfpakete in der Probe sind, welche mit „D …“ beginnen, werden diese ausgeschlossen. 
* weitere möglich ggf bitte ergänzen

## Probenauswahl

Status

* Manuelle Exportfreigabe: dieser Statusfilter schaut auf einen Verfolgungseintrag „23100“ – Freigabe für Export. wurde bei uns seit 2019 nicht mehr verwendet und konnte anscheinend auch nur im HMLIMS erzeugt werden. Ich glaube das ist eine verweiste Option …

## Erweiterte Einstellungen

Simultan

* Standardeintrag kein Haken

no group

* Standardeintrag "Standard"
* Freigabe und manuellen Exportern z.B. abweichende Einstellung
