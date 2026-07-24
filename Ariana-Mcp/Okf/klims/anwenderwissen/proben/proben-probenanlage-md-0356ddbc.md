---
type: "Anwenderwissen"
title: "Probenanlage"
description: "Anwenderhinweise zu Probenanlage."
tags: ["klims", "anwender", "proben"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/proben/proben-probenanlage-md-0356ddbc"
---
# Probenanlage

# **Probenanlage**
_Reihenfolge der Bearbeitung:_

- Import: **Kundenaufträge => Aufträge** => Laborprobe im Klims / HMlims
- manuell: **Aufträge / Neuer Auftrag** => Laborprobe im Klims / HMlims

### **Aufträge:**
_unter "Aufträge" werden alle aktivierten Kundenaufträge angezeigt oder über neuer Auftrag kann hier manuell eine Probe, auch über eine Auftragsvorlage, erfasst werden._

- Status offen: eine Tagebuchnummer wurde im Lims noch nicht erzeugt
- Status aktiviert: eine Tagebuchnummer wurde erzeugt, die Probe ist in der Probenanlage abgeschlossen
- Auftraggeber: ggf. entsprechenden Kunden eingeben
- Probenbezeichnung: ggf. Probenbezeichnung eingeben
- Tagebuchnummer: der erzeugten Probe Klims / HMlims
- Analyse:
_hier nicht zu importierende Aufträge (z.B. fehlerhaft, doppelt aktivierte,...) löschen
Auftragsvorlagen_

### **Neuerstellung einer Auftragsvorlage:**
- wird Prüfbericht an/ Rechnung an nicht ausgefüllt wird automatisch der Auftraggeber gezogen
- wird untersuchter Anteil nicht ausgefüllt wird automatisch "Gesamterzeugnis" gezogen 
- bei mikrobiologischen Untersuchungen muss unbedingt "Grundpreis mikrobiologische Untersuchung" mit in den Umfang aufgenommen werden
- Zuordnung der Basisparameter HMLims zu den KLims Parametern in den entsprechenden Produktklassen kann unter Beauftragung|Analysenzuordnung eingesehen werden
- bei Neuerstellung einer Vorlage: QMF-PA-07 befolgen
- immer folgende Felder einfügen: .Laborinfo, .Anlagequelle
- wenn es sich immer um die gleiche Produktart handelt auch die Produktbeschreibung inkl. Konsistenz und Farbe einfügen

### **Infofelder (Zusatzfelder):**
- Datenexportrelevante Felder: Infofelder können frei definiert werden. Die Hinterlegung aus dem HMlims ist vorhanden. Diese vorrangig verwenden. Die Infofelder werden als Zusatzfelder ins HMLims übernommen. 
- mit der Tabulatortaste springt man von Eingabefeld zu Eingabefeld

### **Attribute (Infofelder) die mit:**
- "#" werden als allgemeiner Kommentar angezeigt; 
- "#xx", wobei "xx" dem exaktem Wortlaut eines Attributs entspricht, werden als spezieller Kommentar unter dem entsprechenden Eingabefeld angezeigt
- "#xx", wobei "xx" keinem Wortlaut eines Attributs entspricht, wird als formatierter, allgemeiner Kommentar "xx: Kommentar angezeigt"
- "Produktbeschreibung|..." wird als Zusatzfeld der genannten Zusatzfeldgruppe übernommen.

### **Attribute (Hilfsparameter):** 
* Splitfunktion in Vorlage vor hinterlegen mittels eckiger Klammer auf zu [] im rechten Feld -> Attribute (Hilfsparameter) entsprechend benennen und rechts [] eintragen -> bei Probenanlage ist direkt der Splithaken gesetzt
* .PrNr Etikett wird auf Parameteretikett gedruckt (stand 28.02.22024 pebr auch UP-Etikett möglich?)
* Splitfunktion manuell: eine automatische fortlaufende Durchnummerierung von 11 bis 20 ist möglich, wenn vor Setzen des Splithakens "11-20" eingegeben wird und dann der Splithaken gesetzt wird.
* Splitfunktion manuell: "V1","V2","V14","V15","V16" hier erfolgt auch ein korrekter Split

**funktionale Felder:**
- $briefpapier: z.B. 
```
	- kneissler
	- kneissler_corona
	- kneissler_human
	- kneissler_ohne_dakks
```
kneissler_corona
- $farbschema: das hier hinterlegte Farbschema wird der Probe hinzugefügt.
- $Prüfberichtssprache: z.B. EN
- $vorprobenzahl: z.B. 100
- $etikettenvariante: Inhalt des Zusatzfeldes: Kompakt:: also z.B. Kompakt:1 oder Kompakt:3 (Kompakt:1:10er); Anzahl und Info auf Barcode-Etikett editierbar: z.B. Kompakt:2:L.mono (beide Doppelpunkte wichtig)
- "$probengruppe" Attribute, die in Positionen (# 1, # 2 ...) verwendet werden und mit "$probengruppe" beginnen, fassen diese zu einer entsprechenden Probengruppe zusammen. Aus einer Probengruppe wird jeweils eine eigene Probe (Suffixprobe) erzeugt. z.B. "$probengruppe": --> "MHD", "$probengruppe": --> "Chemie"...
- Dynamische Präparation: unter erweiterte Ansicht Attribute ergänzen mit Spalte 1: $konfiguration, Spalte2: {Anteile:["Fleisch", "Brötchen"]}; ggf. weitere Anteile ergänzen

## **Sonderlogik: Wasserproben Abklatschproben...:**
**Abklatschproben:**
- bestimmte Felder werden gelöscht: z.B. Eingangstemperatur, Verpackung
- Produktklassse |Standard|Hygiene|Umfeldhygiene|Abklatsch löst Probengruppe Abklatschprobe im HMLims aus 
- pebr mawa ggf noch ergänzen und präzisieren 27/01/2021

**Wasserproben:**
- Attribut (Zusatzfeld) Probenahmeort löst in Kombination mit Produktklasse Trinkwasser Darstellung als Wasser-Prüfbericht aus pebr 27/01/2021
- Produktklassse |Standard|Wasser ... (|Standard|Wasser|Trinkwasser) löst Probengruppe Wasserprobe im HMLims aus

## **Kundenaufträge:**
_unter "Kundenaufträge" werden alle hochgeladenen, noch nicht aktivierten Kundenaufträge angezeigt. Das Hochladen erfolgt derzeit noch manuell in der Regel durch die Aufnahme._

- Status offen: die Datei wurde im Windowsexplorer im entsprechenden Ordner abgelegt und über "Kundenauftragsprofile" entsprechenden Kunden auswählen und hier über Datei-Upload ins Klims hochladen
- Status aktiviert: die Datei wurde im Klims aktiviert und findet sich jetzt zum bearbeiten unter "Aufträge"
- Identifier: bei Berns Dr. kann das Feld angewählt werden und das Kundenetikett gescannt werden. Die Probe wird automatisch ausgewählt.
- Auftraggeber: ggf. entsprechenden Kunden eingeben
- Probenbezeichnung: ggf. Probenbezeichnung eingeben

_hier nicht zu aktivierende Aufträge (z.B. fehlerhafte, doppelt hochgeladene,...) löschen
Import via Klims erfolgt hier (Reiter auf rechter Seite, m.E. nicht optimaler Ort)_
 
- Kunde ist ausgewählt, wenn hinterlegt
- falls verschiedene Auftragsprofile hinterlegt sind (z.B. allgemeine Proben / Corona-Proben) --> richtiges auswählen
- validieren: prüft Auftrag und legt Kundenaufträge nur an, wenn keine Fehler enthalten sind

_Methode:_

- bei gleichen Methoden auf Analysenebene wird diese in die Position übernommen, falls die Methode auf Positionsebene leer ist.
- Positionsmethode wird auf Analysebene übernommen, sofern die Methode auf Analysenebene nicht näher festgelegt ist.
- abweichende Methoden zwischen Position und Analyse sind möglich. Methode auf Positionsebene dient dem Analysenpaketbau (z.b. Vion ohne ph Wert, juegr pebr 18.03.2022)

_Import von Produktbeschreibung_

- Auftraggeber spezifisch in Abhängigkeit der Artikelnummer oder Prüfplan-ID
- Artikelnummer hat höhere Prio
- Hinterlegung in dem Singleton überschreibt Daten im Kundenauftrag beim Aktivieren zum Auftrag
- Klarname ist technisch nicht möglich, Filter bei Artikel und PP-ID ebenso (strg + f verwenden)
- Import der Daten ist möglich: Übernahme aus alter Konfig bzw. neuer.

_Eilig-Beauftragung_

## **Kundenauftragsprofile (besser Auftragsprofile):**
- Zuordnung / Mapping von Prüfkategorie = Untersuchungscode zu Auftragsvorlagen
- Pflege von kundenspezifischer Profile
- Pflege allgemeiner Profile (z.B. Untersuchung nach DGHM, Big7) ohne spezifische Anforderung an Zusatzfelder usw.

_unklar: wie soll logische Benennung erfolgen, derzeit Kundenauftragsprofil = Kunde = Kundenprofil
fehlt: Kommentarspalte in Übersicht_

zusätzliche Attribute:
- ermöglicht das Hinterlegen von Attributen, die vom Kunden so nicht mitgeliefert werden
- Duplizieren eines Zusatzfeldattributs: z.B. #Nummer {{Probennummer}}
- Übernahme/ Standardhinterlegung in Kopfdaten Feld: siehe Fotos
- Standardhinterlegung in einem Zusatzfeldattributs: z.B. .Einsender Müller - Meierhausen

### **Kundenprofile:**
- Zuordnung welche allgemeine bzw. kundenspezifische Auftragsprofile bei einem Kunden ausgewählt werden können.

_Bug: Filter/Sortierung Auftraggeber geht nicht_

### **Auftragsvalidierung (/<externer Link entfernt>):**
_hier wird kundenspezifisch hinterlegt, welche Eingabefelder beim Wiederverwenden der Auftragsvorlage gelöscht werden.
Hinterlegungen in Name "Default" gelten für alle Auftragsvorlagen, es sei denn für einen Kunden (=Name) ist etwas anderes konfiguriert._

- die hinterlegte Auswahlliste ist nicht abschließend, Freitext ist möglich (die exakte Wortlaut aus der Auftragsvorlage ist zu treffen)
- in der Auswahlliste sind u.a. folgende Kategorien Infos.xx, Attribute.xx, Positionen.xx und Kopfdaten (z.B. Anzahlproben) hinterlegt (Probenbezeichnung kann als Freitext ergänzt werden.)
- Kategorien, die auf "." enden sind zu ergänzen (z.B. Attribute.Los, Attribute..Probennummer (hier tatsächlich mit zwei Punkten), d.h. das zu löschende Feld ist näher zu charakterisieren.

**Erweiterung um Auftragvorlagen-spezifische Valdierungen:**

Prio der Anwendung:
1.  AV-spezifisch
2.  Kundenspezifisch
3.  default    
  
Beispiele:

*   Für Testkunde nicht vorhanden, für AV vorhanden --> Nur Felder, die in AV hinterlegt sind weder gelöscht

*   Für Testkunde vorhanden, für AV vorhanden --> Nur Felder, die in AV hinterlegt sind werdenr gelöscht; d.h. die AV-Valdierung übersteuert die kundenspezifische Validierung

*   In einer Validierung Testkunde und AV gepflegt --> die AV-Name übersteuert die kundenspezifische Hinterlegung; **ABER** als Anezigename wird Kunde verwendet!

**Fazit**: AV spezifische Pflege sticht immer!

### KomplettFremdvergabePostprocessor bei IFU
unmittelbar nach der Probenerzeugung wird der „KomplettFremdvergabePostprocessor“ angestoßen, welcher die Probe pauschal auf eine Fremdvergabe an Labor Kneißler umstellt. Bei dieser Aktion werden alle Parameter aus der Probe entfernt, welche nicht im Prüfbericht gedruckt werden. Die exakte Bedingung lautet:
kein Abrechnungsparameter + nicht im Prüfbericht drucken + kein Hilfsparameter

##Fehlermeldungen: 

FEHLER: Sie Sequnez enthält mehr als ein übereinstimmendes Element.
--> Felder (z.B. Zusatzfelder sind doppelt vorhanden: in AV, AV vs. KV).
FEHLER: Der Wert darf nicht NULL sein. Parametername: s
--> Mitarbeiter hat evtl einen schon aktivierten Kundenauftrag aufgerufen.
