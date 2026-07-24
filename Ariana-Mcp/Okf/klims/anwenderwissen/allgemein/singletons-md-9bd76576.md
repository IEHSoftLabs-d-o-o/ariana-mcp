---
type: "Anwenderwissen"
title: "Singletons"
description: "Anwenderhinweise zu Singletons."
tags: ["klims", "anwender", "allgemein"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/allgemein/singletons-md-9bd76576"
---
# Singletons

#**Singletons**
Jokerzeichen (verwendbar in Paketen, Parametern, Preisgruppen):
- einzelne Zeichen werden als ? gejokert
- beliebige Zeichenfolgen als *
- * = irgendwas beliebiges: „P Huhn*“ berücksichtigt Parameter „P Huhn“ und „P Huhn (IEH)“
- | = oder: „P Rind|P Schwein“ berücksichtigt Parameter „P Rind“ oder „P Schwein“ in der Probe, „P Rind*|P Schwein“ berücksichtigt Parameter „P Rind“, „P Rind (IEH)“ oder „P Schwein“ in der Probe

## **ConfigurableProbenanlagePostprocessorConfiguration:**
- Konfiguration muss an letzte Position
- alle Postprocessor werden bei der Aktivierung des Auftrags zur Probe angewendet.

## **CoronaKonfiguration::**
dem Kunden werden über die Kneißler Kundennummer email-Adresse(n) und Auswertungsvorlagen zugeordnet.

Hinweis zur Konfiguration der Auswertungsvorlage für Poolauflösungen:

- Entsprechende Auswertungsvorlage erstellen: z. B. „Allgemein: Corona-Untersuchungen (PN-Datum, Name, Vorname)“ kopieren und im Namen „_Poolaufloesung“ dranhängen.
- Die neue Auswertungsvorlage „Allgemein: Corona-Untersuchungen (PN-Datum, Name, Vorname)_Poolaufloesung“ anpassen, d.h. in der Regel die Ergebnisparameter entfernen und Konstante: SARS-CoV-2 in Poolauflösung einfügen
- Auswertungsvorlage „Allgemein: Corona-Untersuchungen (PN-Datum, Name, Vorname)“ in CoronaKonfiguration hinterlegen. Das Hinterlegen der ..._poolauflösung ist nicht notwendig.
- Abhängig davon, in welchem Tab der Coronaversand-Ansicht man arbeitet (Standard|Poolauflösung), sucht sich das System autom. die korrekte Auswertungsvorlage.

Hinweis zu automatischen Fertigmelden von Portalproben (Testzentrum, Touristik...)

- Unter automatisches Fertigmelden können die Kundennummern hinterlegt werden, für die Coronaproben automatisch fertig gemeldet werden sollen
- Wenn wie besprochen keine Fehler bei der Coronaprobe auftauchen.
- Unter Verantwortlich kann konfiguriert werden, wer als Verantwortilcher auf dem PB drauf stehen soll (kann auch leer gelassen werden, dann steht niemand drauf).

## **KundeFussnotenKonfiguration (hochgestelltes K):**

die Kopfdaten, Zusatzfeldern, Parametern, die mit einem hochgestellten K gekennzeichnet werden sollen, werden festgelegt.

- Einstellung Kunde: Daten werden immer vom Kunden zur Verfügung gestellt. Es werden alle auf "Kunde" eingestellten gekennzeichnet
- Einstellung Labor: Zusatzfeldkonfiguration, ist die Logik getauscht. Es werden alle auf "Labor" eingestellten nicht gekennzeichnet.
- Einstellung Einsender: variable Kennzeichnung. Es werden alle auf "Einsender" eingestellten in Abhängigkeit vom Kopfdaten Feld Probennehmer gekennzeichnet (ist hier "Kneißler" enthalten: keine Kennzeichnung)
- Fehler bei Prüfberichtserstellung tauchen auf, wenn generell Duplikate vorhanden sind (auch zwischen den drei Gruppen)

Folgende Daten sind doppelt vorhanden und wurden dann unter Kopfdaten gepflegt

- elektrische Leitfähigkeit bei 25 °C (vor Ort) ?
- Hersteller (sowohl in Parameter als auch Kopfdaten)
- Probenahmedatum (sowohl in Zusatzfelder als auch Kopfdaten)
- Probenahmeort (sowohl in Zusatzfelder als auch Kopfdaten)
- Probenart (sowohl in Zusatzfelder als auch Kopfdaten)

## **ProbenanlageListenKonfiguration:**

zwei Varianten zur Hinterlegung von Standardwerten möglich:

- Fixe Hinterlegung = PlainAttributwertListe; hier Werte manuell hinterlegen
