---
type: "Anwenderwissen"
title: "Corona"
description: "Anwenderhinweise zu Corona."
tags: ["klims", "anwender", "sonstiges"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/sonstiges/sonstiges-corona-md-5f0c9e30"
---
# Corona

# **Corona**

### **Corona Vorproben:**
_siehe hier: <externer Link entfernt>

### **Corona Übersicht:**
- Suche erfasst alle Covid Parameter entgegen der Auswahl (P Cov..., P Cov... 5er, P Cov...10er)
- Suche erfolgt auch auf Unterproben-Ebene, d.h. der Storno- oder Etikettiert-Haken muss sowohl in der Haupt- als auch in der Unterprobe gesetzt sein. Fehlt der Haken einmal, wird die Probe als nicht-Etiekettiert bzw. nicht-Storniert angezeigt.

## **Corona Probenversand**
### **Allgemeine Infos**
**Reiter Standard:** 
- etikettierte Proben, die geprüft wurden und ein Ergebnis haben, erscheinen zum Verschicken
- nicht etikettierte Proben, die geprüft wurden und ein Ergebnis haben, erscheinen zum Verschicken
- Proben mit dem Paket PCR Covid-19 S-Gen Mutationen erscheinen nicht zum Verschicken
- Consistency Check: “gestern fertiggemeldete” Proben ohne Etikettiert-Haken an Jürgen, Lucia, Peter und Lena (es genügt bei den Proben vom Check das Archiv zurückzusetzen, den Etiketten-Haken zu setzen und sie wieder zu archivieren; dann erscheinen sie auch in der Coronaübersicht und der verantwortliche SV bleibt in der Probe;)
- Fehlermeldung bei Bearbeitung beachten (Ausrufezeichen und letzte Spalte, Grund wird angegeben und ist zu bearbeiten)

**Farbbedeutung:**
- weiß: Mailversand in angegebener Sprache und fertigmelden
- grün: Excelversand, ggf. vereinzelt Mailversand, wenn in BU-Info angegeben, und fertigmelden
- hellblau: prüfen, ggf. Kunden informieren und fertigmelden. Kein Manueller Versand. (va. Krankenhaus SAD)
- pflaume: fertigmelden (umfasst ausschließlich Portalproben aus zuverlässiger Quelle)

**Reiter Poolauflösung:**
- Proben mit Poolauflösungen werden hier angezeigt
- per Excelversand verschicken
- betroffene Proben werden in BU-Info mit "vorab versendet" markiert

**Reiter nachträglicher Versand:**
- hier Suche und Versand fertiggemeldeter Proben möglich (keine Poolauflösungen Vorab Excel möglich)
- kein Berechnen der Formeln und kein Setzen des Vorgemerkt / Gedruckt Hakens

## **Fehlermeldung:**
_erkennbar über Ausrufzeichen und Kommentar in Fehlerspalte beachten und bearbeiten_

**Sammelmail-Versand über über PA-Corona-Mail-Adresse:**
- verschickt gleichzeitig an verschiedene Mailadressen (z.B. 10 Tagebuchnummer mit verschiedenen Mailadressen)
- verschickt gleichzeitig an gleiche Mailadressen mit maximal 20 PDF je Mail (z.B. 140 Prüfberichte eines einzelnen Kunden)
- E-Mail-Adresse wird aus Zusatzfeld gezogen, nicht aus dem Stammdaten.

### **Versand korrigierter Prüfberichte bez. CoronaWarnApp (Erzeugung eines neuen QR-Codes)**
- geht nur bei Suffix-Proben
- Inhalt des Zusatzfeldes "CoronaWarnApp QR-Code" löschen
- speichern
- <externer Link entfernt> ausführen
- prüfen und Probe per Mail und Exporte erneut versenden (<externer Link entfernt>)
- Kunde muss korrigierten mit neuem QR-Code einlesen

## **Corona Spezialuntersuchungen:**
**Reihentestung angeordnet durch Gesundheitsamt**
- wird über KVB abgerechnet, richtige Auftragsvorlage (wegen Zusatzfelder für Abrechnung und Rechnung an) verwenden!
- Auftraggeber und Prüfbericht an ist die Firma, wo der Reihentest durchgeführt wird.
- OEGD Scheine müssen vorliegen oder nachgereicht werden, bei Firmen nicht zwingend, wenn schriftlich bestätigt wird, dass GA Reihentestung angeordnet hat. pebr luku 19/10/2021
- Untersuchungen auf SARS-CoV2 Mutationen (PCR CoVID-19 S-Gen Mutationen)
- Proben tauchen nicht im Corona Probenversand auf
- Ergebniseintrag: Wildtyp; alpha = N501Y, A570D, B.1.1.7; N501Y, B.1.351; L452R, P681R = delta; N501Y, K417N = B.1.1.529 (Omikron)
- nicht bestimmbare Proben: Ergebniseintrag technisch nicht durchführbar
- Meldung an LGL: täglicher Konsistenzcheck Mutationsanalysen: gestern fertiggemeldete Proben (hier nur "bayrische Proben" weitergeben)
- Meldung an Demis (automatisch)
- Abfrage für im aktuellen Jahr fertiggemeldete Proben aller Kunden mit Paket PCR CoVID-19 S-Gen Mutationen: <externer Link entfernt> 

### **Anordnung LGL Dezember 2021 wegen Omikron:**
_derzeit alle Kunden mit positiven Proben laut LGL mit Ausnahme KKH SAD, hier noch manuell_

_Damit die Abrechnung der Mutationsanalysen klappt, muss bei allen Kunden, wo Rechnungsempfänger ungleich KVB und ungleich KKH, in die Rechnungsinfo der Hinweis „vPCR an KVB abrechnen“ ergänzt werden. Damit keine Proben hier durchrutschen, legen wir für die Sachverständigen einen Konsistenz-Check an, welcher Mutationsanalysen mit Rechnungsempfänger != KVB ausgibt. Diesen Check hab ich schon soweit angelegt: Mutationsanalysen mit fehlender Rechnungsinfo "vPCR an KVB abrechnen" (es fehlen noch die E-Mail-Empfänger, bzw. ein eingrenzender Zeitraum)_

Bzgl. der Abrechnungsprobe müssen wir wohl nichts machen, da diese bei der Abfrage „Exportdaten für KVB - München - SARS-CoV-2 (Formular MUSTER 10 98055)“ dabei sein müssten.

### **Untersuchungen aufgrund Anordnung Gesundheitsamt Schwandorf:**
1. Testzentrum alle Kunden: Die Mutationsanalysen werden bei weiteren Kunden z.B. Ankerzentrum nachgelegt, insgesamt bei { "Rechnungsempfaenger": [KVB: "18815", Anker: "110407",Testzentrum: "110363", "110364", "110859", Wünsche:"11970"] }. KVB (hier Zusatzfeld 98055-Proben aber ausgenommen); Rechnungsempfänger KVB trifft auch KKH MA Proben zu 01/03/2021)
1. Proben werden nach Ergebniseintrag einmal täglich abends automatisch fertiggemeldet (wenn = { "Rechnungsempfaenger": ["18815","110363", "110364", "110859"] KVB, 3x Testzentrum SAD}.
1. Proben (gestern und heute fertiggemeldeten Mutationsanalysen) werden über Kundenexport Mutationsergebnisexport im 30-Minuten-Takt ins Testzentrums-Portal hochgeladen

### **Untersuchung im Kundenauftrag oder im Auftrag eines weiteren Gesundheitsamtes:**
- Erstellung einer Korrektur und nachlegen der Untersuchung durch SV
- bei Auftrag durch Gesundheitsamt: Vermerk in manuelle Abrechnungsprob (s.u.) + Vermerk in R-Info: Mutation vom GA beauftragt, nicht abrechnen (GA XY) Datum Kürzel)

**Versand / Fertigmelden**
_Abfrage: <externer Link entfernt> oder täglich Konsistenzcheck: Fertig zu meldende Mutationsanalysen_
- Untersuchung im Kundenauftrag --> manuell verschicken und fertigmelden
- Kundenbericht über Mutation über Gesundheitsamt beauftragt: an Kunden verschicken, fertigmelden und automatisch über DEMIS
- alle andere Prüfberichte, die Testzentrum als Rechnungsempfänger haben, werden automatisch fertiggemeldet. 

### **Abrechnung**
1. Kunde
1. Kundenbericht Mutation über Gesundheitsamt beauftragt (nicht SAD, manuelle Abrechnungsprobe + Vermerk in R-Info: Mutation vom GA beauftragt, nicht abrechnen (GA XY) Datum Kürzel)
1. Testzentrum via RE = KVB
1. Testzentrum andere RE (manuelle Abrechnungsprobe)
- über Auftragsvorlage: Abrechnung SARS-CoV-2 Mutationen manuelle Abrechnungsprobe erstellen
- hier je untersuchter Probe Unterprobe ergänzen (mit Hilfsparameter „Probennummer“, Hilfsparameter „Identifikation ÖGD PLZ“ = PLZ anforderndes Gesundheitsamt, Parameter „P CoVID-19 S-Gen Mutationen“ --> leer abhaken)
- Abrechnungsprobe kann dann nach und nach ergänzt, fertiggemeldet und abgerechnet werden. 
- über Abfrage Abrechnung Mutationen über KVB: Hier werden alle Proben aufgelistet, die Mutationsanalysen enthalten und auf den Rechnungsempfänger „Testzentrum nicht abrechenbar – Schwandorf“, „Testzentrum Landratsamt Grenzpendler Schwandorf“ oder „Testzentrum Landratsamt – Schwandorf“ laufen und die im aktuellen Kalendermonat fertiggemeldet wurden. Für diese Proben werden dann am Ende des Monats auch jeweils einmal der Parameter in die Abrechnungsprobe gelegt und ggf. ein Screenshot an die Probe angehängt werden, dass wir im Nachhinein wissen, welche Proben abgerechnet wurden.

### **Sequenzierung Genom SARS-CoV2 (PCR NGS Sequencing Covid-19):**
- Ct-Wert <25 in der Regel notwendig
- Untersuchung in der Regel einmal pro Woche, Dauer ca. 30h
- Abfrage über alle Sequenzierungsergebnisse: wie gestalten, da Eintrag in Lims nur ja?

### **Untersuchungen aufgrund Untersuchungsverordnung:**
- zufällig ausgewählte positive Proben (Anzahl max 5-10% der positiven der letzten Untersuchungswoche) durch PL
- Erstellung einer Suffixprobe durch PL
- Rechnung an KVB
- Fertigmelden der Probe durch IT (Markus trägt in die Tagebuchnummern den IMS Identifier ein und macht die Proben fertig, Abfrage für offene ohne IMS <externer Link entfernt>)
- Meldung an RKI über PL (manuell dann automatisch)
- Meldung an LGL via Exceltabelle (Y:\BUL\Arbeitsgruppen\Gemeinschaftsordner\100_Diverses\HP_LJ_1536_DNF_MFP\Meldungen LGL)

### **Untersuchungen aufgrund Wunsch Gesundheitsamt Schwandorf:**
- schriftliche Beauftragung mit allen notwenigen Daten sowie der Bestätigung der Kostenübernahme bei nicht ausreichender Qualität des Ergebnisses.
- Untersuchung und Abrechnung über Untersuchungsverordnung, soweit aufgrund der Qualtiätsmaßstäbe des RKIs möglich.

### **Untersuchungen aufgrund Wunsch weiterer Gesundheitsämter:**
_bei Labor Kneißler in der Regel nicht möglich pebr 06.10.2021_

### **Untersuchungen im Kundenauftrag**
- Erstellung einer Korrektur und nachlegen der Untersuchung durch SV
- Rechnung an Kunden
- Fertigmelden der Probe durch SV, derzeit im Corona Probenversand, ggf. analoges Vorgehen zu Mutationsuntersuchungen)
- Meldung an RKI über PL (manuell dann automatisch)? klären
- Meldung an Demis ? klären

### **Untersuchung mittels zweiter Methode bei positiven Proben (Ankerzentrum)**
- Die erste Methode wurde untersucht: Probe positiv, zweite Methode wird durch PCR nachgelegt
- Probe wird in der Corona Versandansicht angezeigt; in der Spalte „Fehler“ wird der noch offene Parameter mit der zweiten Methode angezeigt
- Der Mitarbeiter hat dadurch jetzt die Möglichkeit eine Excel zu versenden
- Probe darf nicht fertiggemeldet werden
- Die Ankerproben bleiben in der Ansicht stehen
- Wenn der Parameter mit der zweiten Methode fertig ist, dann wird die Probe weiterhin angezeigt (aber jetzt ohne Fehler)
- Der PB und Excel werden jetzt versendet
